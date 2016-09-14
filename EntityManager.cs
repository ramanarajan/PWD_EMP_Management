using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TruLiveEncoder.WD
{
    public abstract class EntityManager
    {
        internal static ApplicationSettings AppSett = null;


        /// <summary>
        /// Process in running state
        /// </summary>
        public bool IsBusy { get { if (_thEM != null) return _thEM.IsAlive; return false; } }

        /// <summary>
        /// Abort current request
        /// </summary>
        public void Abort()
        {
            bool _iscancel = false;
            while (IsBusy)
            {
                _iscancel = true;
                System.Threading.Thread.Sleep(100);
                _thEM.Abort();
            }

            if (_iscancel)
                OnResponseEventRaised(new ResponseEventArgs(true));
        }

        Thread _thEM = null;

        public EntityManager()
        {
            if (AppSett == null)
                DBManipulation(0, 0, null);
        }
        /// <summary>
        /// Clear event
        /// </summary>
        protected void ResetEvent()
        {
            if (_onResponse != null)
                foreach (Delegate d in _onResponse.GetInvocationList())
                    _onResponse -= (ResponseEventHandle)d;
            Abort();
        }
        /// <summary>
        /// Event lock object 
        /// </summary>
        protected object _ResponseEventlock = new object();
        /// <summary>
        /// Event of server response
        /// </summary>
        protected event ResponseEventHandle _onResponse;

        /// <summary>
        /// Raised buffer progress event
        /// </summary>
        /// <param name="value"></param>
        /// <param name="_isInternet"></param>
        public void OnResponseEventRaised(ResponseEventArgs e)
        {
            ResponseEventHandle handler;
            lock (_ResponseEventlock)
            {
                handler = _onResponse;
            }
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        /// Manipulation
        /// </summary>
        /// <param name="_task"></param>
        /// <param name="_opType"></param>
        /// <param name="_data"></param>
        /// <returns></returns>
        internal object DBManipulation(int _taskindex,int _operation,object _data)
        {
            object _resobj = null;
            try
            {
                using (pwd_empmgmEntities1 pwd = new pwd_empmgmEntities1())
                {
                    switch (_taskindex)
                    {
                        case 0:
                            AppSett = new ApplicationSettings();
                            foreach (var item in pwd.pwd_configmaster.ToList())
                            {
                                switch (item.id)
                                {
                                    case 1:
                                        AppSett.AttemptCount = item.value;
                                        break;
                                    case 2:
                                        AppSett.AttemptInterval = item.value;
                                        break;
                                    case 3:
                                        AppSett.SessionTimeout = item.value;
                                        break;
                                }
                            }

                           
                            break;
                        /// Authentication of PWD account
                        case 1:
                            string _userName = (_data as object[])[0] as string;
                            string _pwd = (_data as object[])[1] as string;
                            var v = pwd.pwd_usercredential.SqlQuery(string.Format("SELECT * FROM pwd_usercredential where uname='{0}'", _userName)).ToList();
                            if (v == null)
                                throw new Exception("Invalid username or password!!!");
                            var res = v.GetEnumerator();
                            if (!res.MoveNext())
                                throw new Exception("Invalid username or password!!!");
                            var d = DateTime.Now - res.Current.datetime;
                            if (res.Current.invalidcount >= AppSett.AttemptCount && d.TotalMinutes <= AppSett.AttemptInterval)
                                throw new Exception(string.Format("Retry to login after {0} minutes", AppSett.AttemptInterval));
                            /// Attempt count reset
                            if (string.Compare(res.Current.pwd, _pwd, true) == 0)
                            {
                                /// Attempt count reset
                                v[0].invalidcount = 0;
                                _resobj = new Authentication(new User(res.Current.uname, null));
                            }

                            else
                            {
                                if (v[0].invalidcount >= AppSett.AttemptCount)
                                    v[0].invalidcount = 0;
                                //Increase Attempt count
                                v[0].invalidcount += 1;
                                if (v[0].invalidcount == AppSett.AttemptCount)
                                    v[0].datetime = DateTime.Now;
                                pwd.SaveChanges();
                                throw new Exception(string.Format("Invalid username or password!!! attempt{0}/{1}",v[0].invalidcount,AppSett.AttemptCount));
                            }
                            v[0].datetime = DateTime.Now;
                            pwd.SaveChanges();
                            break;
                        case 2:
                             _resobj= pwd.pwd_employeedetails.ToList();
                            break;
                        default:
                            throw new FormatException("Invalid format");
                    }
                }
                OnResponseEventRaised(new ResponseEventArgs(_resobj));
            }
            catch (Exception ex)
            {
                OnResponseEventRaised(new ResponseEventArgs(ex));
            }
            return _resobj;
        }


        /// <summary>
        /// Manipulation asynchronously
        /// </summary>
        /// <param name="_task"></param>
        /// <param name="_opType"></param>
        /// <param name="_data"></param>
        internal void DBManipulationAsync(int _task, int _opType, object _data)
        {
            if (IsBusy)
                OnResponseEventRaised(new ResponseEventArgs(new DuplicateWaitObjectException("Application in busy state")));

            _thEM = new Thread(new ParameterizedThreadStart(delegate(object obj)
            {
                _thEM.Name = "THDBManipulation";
                try
                {
                    DBManipulation(Convert.ToInt32((obj as object[])[0]), Convert.ToInt32((obj as object[])[1]), (obj as object[])[2]);
                }
                catch (Exception ex)
                {
                    OnResponseEventRaised(new ResponseEventArgs(ex));
                }
            }));
            _thEM.IsBackground = true;
            _thEM.Start(new object[] { _task, _opType, _data });
        }



       

        private object Selection()
        {
            // Create a query that takes two parameters.
//            string queryString =
//                @"SELECT VALUE product FROM 
//      AdventureWorksEntities.Products AS product 
//      order by product.ListPrice SKIP @skip LIMIT @limit";

//            ObjectQuery<Product> productQuery =
//                new ObjectQuery<Product>(queryString, context);

//            // Add parameters to the collection.
//            productQuery.Parameters.Add(new ObjectParameter("skip", 3));
//            productQuery.Parameters.Add(new ObjectParameter("limit", 5));

//            // Iterate through the collection of Contact items.
//            foreach (Product result in productQuery)
//                Console.WriteLine("ID: {0}; Name: {1}",
//                result.ProductID, result.Name);

              return null;
        }

        
    }
}
