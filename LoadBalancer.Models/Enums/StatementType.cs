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
        Select = 1,
        Insert = 2,
        Update = 3,
        Delete = 4
    }
}