using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
//using static System.Net.WebRequestMethods;

namespace SAM.WEB
{
    public static class RoutesController<T> where T : class
    {
        public static List<T> GetDbSet(string key)
        {
            var writableDoc = JsonNode.Parse(File.ReadAllText("mock.json"));

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var arr = new List<T>();

            JsonNode matchedSet = null;

            writableDoc?.Root.AsObject().TryGetPropertyValue(key, out matchedSet);

            arr = JsonSerializer.Deserialize<List<T>>(matchedSet, options);

            return arr;
        }

        public static bool PostDbSet(T payload, string key)
        {
            try
            {
                var writableDoc = JsonNode.Parse(File.ReadAllText("mock.json"));

                JsonNode matchedSet = null;

                writableDoc?.Root.AsObject().TryGetPropertyValue(key, out matchedSet);

                string content = string.Empty;

                content = JsonSerializer.Serialize(payload);

                var newNode = JsonNode.Parse(content);

                var array = matchedSet.AsArray();

                //newNode.AsObject().Add("Id",  refNbr);

                array.Add(newNode);

                File.WriteAllText("mock.json", writableDoc.ToString());

                return true;

            }
            catch { throw; }
        }

        public static bool UpdateDbSet(T payload, string key, string idName, string id)
        {
            try
            {
                var writableDoc = JsonNode.Parse(File.ReadAllText("mock.json"));

                JsonNode matchedSet = null;

                writableDoc?.Root.AsObject().TryGetPropertyValue(key, out matchedSet);

                var matchedItem = matchedSet.AsArray()
                 .Select((value, index) => new { value, index })
                 .SingleOrDefault(row => row.value
                  .AsObject()
                  .Any(o => o.Value.ToString() == id)
                );

                if (matchedItem != null)
                {
                    matchedSet.AsArray().RemoveAt(matchedItem.index);
                    File.WriteAllText("mock.json", writableDoc.ToString());
                }

                string content = string.Empty;

                content = JsonSerializer.Serialize(payload);

                var newNode = JsonNode.Parse(content);

                var array = matchedSet.AsArray();

                array.Add(newNode);

                File.WriteAllText("mock.json", writableDoc.ToString());

                return true;

            }
            catch { throw; }
        }

        public static bool UpdateDocDbSet(T payload, string key, string idName, string id, string fileId)
        {
            try
            {
                var writableDoc = JsonNode.Parse(File.ReadAllText("mock.json"));

                JsonNode matchedSet = null;

                writableDoc?.Root.AsObject().TryGetPropertyValue(key, out matchedSet);

                var matchedItems = matchedSet.AsArray()
                 .Select((value, index) => new { value, index })
                 .Where(row => row.value
                  .AsObject()
                  .Any(o => o.Value.ToString() == id)
                );

                if (matchedItems != null && matchedItems.Count() > 0)
                {
                    var matchedItem = matchedSet.AsArray()
                  .Select((value, index) => new { value, index })
                  .SingleOrDefault(row => row.value
                   .AsObject()
                   .Any(o => o.Value.ToString() == fileId)
                 );

                    if (matchedItem != null)
                    {
                        matchedSet.AsArray().RemoveAt(matchedItem.index);
                        File.WriteAllText("mock.json", writableDoc.ToString());
                    }
                }

                string content = string.Empty;

                content = JsonSerializer.Serialize(payload);

                var newNode = JsonNode.Parse(content);

                var array = matchedSet.AsArray();

                array.Add(newNode);

                File.WriteAllText("mock.json", writableDoc.ToString());

                return true;

            }
            catch { throw; }
        }

        public static bool DeleteDbDocSet(string idName, string id, string fileId, string key)
        {
            try
            {
                var writableDoc = JsonNode.Parse(File.ReadAllText("mock.json"));

                JsonNode matchedSet = null;

                writableDoc?.Root.AsObject().TryGetPropertyValue(key, out matchedSet);

                var matchedItems = matchedSet.AsArray()
                 .Select((value, index) => new { value, index })
                 .Where(row => row.value
                  .AsObject()
                  .Any(o => o.Value.ToString() == id)
                );

                if (matchedItems != null && matchedItems.Count() > 0)
                {
                    var matchedItem = matchedSet.AsArray()
                  .Select((value, index) => new { value, index })
                  .SingleOrDefault(row => row.value
                  .AsObject()
                   .Any(o => o.Value.ToString() == fileId)
                 );

                    if (matchedItem != null)
                    {
                        matchedSet.AsArray().RemoveAt(matchedItem.index);
                        File.WriteAllText("mock.json", writableDoc.ToString());
                    }
                }

                return true;
            }
            catch { throw; }
        }

        public static bool DeleteDbSet(string idName, string id, string key)
        {
            try
            {
                var writableDoc = JsonNode.Parse(File.ReadAllText("mock.json"));

                JsonNode matchedSet = null;

                writableDoc?.Root.AsObject().TryGetPropertyValue(key, out matchedSet);

                var matchedItem = matchedSet.AsArray()
                 .Select((value, index) => new { value, index })
                 .SingleOrDefault(row => row.value
                  .AsObject()
                  .Any(o => o.Value.ToString() == id)
                //.Any(o => o.Key.ToLower() == idName.ToLower() && o.Value.ToString() == id)
                );

                if (matchedItem != null)
                {
                    matchedSet.AsArray().RemoveAt(matchedItem.index);
                    File.WriteAllText("mock.json", writableDoc.ToString());
                }

                return true;
            }
            catch { throw; }
        }

        public static bool DeleteRangeDbSet(string idName, string id, string key)
        {
            try
            {
                var writableDoc = JsonNode.Parse(File.ReadAllText("mock.json"));

                JsonNode matchedSet = null;

                writableDoc?.Root.AsObject().TryGetPropertyValue(key, out matchedSet);

                var matchedItem = matchedSet.AsArray()
                 .Select((value, index) => new { value, index })
                 .Where(row => row.value
                  .AsObject()
                  .Any(o => o.Value.ToString() == id)
                );

                if (matchedItem != null && matchedItem.Count() > 0)
                {
                    foreach (var item in matchedItem)
                    {
                        matchedSet.AsArray().RemoveAt(item.index);
                    }

                    File.WriteAllText("mock.json", writableDoc.ToString());
                }

                return true;
            }
            catch { throw; }
        }
    }
}