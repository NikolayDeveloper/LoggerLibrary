using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using WTO.Service.Logger.Utils.StringTemplate;

namespace WTO.Service.Logger.Utils.StringUtils
{
	/// <summary>
	/// Утилиты для работы с текстом
	/// </summary>
	public static partial class uText
	{
		/// <summary>
		/// Проверим строку регулярным выражением
		/// </summary>
		/// <param name="original">Строка, над которой будет производиться проверка</param>
		/// <param name="regExp">Регулярное выражение, которому должна соответствовать строка</param>
		/// <returns>true, если строка соответствует регулярному выражению</returns>
		public static bool StrEqual(object original, Regex regExp)
		{
			string originalValue = IfNull(original);

			Match match = regExp.Match(originalValue);
			if (match.Success)
			{
				// первое вхождение по регулярному выражению должно совпадать со строкой целиком
				return (originalValue == match.Value);
			}
			return false;
		}

		/// <summary>
		/// Проверим строку регулярным выражением
		/// </summary>
		/// <param name="original">Строка, над которой будет производиться проверка</param>
		/// <param name="pattern">Регулярное выражение, которому должна соответствовать строка</param>
		/// <returns>true, если строка соответствует регулярному выражению</returns>
		public static bool StrEqual(object original, string pattern)
		{
			Regex regExp = new Regex(pattern);
			return StrEqual(original, regExp);
		}

		/// <summary>
		/// Проверить, является ли строковое представление переданного объекта
		/// целым числом. Числу может предшествовать знак минуса
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static bool IsNumber(object original)
		{
			return StrEqual(original, @"-?\d+");
		}

		/// <summary>
		/// Проверить, является ли строковое представление переданного объекта
		/// целым положительным числом (то есть содержит одни только цифры, никаких знаком перед числом)
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static bool IsNumberUnsigned(object original)
		{
			return StrEqual(original, @"\d+");
		}

