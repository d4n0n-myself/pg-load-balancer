namespace LoadBalancer.Models.Entities
{
    /// <summary>
    /// Server statistics holder object.
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// Server is online and responding to "GetStatistics" query.
        /// </summary>
        public bool IsOnline { get; init; }
        
        /// <summary>
        /// Active transactions count on the server.
        /// </summary>
        public int CurrentSessionsCount { get; init; }

        /// <summary>
        /// Convenient handler: get empty stats object { online = false, current sessions = 0 }.
        /// </summary>
        public static Statistics Empty => new();
    }
}