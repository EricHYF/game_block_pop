using System;
using UnityEngine;
using Newtonsoft.Json;


public static class JsonParser
{
    public static T ToObject<T>(string value)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        
        return default(T);
    }

    public static string ToJson(object data)
    {
        try
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            return json;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        return "";
    }
}
