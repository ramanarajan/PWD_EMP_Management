
using System;
using System.Diagnostics;
using System.Threading;
namespace TruLiveEncoder.WD
{
    /// <summary>
    /// Authedication details
    /// </summary>
    public class Authentication : IAuthentication,IDisposable
    {

        /// <summary>
        /// Process in running state
        /// </summary>
        public bool IsBusy { get { if (_thmanipulation != null) return _thmanipulation.IsAlive; return false; } }

        Thread _thmanipulation = null;

        /// <summary>
        /// contructor of Authentication
        /// </summary>
        public Authentication(User _userdet)
        {
            this._sw = new Stopwatch();
            this.SessionTimeOut = 30000;
            this.UserDetails = _userdet;
        }

        Stopwatch _sw = null;
        /// <summary>
        /// Session timeout
        /// </summary>
        public int SessionTimeOut { set; get; }

        User _userDetails;
        /// <summary>
        /// User Details
        /// </summary>
        public User UserDetails
        {
            private set
            {
                _userDetails = value;
                if (!string.IsNullOrEmpty(value.Name))
                    _sw.Start();
            }
            get { return _userDetails; }
        }

        /// <summary>
        /// Manipulation
        /// </summary>
        /// <param name="_task"></param>
        /// <param name="_opType"></param>
        /// <param name="_data"></param>
        /// <returns></returns>
        public object Manipulation(int _task, int _opType, object _data)
        {
            if (_sw.ElapsedMilliseconds > SessionTimeOut)
                throw new TimeoutException("Session timeout");
             
            _sw.Reset();
            switch (_task)
            {
                case 1:
                    //var customers = (from c in db.Customers
                    //                      orderby c.CustomerName
                    //                      select c).ToList();
                    break;
                default:
                    break;
            }

            return null;
        }

        /// <summary>
        /// Manipulation asynchronously
        /// </summary>
        /// <param name="_task"></param>
        /// <param name="_opType"></param>
        /// <param name="_data"></param>
        public void ManipulationAsync(int _task, int _opType, object _data)
        {
            if (IsBusy)
                OnResponseEventRaised(new ResponseEventArgs(new DuplicateWaitObjectException("Application in busy state")));

            _thmanipulation = new Thread(new ParameterizedThreadStart(delegate(object obj)
            {
                _thmanipulation.Name = "THDBManipulation";
                try
                {
                    Manipulation(Convert.ToInt32((obj as object[])[0]), Convert.ToInt32((obj as object[])[1]), (obj as object[])[2]);
                }
                catch (Exception ex)
                {
                    OnResponseEventRaised(new ResponseEventArgs(ex));
                }
            }));
            _thmanipulation.IsBackground = true;
            _thmanipulation.Start(new object[] { _task, _opType, _data });
        }


      

        /// <summary>
        /// Event lock object 
        /// </summary>
        object ResponseEventlock = new object();
        /// <summary>
        /// Event of server response
        /// </summary>
        event ResponseEventHandle _onResponse;
        /// <summary>
        /// Event of server response
        /// </summary>
        public event ResponseEventHandle OnResponse
        {
            add
            {
                lock (ResponseEventlock)
                {
                    _onResponse += value;
                }
            }
            remove
            {
                lock (ResponseEventlock)
                {
                    _onResponse -= value;
                }
            }
        }

        /// <summary>
        /// Raised buffer progress event
        /// </summary>
        /// <param name="value"></param>
        /// <param name="_isInternet"></param>
        public void OnResponseEventRaised(ResponseEventArgs e)
        {
            ResponseEventHandle handler;
            lock (ResponseEventlock)
            {
                handler = _onResponse;
            }
            if (handler != null)
                handler(this, e);
        }
        


        #region IDisposable Implementation
        bool _isdispose = false;
        protected virtual void Dispose(bool _isdispose)
        {
            if (!this._isdispose && _isdispose)
            {
                if (_onResponse != null)
                    foreach (Delegate d in _onResponse.GetInvocationList())
                        OnResponse -= (ResponseEventHandle)d;

                Abort();
                this._isdispose = true;
                this._sw.Stop();
                UserDetails = new User();
            }
        }

        /// <summary>
        /// Dispose 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

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
                _thmanipulation.Abort();
            }

            if(_iscancel)
                OnResponseEventRaised(new ResponseEventArgs(true));
        }
    }
}
