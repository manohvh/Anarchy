using System.Net.Http;
using Newtonsoft.Json;
using Discord.Gateway;
using Leaf.xNet;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Discord
{
    public static class JsonExtensions
    {
        internal static T Deserialize<T>(this string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }


        public static T Deserialize<T>(this HttpResponse response)
        {
            return response.ToString().Deserialize<T>();
        }


        public static T DeserializeEx<T>(this HttpResponse response) where T : ControllableEx
        {
            JObject json = JObject.Parse(response.ToString());
            return ((T)json.ToObject(typeof(T))).SetJson(json);
        }


        public static List<T> DeserializeExArray<T>(this HttpResponse response) where T : ControllableEx, new()
        {
            return JArray.Parse(response.ToString()).PopulateListJson<T>();
        }


        public static List<T> PopulateListJson<T>(this JArray jArray) where T : ControllableEx
        {
            List<T> results = new List<T>();

            foreach (var channel in jArray.Children<JObject>())
            {
                T obj = channel.ToObject<T>();
                obj.Json = channel;

                results.Add(obj);
            }

            return results;
        }


        internal static T Deserialize<T>(this GatewayResponse response)
        {
            return response.Data.ToString().Deserialize<T>();
        }
    }
}