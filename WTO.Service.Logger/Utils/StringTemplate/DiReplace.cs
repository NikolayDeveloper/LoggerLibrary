using System.Collections.Generic;
using WTO.Service.Logger.Utils.ListUtils;

namespace WTO.Service.Logger.Utils.StringTemplate
{
	/// <summary>
	/// Класс: список элементов для подстановки в шаблон
	/// </summary>
	public class DiReplace : Dictionary<string, object>
	{
		/// <summary>
		/// Массово добавим ещё значения в словарь
		/// </summary>
		/// <param name="otherDic"></param>
		public void AddRange(DiReplace otherDic)
		{
			foreach (string key in otherDic.Keys)
			{
				this[key] = otherDic[key];
			}
		}

		/// <summary>
		/// Вывод элементов в строку с указанным разделителем
		/// </summary>
		/// <param name="glue">Разделитель для соединения элементов</param>
		/// <returns></returns>
		public string ToString(string glue = ", ")
		{
			if (this.IsEmpty())
			{
				return string.Empty;
			}

			ListJoin keyValuePairs = new ListJoin();

			foreach (string key in this.Keys)
			{
				keyValuePairs.AddFormat("{0}={1}", key, this[key]);
			}
			return keyValuePairs.ToString(glue);
		}
	}
}
