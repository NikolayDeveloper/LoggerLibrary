using System;
using System.Security.Cryptography;
using System.Text;

namespace WTO.Service.Logger.Utils.StringUtils
{
	public static class StringExtended
	{
		/// <summary>
		/// Определяет, совпадает ли начало данного экземпляра строки с указанной строкой.
		/// Если указано несколько строк, проееряет для каждой из них
		/// </summary>
		/// <param name="original"></param>
		/// <param name="additionalValues"></param>
		/// <returns></returns>
		public static bool StartsWith(this string original, params string[] additionalValues)
		{
			bool result = false;

			foreach (string currentValue in additionalValues)
			{
				result = result || original.StartsWith(currentValue);
			}
			return result;
		}

		/// <summary>
		/// Вычислить MD5-хэш для строки
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static string HashMD5(this string original, Encoding encoding = null)
		{
			// Если кодировка не задана, считаем, что UTF-8
			if (encoding == null)
			{
				encoding = Encoding.UTF8;
			}
			// Вычисляем хэш и упаковываем в строку
			byte[] bytes = encoding.GetBytes(original);
			byte[] hash = new MD5CryptoServiceProvider().ComputeHash(bytes);

			// Форматируем результат как 16-ричные числа
			StringBuilder result = new StringBuilder();
			foreach (byte value in hash)
			{
				result.Append(value.ToString("x2"));
			}
			return result.ToString();
		}

		/// <summary>
		/// Разбивает строки на подстроки по заданной строке-разделителю
		/// </summary>
		public static string[] SimpleSplit(this string self, string separator, StringSplitOptions options = StringSplitOptions.None)
		{
			string[] result = self.Split(new[] { separator }, options);
			return result;
		}
	}
}
