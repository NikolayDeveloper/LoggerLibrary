using System;
using System.Text;
using System.Text.RegularExpressions;
using WTO.Service.Logger.Utils.ListUtils;

namespace WTO.Service.Logger.Utils.StringTemplate

{
	/// <summary>
	/// Класс: строка с возможностью замены по шаблону
	/// </summary>
	public class StringTemplater
	{
		/// <summary>
		/// Проверим словарь на пустоту
		/// </summary>
		/// <param name="dic"></param>
		/// <returns></returns>
		protected static bool IsNullOrEmpty(DiReplace dic)
		{
			if (dic == null)
			{
				return true;
			}
			return (dic.Count == 0);
		}

		/// <summary>
		/// Подставить значения шаблона в строку. Для перевода на новую строку можно использовать 
		/// {NewLine}, {Br} или {RN}, не включая их в словарь replacements
		/// </summary>
		/// <param name="original">Строка с маркерами шаблона</param>
		/// <param name="replacements">Словарь с элементами для подстановки в шаблон</param>
		/// <returns></returns>
		public static string Compile(string original, DiReplace replacements)
		{
			if (IsNullOrEmpty(replacements))
			{
				return original;
			}

			// Пусть всегда присутствует элемент для перевода на новую строку
			if (!replacements.ContainsKey("NewLine"))
			{
				replacements.Add("NewLine", Environment.NewLine);
			}
			if (!replacements.ContainsKey("Br"))
			{
				replacements.Add("Br", Environment.NewLine);
			}
			if (!replacements.ContainsKey("RN"))
			{
				replacements.Add("RN", Environment.NewLine);
			}

			string result = original;
			Match occurence = new Regex(@"{(\w+)}").Match(original);
			// Пройдемся по всем совпадениям в строке
			while (occurence.Success)
			{
				string name = occurence.Groups[1].Value;
				// Если в словаре есть такое значение - подставим его
				if (replacements.ContainsKey(name))
				{
					result = result.Replace(occurence.Value, replacements.GetString(name));
				}
				occurence = occurence.NextMatch();
			}
			return result;
		}

		/// <summary>
		/// Подставить значения шаблона в строку
		/// </summary>
		/// <param name="original">Строка с маркерами шаблона</param>
		/// <param name="replacements">Элементы для подстановки в шаблон</param>
		/// <returns></returns>
		public static string Compile(StringBuilder original, DiReplace replacements)
		{
			return Compile(original.ToString(), replacements);
		}
	}
}
