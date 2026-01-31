using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// JSON辅助类，用于序列化字典等Unity JsonUtility不支持的类型
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// 将字典序列化为JSON字符串
    /// </summary>
    public static string SerializeDictionary(Dictionary<string, object> dict)
    {
        if (dict == null)
            return "{}";
        
        var sb = new StringBuilder();
        sb.Append("{");
        
        bool first = true;
        foreach (var kvp in dict)
        {
            if (!first)
                sb.Append(",");
            
            sb.Append("\"");
            sb.Append(EscapeJsonString(kvp.Key));
            sb.Append("\":");
            
            sb.Append(SerializeValue(kvp.Value));
            
            first = false;
        }
        
        sb.Append("}");
        return sb.ToString();
    }
    
    /// <summary>
    /// 序列化值
    /// </summary>
    private static string SerializeValue(object value)
    {
        if (value == null)
            return "null";
        
        if (value is string str)
        {
            return "\"" + EscapeJsonString(str) + "\"";
        }
        else if (value is bool b)
        {
            return b ? "true" : "false";
        }
        else if (value is int || value is long || value is short || value is byte)
        {
            return value.ToString();
        }
        else if (value is float || value is double || value is decimal)
        {
            return value.ToString().Replace(",", ".");
        }
        else if (value is Vector2 vec2)
        {
            return $"[{vec2.x},{vec2.y}]";
        }
        else if (value is Vector3 vec3)
        {
            return $"[{vec3.x},{vec3.y},{vec3.z}]";
        }
        else if (value is float[] arr)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < arr.Length; i++)
            {
                if (i > 0) sb.Append(",");
                sb.Append(arr[i].ToString().Replace(",", "."));
            }
            sb.Append("]");
            return sb.ToString();
        }
        else if (value is Dictionary<string, object> dict)
        {
            return SerializeDictionary(dict);
        }
        else if (value is List<object> list)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0) sb.Append(",");
                sb.Append(SerializeValue(list[i]));
            }
            sb.Append("]");
            return sb.ToString();
        }
        else
        {
            // 尝试使用Unity的JsonUtility
            try
            {
                return JsonUtility.ToJson(value);
            }
            catch
            {
                return "\"" + EscapeJsonString(value.ToString()) + "\"";
            }
        }
    }
    
    /// <summary>
    /// 转义JSON字符串
    /// </summary>
    private static string EscapeJsonString(string str)
    {
        if (string.IsNullOrEmpty(str))
            return "";
        
        return str.Replace("\\", "\\\\")
                  .Replace("\"", "\\\"")
                  .Replace("\n", "\\n")
                  .Replace("\r", "\\r")
                  .Replace("\t", "\\t");
    }
    
    /// <summary>
    /// 反序列化JSON字符串为字典
    /// </summary>
    public static Dictionary<string, object> DeserializeDictionary(string json)
    {
        var result = new Dictionary<string, object>();
        
        if (string.IsNullOrEmpty(json) || json.Trim() == "{}")
            return result;
        
        try
        {
            // 使用Unity的JsonUtility解析为简单对象
            // 这里简化处理，实际项目中建议使用完整的JSON库
            json = json.Trim();
            if (json.StartsWith("{") && json.EndsWith("}"))
            {
                json = json.Substring(1, json.Length - 2);
                var pairs = json.Split(',');
                
                foreach (var pair in pairs)
                {
                    var colonIndex = pair.IndexOf(':');
                    if (colonIndex > 0)
                    {
                        var key = pair.Substring(0, colonIndex).Trim().Trim('"');
                        var valueStr = pair.Substring(colonIndex + 1).Trim();
                        
                        object value = ParseJsonValue(valueStr);
                        result[key] = value;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[JsonHelper] 反序列化失败: {e.Message}");
        }
        
        return result;
    }
    
    /// <summary>
    /// 解析JSON值
    /// </summary>
    private static object ParseJsonValue(string valueStr)
    {
        valueStr = valueStr.Trim();
        
        if (valueStr == "null")
            return null;
        
        if (valueStr == "true")
            return true;
        
        if (valueStr == "false")
            return false;
        
        if (valueStr.StartsWith("\"") && valueStr.EndsWith("\""))
        {
            return valueStr.Substring(1, valueStr.Length - 2);
        }
        
        if (valueStr.StartsWith("[") && valueStr.EndsWith("]"))
        {
            // 数组
            var content = valueStr.Substring(1, valueStr.Length - 2);
            var items = content.Split(',');
            var list = new List<object>();
            foreach (var item in items)
            {
                list.Add(ParseJsonValue(item.Trim()));
            }
            return list;
        }
        
        // 尝试解析为数字
        if (int.TryParse(valueStr, out int intVal))
            return intVal;
        
        if (float.TryParse(valueStr, System.Globalization.NumberStyles.Float, 
            System.Globalization.CultureInfo.InvariantCulture, out float floatVal))
            return floatVal;
        
        return valueStr;
    }
}
