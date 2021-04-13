using System.IO;
using Newtonsoft.Json;

namespace LoadBalancer.Tests
{
    /// <summary>
    /// Base functions from mapping tests.
    /// </summary>
    public partial class Mapping
    {
        /// <summary>
        /// Read standard from respected type file.
        /// </summary>
        private static T ReadFromFile<T>()
        {
            var readAllText = File.ReadAllText($@"../../../Objects/{typeof(T).Name}.json");
            return JsonConvert.DeserializeObject<T>(readAllText);
        }
    }
}