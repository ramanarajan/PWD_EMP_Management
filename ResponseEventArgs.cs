using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruLiveEncoder.WD
{

    /// <summary>
    /// Event handler of server response
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ResponseEventHandle(object sender, ResponseEventArgs e);

    /// <summary>
    /// Event args of server response
    /// </summary>
    public class ResponseEventArgs : EventArgs
    {
        /// <summary>
        /// Result of client request
        /// </summary>
        public object Result { private set; get; }

        /// <summary>
        /// Error on client request
        /// </summary>
        public Exception Error { private set; get; }

        /// <summary>
        /// Request cancelled by user
        /// </summary>
        public bool IsCancel { private set; get; }

        /// <summary>
        /// Result of response
        /// </summary>
        /// <param name="_result"></param>
        public ResponseEventArgs(object _result)
        {
            this.Result = _result;

        }

        /// <summary>
        /// Error on Response
        /// </summary>
        /// <param name="_error"></param>
        public ResponseEventArgs(Exception _error)
        {
            this.Error = _error;
        }

        /// <summary>
        /// Cancel of client request
        /// </summary>
        /// <param name="_error"></param>
        public ResponseEventArgs(bool _isCancel)
        {
            this.IsCancel = _isCancel;
        }

        /// <summary>
        /// full parameter of response
        /// </summary>
        /// <param name="_result"></param>
        /// <param name="_error"></param>
        /// <param name="_isCancel"></param>
        public ResponseEventArgs(object _result, Exception _error, bool _isCancel)
        {
            this.Result = _result;
            this.Error = _error;
            this.IsCancel = _isCancel;
        }

    }
}
