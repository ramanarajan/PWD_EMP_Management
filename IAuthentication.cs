using System;
namespace TruLiveEncoder.WD
{
    

    public interface IAuthentication
    {
        /// <summary>
        /// Event of server response
        /// </summary>
        event ResponseEventHandle OnResponse;

        /// <summary>
        /// Manipulation asynchronously
        /// </summary>
        /// <param name="_task"></param>
        /// <param name="_opType"></param>
        /// <param name="_data"></param>
        void ManipulationAsync(int _task, int _opType, object _data);

        /// <summary>
        /// Manipulation
        /// </summary>
        /// <param name="_task"></param>
        /// <param name="_opType"></param>
        /// <param name="_data"></param>
        object Manipulation(int _task, int _opType, object _data);
        
        /// <summary>
        /// User Details
        /// </summary>
        User UserDetails { get; }
        
        /// <summary>
        /// Dispose 
        /// </summary>
        void Dispose();
    }
}