		/// <summary>
		/// Проверить, является ли строковое представление переданного объекта
		/// корректным IP-адресом
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsValidIP(object original)
		{
			if (uText.SafeTrim(original) == "::1")
			{
				return true;
			}

			bool result = StrEqual(original, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
			return result;
		}

		/// <summary>
		/// Обычный пробел
		/// </summary>
		public const string Space = (" ");

		/// <summary>
		/// Неразрывный пробел &amp;nbsp; (он же &amp;#160;)
		/// </summary>
		public const string NonBreakingSpace = ("\u00A0");

		///// <summary>
		///// Класс: абзац со вшитым форматированием
		///// </summary>
		//public class FmtParagraph : Paragraph
		//{
		//	//.public.public 

		//	public FmtParagraph AppendLine(string text)
		//	{
		//		return (FmtParagraph)base.AppendLine(text).Font(new FontFamily("Times New Roman")).FontSize(14);
		//	}
		//}

		/// <summary>
		/// Сделать первую букву заглавной, не затрагивая остальных
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static string ToFirstUpper(string original)
		{
			StringBuilder sbResult = new StringBuilder();
			sbResult.Append(original.Substring(0, 1).ToUpper());
			sbResult.Append(original.Substring(1));
			return sbResult.ToString();
		}

		/// <summary>
		/// Заменяет некоторые спецсимволы на их эквиваленты,
		/// чтобы строка заняла одну линию
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static string EscapeToOneLine(string original)
		{
			string result = original;

			Dictionary<string, string> replacer = new Dictionary<string, string>()
			{
			//	{"\t", @"\t"},
			//	{"\r", @"\r"},
			//	{"\n", @"\n"},
				{"\r", ""},
				{"\n", ""},
			};
			foreach (string key in replacer.Keys)
			{
				result = result.Replace(key, replacer[key]);
			}
			return result;
		}

		/// <summary>
		/// Число с подписью в нужном падеже
		/// </summary>
		/// <param name="number"></param>
		/// <param name="word"></param>
		/// <returns></returns>
		public static string NumberInCase(int number, WordInCase word)
		{
			string text = string.Empty;

			// Определим последнюю цифру
			int lastDigit = (number % 10);

			switch (lastDigit)
			{
				case 0:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					{
						text = word.Count10;
						break;
					}
				case 1:
					{
						text = word.Count1;
						break;
					}
				case 2:
				case 3:
				case 4:
					{
						text = word.Count2;
						break;
					}
			}
			// Исключение для чисел с 11 до 14
			lastDigit = (number % 100);
			if (10 < lastDigit && lastDigit < 15)
			{
				text = word.Count10;
			}
			return string.Format("{0} {1}", number, text);
		}

		/// <summary>
		/// Число с подписью в нужном падеже
		/// </summary>
		/// <param name="number"></param>
		/// <param name="count1">Подпись для количества 1</param>
		/// <param name="count2">Подпись для количества 2</param>
		/// <param name="count10">Подпись для количества 10</param>
		/// <returns></returns>
		public static string NumberInCase(object value, string count1, string count2, string count10)
		{
			WordInCase word = new WordInCase()
			{
				Count1 = count1,
				Count2 = count2,
				Count10 = count10
			};
			int number = Convert.ToInt32(value);
			return NumberInCase(number, word);
		}

		/// <summary>
		/// Безопасное применение строки на случай, если строка null
		/// </summary>
		/// <param name="original">Исходная строка</param>
		/// <param name="byDefault">Чем заменить исходную строку, если та оказалась null</param>
		/// <returns></returns>
		public static string IfNull(object original, string byDefault = "")
		{
			byDefault = (byDefault ?? string.Empty);
			return (original ?? byDefault).ToString();
		}

		/// <summary>
		/// Безопасное отсечение пробелов на случай, если строка null
		/// </summary>
		/// <param name="original">Исходная строка, в концах которой будут удалены пробелы</param>
		/// <param name="byDefault">Чем заменить исходную строку, если та оказалась null</param>
		/// <returns>Строка с изъятыми по концам пробелами</returns>
		public static string SafeTrim(object original, string byDefault = "")
		{
			return IfNull(original, byDefault).Trim();
		}

		/// <summary>
		/// Безопасное возвращение на случай, если файл пуст
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static byte[] FileTrim(byte[] fileContent)
		{
			return (fileContent ?? new byte[0]);
		}

		/// <summary>
		/// Указывает, имеет ли указанная строка значение null или System.String.Empty
		/// </summary>
		/// <param name="original">Строка для проверки</param>
		/// <returns>true, если параметр value равен null или пустой строке ("");
		/// в противном случае — false</returns>
		public static bool IsNullOrEmpty(object original)
		{
			if (original == null)
			{
				return true;
			}
			return string.IsNullOrEmpty(Convert.ToString(original));
		}

		/// <summary>
		/// Указывает, является ли указанная строка значением null, пустой строкой
		/// или строкой, состоящей только из пробельных символов
		/// </summary>
		/// <param name="original">Строка для проверки</param>
		/// <returns>Значение true, если параметр value имеет значение null или System.String.Empty,
		/// либо если параметр value содержит только пробельные символы</returns>
		public static bool IsNullOrWhiteSpace(object original)
		{
			if (original == null)
			{
				return true;
			}
			return string.IsNullOrWhiteSpace(Convert.ToString(original));
		}

		/// <summary>
		/// Заменим в строке спецсимволы на символы обычной кириллицы и латиницы
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static string NoSpecialSymbols(string original)
		{
			string result = original;
			// Проверим, есть ли спецсимволы в строке
			Regex pattern = new Regex("[ӘәІіҢңҒғҮүҰұҚқӨөҺһ]");
			Match match = pattern.Match(original);
			if (match.Success)
			{
				// Список символов, подлежащих замене
				Dictionary<string, string> replacer = new Dictionary<string, string>()
				{
					{"Ә", "А"},
					{"ә", "а"},
					{"І", "I"},
					{"і", "i"},
					{"Ң", "Н"},
					{"ң", "н"},
					{"Ғ", "Г"},
					{"ғ", "г"},
					{"Ү", "Y"},
					{"ү", "y"},
					{"Ұ", "Y"},
					{"ұ", "y"},
					{"Қ", "К"},
					{"қ", "к"},
					{"Ө", "О"},
					{"ө", "о"},
					{"Һ", "H"},
					{"һ", "h"}
				};
				foreach (string key in replacer.Keys)
				{
					result = result.Replace(key, replacer[key]);
				}
			}
			return result;
		}

		/// <summary>
		/// Удалить символ конца строки
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static string NoBreak(string original)
		{
			return new Regex("@[\r\n]").Replace(original, string.Empty);
		}

		/// <summary>
		/// "Уверенное" извлечение подстроки без возникновения исклчительной ситуации, 
		/// даже если длина отрезка выходит за пределы строки
		/// </summary>
		/// <param name="original"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string SureSubstring(string original, int start, int length)
		{
			int originLen = original.Length;
			// Если start уже выходит за пределы строки
			if (start >= originLen)
			{
				return string.Empty;
			}
			// Если start выходит за нижнюю границу индексов
			if (start < 0)
			{
				start = 0;
			}
			// Если length указан некорректно
			if (length <= 0)
			{
				return string.Empty;
			}
			// Если сумма (start + length) выходит за пределы строки
			if (start + length > originLen)
			{
				return original.Substring(start);
			}
			// Возвращаем обычную подстроку
			return original.Substring(start, length);
		}

		/// <summary>
		/// "Уверенное" извлечение подстроки без возникновения исклчительной ситуации, 
		/// даже если длина отрезка выходит за пределы строки
		/// </summary>
		/// <param name="original"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string SureSubstring(object original, int start, int length)
		{
			string sOriginal = IfNull(original);
			return SureSubstring(sOriginal, start, length);
		}

		/// <summary>
		/// Усечём строку до указанной длины, если нужно
		/// </summary>
		/// <param name="original">Исходная строка</param>
		/// <param name="length">Количество символов, которое не должна превысить длина строки</param>
		/// <param name="postfix">Указатель на то, что строка усечена</param>
		/// <returns></returns>
		public static string TrimByLength(string original, int length, string postfix = "...")
		{
			if (original.Length <= length)
			{
				return original;
			}
			StringBuilder result = new StringBuilder();
			result.Append(original.Substring(0, length - postfix.Length));
			result.Append(postfix);
			return result.ToString();
		}

		/// <summary>
		/// Усечём строку до указанной длины, если нужно
		/// </summary>
		/// <param name="original">Исходная строка</param>
		/// <param name="length">Количество символов, которое не должна превысить длина строки</param>
		/// <param name="postfix">Указатель на то, что строка усечена</param>
		/// <returns></returns>
		public static string TrimByLength(object original, int length, string postfix = "...")
		{
			string sOriginal = IfNull(original);
			return TrimByLength(sOriginal, length, postfix);
		}

		/// <summary>
		/// Заключить строку в двойные проценты — подготовить для перевода
		/// </summary>
		/// <param name="original">Исходная строка</param>
		/// <returns>Текст, обрамлённый двойными процентами</returns>
		public static string Percented(string original)
		{
			if (!original.Contains("%%"))
			{
				original = string.Format("%%{0}%%", original);
			}
			return original;
		}

		/// <summary>
		/// Найти процент совпадения двух строк посимвольно
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static double PercentOfMatch(string first, string second)
		{
			double result = 0;
			// Очистим строки
			first = SafeTrim(first);
			second = SafeTrim(second);
			// Определим, какая строка более длинная
			int minLen = first.Length;
			int maxLen = first.Length;
			if (first.Length > second.Length)
			{
				minLen = second.Length;
			}
			else
			{
				maxLen = second.Length;
			}

			int matchLen = 0;
			while ((matchLen < minLen) && first[matchLen] == second[matchLen])
			{
				matchLen++;
			}
			if (maxLen > 0)
			{
				result = matchLen * 100d / maxLen;
			}
			return result;
		}

		//public static FontFamily Times
		//{
		//	get { return new FontFamily("Times New Roman"); }
		//}

		//public static FontFamily Arial
		//{
		//	get { return new FontFamily("Arial"); }
		//}

		/// <summary>
		/// очистить входящий текст от всех тегов
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static string ClearTags(string original)
		{
			string result = original;
			result = new Regex(@"<.*?>").Replace(original, "");
			result = result.Replace("&nbsp;", " ");
			return result;
		}

		/// <summary>
		/// Кодировать URL-адрес для браузера IE
		/// </summary>
		/// <param name="context"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		//public static string UrlPathEncode(HttpContext context, string fileName)
		//{
		//	if (context == null)
		//	{
		//		return fileName;
		//	}
		//	return UrlPathEncode(context.Request, fileName);
		//}

		/// <summary>
		/// Кодировать URL-адрес для браузера IE
		/// </summary>
		/// <param name="context"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		//public static string UrlPathEncode(HttpRequest request, string fileName)
		//{
		//	if (request == null)
		//	{
		//		return fileName;
		//	}
		//	if (request.Browser.IsBrowser("InternetExplorer"))
		//	{
		//		fileName = HttpUtility.UrlPathEncode(fileName);
		//	}
		//	return fileName;
		//}

		/// <summary>
		/// Записать число в 16-ричном формате (по умолчанию 2 символа на байт)
		/// </summary>
		/// <typeparam name="T">int или byte</typeparam>
		/// <param name="number">Исходное число</param>
		/// <param name="size">Сколько выводить 16-ричных цифр</param>
		/// <returns></returns>
		public static string Hex<T>(T number, int size = 2) where T : IFormattable
		{
			return number.ToString("X" + size, null);
		}

		/// <summary>
		/// Записать 16-ричные значения массива целых в виде строки
		/// </summary>
		/// <typeparam name="T">int или byte</typeparam>
		/// <param name="encoded">Массив целых</param>
		/// <param name="delimitWithDash">Разделять ли числа дефисами</param>
		/// <returns></returns>
		public static string GetHexString<T>(IEnumerable<T> encoded, bool delimitWithDash = false) where T : IFormattable
		{
			// Запишем каждое значение в виде 16-ричного числа
			var hex = encoded.Select(code => Hex(code));
			string delimiter = (delimitWithDash ? "-" : "");
			string result = string.Join(delimiter, hex);
			return result;
		}

		/// <summary>
		/// Извлекает расширение для файла из значения MIME
		/// </summary>
		/// <param name="mimeType">Cтроки, представляющей собой значение типа MIME</param>
		/// <returns></returns>
		public static string GetExtensionFromMime(string mimeTypeValue)
		{
			string result = string.Empty;
			string mimeType = SafeTrim(mimeTypeValue).ToLower();

			if (!string.IsNullOrEmpty(mimeType))
			{
				Match match = new Regex(@".*/(.*?)$").Match(mimeType);
				result = (match.Success) ? (match.Groups[1].Value) : (string.Empty);

				// Произведем некоторые замены
				Dictionary<string, string> replace = new Dictionary<string, string>()
				{
					{"plain", "txt"},
					{"jpeg", "jpg"},
				};
				foreach (string key in replace.Keys)
				{
					result = result.Replace(key, replace[key]);
				}
			}
			result = result.Trim();
			if (!string.IsNullOrEmpty(result))
			{
				result = ("." + result);
			}
			return result;
		}

		/// <summary>
		/// Возвращает массив данных по имени субъекта из сертификата
		/// </summary>
		/// <param name="certificate"></param>
		/// <returns></returns>
		public static DiReplace GetSubject(X509Certificate2 certificate)
		{
			DiReplace result = new DiReplace();
			string[] subjectInfo = certificate.Subject.SimpleSplit(", ", StringSplitOptions.RemoveEmptyEntries);

			foreach (string row in subjectInfo)
			{
				string[] rowInfo = row.SimpleSplit("=");
				result[rowInfo[0]] = rowInfo[1];
			}

			foreach (X509Extension ext in certificate.Extensions)
			{
				// Пытаемся отыскать идентификатор ключа субъекта
				if (ext is X509SubjectKeyIdentifierExtension)
				{
					var subjectKey = (ext as X509SubjectKeyIdentifierExtension);
					result["SKI"] = subjectKey.SubjectKeyIdentifier.ToLowerInvariant();
					break;
				}
			}
			return result;
		}
	}
}
