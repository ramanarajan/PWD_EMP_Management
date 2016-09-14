
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TruLiveEncoder.WD
{
    /// <summary>
    /// User Details
    /// </summary>
    public struct User
    {
        /// <summary>
        /// Constructor of user
        /// </summary>
        /// <param name="_userName"></param>
        /// <param name="_ltply"></param>
        public User(string  _userName,List<Policy> _ltply):this()
        {
            Name = _userName;
            Policy = new ReadOnlyCollection<Policy>(_ltply??new List<Policy>());
            
        }
        /// <summary>
        /// UserName
        /// </summary>
        public string Name { private set; get; }

        /// <summary>
        /// Policy details
        /// </summary>
        ReadOnlyCollection<Policy> Policy { set; get; }

        /// <summary>
        /// If policy is enable or not
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool this[int index]
        {
            get { return Policy.Any(p => p.ID == index && p.Enabled); }
        }



        
    }
    
}
