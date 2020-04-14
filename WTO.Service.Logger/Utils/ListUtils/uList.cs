using System.Collections.Generic;

namespace WTO.Service.Logger.Utils.ListUtils
{
	public class uList<T> : List<T>
	{
		/// <summary>
		/// Определить, назначено ли значение List&lt;T&gt; и содержит ли оно в себе данные
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(uList<T> list)
		{
			if (list == null)
			{
				return true;
			}
			return list.IsEmpty();
		}

		/// <summary>
		/// Неявное проеобразование типа к массиву
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static implicit operator T[](uList<T> source)
		{
			return source.ToArray();
		}
	}
}
