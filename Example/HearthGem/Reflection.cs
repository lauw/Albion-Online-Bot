using System.Reflection;
using UnityEngine;

namespace HearthGem
{
	public class Reflection
	{
		public static object GetField(object obj,string field)
		{
			return obj.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
		}
		public static object GetStaticField<T>(string field)
		{
			return typeof(T).GetField(field, BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
		}
		public static void SetField(object obj,string field,object value)
		{
			obj.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(obj,value);
		}
		public static void SetStaticField<T>(string field,object value)
		{
			typeof(T).GetField(field, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null,value);
		}
		public static object InvokeMethod(object obj,string method,params object[] parameters)
		{
			return obj.GetType().GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(obj, parameters);
		}
		public static object InvokeStaticMethod<T>(string method,params object[] parameters)
		{
			return typeof(T).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, parameters);
		}
	}
}

