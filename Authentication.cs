
using System;
using System.Diagnostics;
namespace TruLiveEncoder.WD
{
    /// <summary>
    /// Authedication details
    /// </summary>
    internal class Authentication : EntityManager, IAuthentication, IDisposable
    {
        /// <summary>
        /// contructor of Authentication
        /// </summary>
        public Authentication(User _userdet)
        {
            this._sw = new Stopwatch();
            this.UserDetails = _userdet;
        }

        Stopwatch _sw = null;

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
            if (_sw.Elapsed.TotalSeconds > EntityManager.AppSett.SessionTimeout)
                throw new TimeoutException("Session timeout");
            _sw.Reset();

            return this.DBManipulation(_task,_opType,_data);
        }

        /// <summary>
        /// Manipulation asynchronously
        /// </summary>
        /// <param name="_task"></param>
        /// <param name="_opType"></param>
        /// <param name="_data"></param>
        public void ManipulationAsync(int _task, int _opType, object _data)
        {
            try
            {
                this.Manipulation(_task, _opType, _data);
            }
            catch(Exception ex) {
                OnResponseEventRaised(new ResponseEventArgs(ex));
            }
        }


      

        /// <summary>
        /// Event of server response
        /// </summary>
        public event ResponseEventHandle OnResponse
        {
            add
            {
                lock (_ResponseEventlock)
                {
                    _onResponse += value;
                }
            }
            remove
            {
                lock (_ResponseEventlock)
                {
                    _onResponse -= value;
                }
            }
        }



        #region IDisposable Implementation
        bool _isdispose = false;
        protected virtual void Dispose(bool _isdispose)
        {
            if (!this._isdispose && _isdispose)
            {
                ResetEvent();
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
    }
}
