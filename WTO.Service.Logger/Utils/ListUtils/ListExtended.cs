using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using WTO.Service.Logger.Utils.StringTemplate;

namespace WTO.Service.Logger.Utils.ListUtils
{
	public static class ListExtended
	{
		/// <summary>
		/// Определить, содержит ли список в себе данные
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static bool IsEmpty(this ICollection list)
		{
			return (list.Count == 0);
		}

		#region GetString

		//public static string GetString(this System.Web.SessionState.HttpSessionState dic, string key)
		//{
		//	object result = dic[key];
		//	return Convert.ToString(result);
		//}

		//public static string GetString(this System.Web.HttpApplicationState dic, string key)
		//{
		//	object result = dic[key];
		//	return Convert.ToString(result);
		//}

		//public static string GetString(this System.Web.UI.StateBag dic, string key)
		//{
		//	object result = dic[key];
		//	return Convert.ToString(result);
		//}

		public static string GetString(this NameValueCollection dic, string key)
		{
			object result = dic[key];
			return Convert.ToString(result);
		}

		public static string GetString(this OrderedDictionary dic, object key)
		{
			object result = dic[key];
			return Convert.ToString(result);
		}

		public static string GetString(this DiReplace dic, string key)
		{
			object result = dic[key];
			return Convert.ToString(result);
		}

		#endregion

		#region GetDouble

		//public static double GetDouble(this System.Web.SessionState.HttpSessionState dic, string key)
		//{
		//	object result = dic[key];
		//	return Convert.ToDouble(result, CultureInfo.InvariantCulture);
		//}

		public static float GetSingle(this NameValueCollection dic, string key)
		{
			object result = dic[key];
			return Convert.ToSingle(result, CultureInfo.InvariantCulture);
		}

		#endregion

		#region GetInt64

		//public static long GetInt64(this System.Web.SessionState.HttpSessionState dic, string key)
		//{
		//	object result = dic[key];
		//	return Convert.ToInt64(result);
		//}

		//public static long GetInt64(this System.Web.UI.StateBag dic, string key)
		//{
		//	object result = dic[key];
		//	return Convert.ToInt64(result);
		//}

		public static long GetInt64(this NameValueCollection dic, string key)
		{
			object result = dic[key];
			return Convert.ToInt64(result);
		}

		public static long GetInt64(this OrderedDictionary dic, object key)
		{
			object result = dic[key];
			return Convert.ToInt64(result);
		}

		#endregion

		#region GetInt32

		//public static int GetInt32(this System.Web.SessionState.HttpSessionState dic, string key)
		//{
		//	object result = dic[key];
		//	return Convert.ToInt32(result);
		//}

		//public static int GetInt32(this System.Web.HttpApplicationState dic, string key)
		//{
		//	object result = dic[key];
		//	return Convert.ToInt32(result);
		//}

		//public static int GetInt32(this System.Web.UI.StateBag dic, string key)
		//{
		//	object result = dic[key];
		//	return Convert.ToInt32(result);
		//}

		public static int GetInt32(this OrderedDictionary dic, object key)
		{
			object result = dic[key];
			return Convert.ToInt32(result);
		}

		public static int GetInt32(this NameValueCollection dic, string key)
		{
			object result = dic[key];
			return Convert.ToInt32(result);
		}

		#endregion

		#region GetBoolean

		public static bool GetBoolean(this OrderedDictionary dic, string key)
		{
			object result = dic[key];
			return Convert.ToBoolean(result);
		}

		public static bool GetBoolean(this NameValueCollection dic, string key)
		{
			object result = dic[key];
			return Convert.ToBoolean(result);
		}

		#endregion

		#region GetDateTime

		public static DateTime GetDateTime(this NameValueCollection dic, string key)
		{
			object result = dic[key];
			return Convert.ToDateTime(result);
		}

		#endregion
	}
}
