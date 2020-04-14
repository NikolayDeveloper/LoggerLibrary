using WTO.Service.Logger.Utils.StringTemplate;

namespace WTO.Service.Logger.Utils.ListUtils
{
	public class ListJoin : uList<string>
	{
		/// <summary>
		/// Добавляет строку в конец коллекции.
		/// Применяется форматирование по принципу string.Format
		/// </summary>
		/// <param name="item">Шаблон строки, добавляемой в конец коллекции</param>
		/// <param name="args">Элементы для подстановки в шаблон</param>
		public void AddFormat(string item, params object[] args)
		{
			this.Add(string.Format(item, args));
			
		}

		/// <summary>
		/// Добавляет строку в конец коллекции.
		/// Применяется форматирование по принципу StringTemplater
		/// </summary>
		/// <param name="item">Шаблон строки, добавляемой в конец коллекции</param>
		/// <param name="args">Элементы для подстановки в шаблон</param>
		public void AddTemplate(string item, DiReplace args)
		{

			this.Add(StringTemplater.Compile(item, args));
		}

		/// <summary>
		/// Вывод элементов в строку с указанным разделителем
		/// </summary>
		/// <param name="glue">Разделитель для соединения элементов</param>
		/// <returns></returns>
		public string ToString(string glue = ", ")
		{
			return string.Join(glue, this);
		}
	}
}
