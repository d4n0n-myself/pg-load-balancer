using System.Linq;
using LoadBalancer.Domain.Models;
using TSQL;
using TSQL.Tokens;

namespace LoadBalancer.Domain
{
    public class QueryParser
    {
        private readonly TSQLKeywords[] _supportedStatements = 
            {TSQLKeywords.SELECT, TSQLKeywords.INSERT, TSQLKeywords.UPDATE, TSQLKeywords.DELETE };

        private StatementType[] _modifyingStatementTypes =
        {
            StatementType.Insert,
            StatementType.Update,
            StatementType.Delete
        };
        
        public Query ParseQuery(string query)
        {
            var tokens = TSQLTokenizer.ParseTokens(query).ToList();

            var keywords = tokens
                .FindAll(x => x is TSQLKeyword) // && _supportedStatements.Contains(keyword.Keyword))
                .Cast<TSQLKeyword>()
                .ToArray();

            var statements = keywords
                .Select(x => Map(x.Keyword))
                .Where(x => x is not null)
                .Select(x => x.Value)
                .ToArray();

            var hasCte = keywords.Any(x => x.Keyword == TSQLKeywords.WITH);
            // ReSharper disable once PossibleInvalidOperationException
            var modifyingStatement = statements.FirstOrDefault(x => _modifyingStatementTypes.Contains(x));
            if (modifyingStatement == 0)
            {
                var hasSubqueries = statements.Count(x => x == StatementType.Select) > 1;
                // this is a select, not changing data 
                return new Query
                {
                    Text = query,
                    Type = StatementType.Select,
                    HasCte = hasCte,
                    HasSubqueries = hasSubqueries,
                    HasComplexTypes = false, // ?? 
                    HasSelectionFromViews = false // ??
                };
            }
            else
            {
                var hasSubqueries = statements.Any(x => x == StatementType.Select);
                // this is not a select, query is changing data 
                return new Query
                {
                    Text = query,
                    Type = StatementType.Select,
                    HasCte = hasCte,
                    HasSubqueries = hasSubqueries,
                    HasComplexTypes = false, // ?? 
                    HasSelectionFromViews = false // ??
                };
            }
        }

        private static StatementType? Map(TSQLKeywords keywords)
        {
            return keywords.ToString() switch
            {
                "SELECT" => StatementType.Select,
                "INSERT" => StatementType.Insert,
                "UPDATE" => StatementType.Update,
                "DELETE" => StatementType.Delete,
                _ => null
            };
        }
    }
}