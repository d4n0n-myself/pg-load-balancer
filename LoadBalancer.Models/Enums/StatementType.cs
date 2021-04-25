namespace LoadBalancer.Models.Enums
{
    /// <summary>
    /// SQL query statement type.
    /// </summary>
    /// <remarks>
    /// DML only.
    /// </remarks>
    public enum StatementType
    {
        /// <summary>
        /// Select.
        /// </summary>
        Select = 1,

        /// <summary>
        /// Insert.
        /// </summary>
        Insert = 2,

        /// <summary>
        /// Update.
        /// </summary>
        Update = 3,

        /// <summary>
        /// Delete.
        /// </summary>
        Delete = 4
    }
}