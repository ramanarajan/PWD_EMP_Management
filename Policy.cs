
namespace TruLiveEncoder.WD
{
    /// <summary>
    /// Policy details
    /// </summary>
    public struct Policy
    {
        /// <summary>
        /// Policy ID
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// If policy enbled or not
        /// </summary>
        public bool Enabled { set; get; }
        /// <summary>
        /// Policy name
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Policy describtion
        /// </summary>
        public string Describtion { set; get; }
    }
}
