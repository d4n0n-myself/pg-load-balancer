using System.IO;
using Newtonsoft.Json;

namespace LoadBalancer.Tests
{
    public partial class Mapping
    {
        private static T ReadFromFile<T>()
        {
            var readAllText = File.ReadAllText($@"../../../Objects/{typeof(T).Name}.json");
            return JsonConvert.DeserializeObject<T>(readAllText);
        }
    }
}