namespace LoadBalancer.Models.Enums
{
    /// <summary>
    /// Query execution result.
    /// </summary>
    public enum QueryExecutionResult
    {
        /// <summary>
        /// Query is finished.
        /// </summary>
        QueryCompleted = 1,
        
        /// <summary>
        /// Query added to queue due to restrictions of execution.
        /// </summary>
        QueryQueued = 2,
        
        /// <summary>
        /// Query failed to execute.
        /// </summary>
        QueryFailed = 3
    }
}