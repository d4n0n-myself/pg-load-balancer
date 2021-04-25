namespace LoadBalancer.Models.Enums
{
    /// <summary>
    /// Sql query types. Defines server pools.
    /// </summary>
    public enum QueryType
    {
        /// <summary>
        /// Query is OLAP type.
        /// </summary>
        Olap = 1,

        /// <summary>
        /// Query is OLTP type.
        /// </summary>
        Oltp = 2
    }
}