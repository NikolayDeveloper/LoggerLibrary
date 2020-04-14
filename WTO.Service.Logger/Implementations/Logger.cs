using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using WTO.Service.Logger.Abstract;
using WTO.Service.Logger.Enums;
using WTO.Service.Logger.Utils.StringUtils;
using WTO.Service.Logger.Utils.StringTemplate;

namespace WTO.Service.Logger.Implementations
{
	#region FAQ ???



	#endregion






	public abstract class Logger : ILogger
	{

		#region Свойства

		private readonly LoggerSettings _loggerSettings;
		private readonly IHostEnvironment _env;


		/// <summary>
		/// Уровень Логирования
		/// </summary>
		private  LoggingLvlType _loggingLvl
		{
			get
			{
				
				LoggingLvlType result = LoggingLvlType.Standart;
				//if (string.IsNullOrWhiteSpace(_loggerSettings.LoggingLvl))
				//{
				//	return result;
				//}
				//int Value = 0;
				//if (!int.TryParse(_loggerSettings.LoggingLvl, out Value))
				//{
				//	return result;
				//}

				try
				{
					result = (LoggingLvlType)_loggerSettings.LoggingLvl;
				}
				catch (OverflowException Ex)
				{
					string error = Ex.Message;
				}
				catch (NullReferenceException Ex)
				{
					string error = Ex.Message;
				}
				catch (Exception Ex)
				{
					string error = Ex.Message;
				}
				return result;
			}
		}
		/// <summary>
		/// Путь к файлу логов
		/// </summary>
		//private readonly string _fileName;

		/// <summary>
		/// Кодировка
		/// </summary>
		private readonly Encoding _charset = Encoding.UTF8;

		/// <summary>
		/// 
		/// </summary>
		private static readonly object locker = new object();

		/// <summary>
		/// Текущий тип сообщений для логирования (Распределение по файлам)
		/// </summary>
		protected abstract LogerType LogerTypeValue { get;}

		/// <summary>
		/// Имя файла лога
		/// </summary>
		private string _logFileName
		{
			get
			{
				
				return Path.ChangeExtension(LogerTypeValue.ToString("G"), ".log");
			}
		}

		/// <summary>
		/// Возвращает путь к лог файлу
		/// </summary>
		private string _fileName
		{
			get
			{
				string result = _logFileName;
				string patchRoot = _env.ContentRootPath;
				string patchDir = "";
				if (!string.IsNullOrEmpty(_loggerSettings.LoggingDir))
				{
					patchDir = _loggerSettings.LoggingDir;
				}

				try
				{
					result = Path.Combine(patchRoot, patchDir, _logFileName);
				}
				catch (Exception Ex)
				{
				}
			
				return result;
			}
		}

		private readonly IHttpContextAccessor _contextAccessor;
		#endregion Свойства

		#region Конструктор
		public Logger(IOptions<LoggerSettings> loggerSettings, IHostEnvironment env, IHttpContextAccessor contextAccessor)
		{
			this._loggerSettings = loggerSettings.Value;
			this._env = env;
			this._contextAccessor = contextAccessor;
		}
		#endregion Конструктор


		#region Методы
		private string GetClientIP()
		{
			return this._contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
			//if (_contextAccessor.HttpContext.Current != null)
			//{
			//	return GetClientIP(this._contextAccessor.HttpContext.Current);
			//}
			//if (OperationContext.Current != null)
			//{
			//	return GetClientIP(OperationContext.Current);
			//}
			//return DefaultAddress;
		}
		/// <summary>
		/// Запишем строку с сообщением в лог-файл
		/// </summary>
		/// <param name="msg">Сообщение, которое нужно зафиксировать в лог-файле</param>
		/// <param name="returnDate">если true, то проставлять в начале каждой строки дату-время записи</param>
		public void Write(string msg = "", bool returnDate = true)
		{
			// Проверим, что нам действительно передано сообщение
			string returnMsg = uText.IfNull(msg);
			// Проверим, можем ли мы извлечь IP посетителя
			string returnIp = "no IP";
			try
			{
				//var t = this._contextAccessor.HttpContext.Connection.RemoteIpAddress;
				returnIp = GetClientIP();
				
				//var t = Request.HttpContext.Connection.RemoteIpAddress;
				////returnIp = WTOServerUtils.GetClientIP();
			}
			//catch (HttpException ex)
			//{
				
			//}
			catch (NullReferenceException ex)
			{
				returnIp = "no IP";
			}
			catch (Exception ex)
			{
				returnIp = "no IP";
			}

			// Открываем файл лога в режиме дозаписи (append = true)
			lock (locker)
			{
				try
				{

					string dir = Path.GetDirectoryName(_fileName);
					if (!Directory.Exists(dir))
					{
						Directory.CreateDirectory(dir);
					}
					using (StreamWriter sw = new StreamWriter(_fileName, true, _charset))
					{
						// Запоминать IP автоматически
						returnMsg = string.Format("[{0}] - {1}", returnIp, returnMsg);
						// Запоминать дату автоматически
						if (returnDate)
						{
							returnMsg = string.Format("[{0:dd.MM.yyyy HH:mm:ss.fff}] - {1}", DateTime.Now, returnMsg);
						}
						// Запишем сообщение
						sw.WriteLine(returnMsg);
					}
				}
				catch (FormatException ex)
				{
					return;
				}
				catch (IOException ex)
				{
					return;
				}
				catch (NullReferenceException ex)
				{
					return;
				}
				catch (Exception ex)
				{
					return;
				}
			}
		}

		public void WriteClear(string msg = "")
		{
			// Проверим, что нам действительно передано сообщение
			string returnMsg = uText.IfNull(msg);
			// Проверим, можем ли мы извлечь IP посетителя
			// Открываем файл лога в режиме дозаписи (append = true)
			lock (locker)
			{
				try
				{

					string dir = Path.GetDirectoryName(_fileName);
					if (!Directory.Exists(dir))
					{
						Directory.CreateDirectory(dir);
					}
					using (StreamWriter sw = new StreamWriter(_fileName, true, _charset))
					{
						returnMsg = string.Format("[{0:dd.MM.yyyy HH:mm:ss.fff}] - {1}", DateTime.Now, returnMsg);
						// Запишем сообщение
						sw.WriteLine(returnMsg);
					}
				}
				catch (FormatException ex)
				{
					return;
				}
				catch (IOException ex)
				{
					return;
				}
				catch (NullReferenceException ex)
				{
					return;
				}
				catch (Exception ex)
				{
					return;
				}
			}
		}

		public void WriteLog(string msg = "", params object[] args) // TODO: Logger.WriteLog
		{
			this.Write(string.Format(msg, args), true);
		}

		public void WriteLog(string msg = "", DiReplace args = null) // TODO: Logger.WriteLog
		{
			this.Write(StringTemplater.Compile(msg, args), true);
		}

		/// <summary>
		/// Запишем строку с сообщением в лог-файл. Применяется форматирование по принципу string.Format.
		/// Проставляем в начале каждой строки дату-время записи
		/// </summary>
		/// <param name="LogLvl">Уровень логирования</param>
		/// <param name="msg">Сообщение, которое нужно зафиксировать в лог-файле</param>
		/// <param name="args">Аргументы для подстановки в текст сообщения</param>
		public void WriteLogLvl(LoggingLvlType LogLvl, string msg = "", params object[] args)
		{
			if ((int)LogLvl > (int)_loggingLvl)
				return;
			this.WriteLog(msg, args);
		}

		public void WriteLogLvl(LoggingLvlType LogLvl, string msg = "", DiReplace args = null)
		{
			if ((int)LogLvl > (int)_loggingLvl)
				return;
			this.WriteLog(msg, args);
		}

		public void WritePackage(object sender, string method, Exception ex, string prefix = "")
		{
			this.WriteLog($"Class ({sender.GetType().Name}) Method ({method}) {prefix}Exception ({ex.GetType().Name}) Message =({ex.Message}) StackTrace =({ex.StackTrace})");

			if (ex.InnerException != null)
			{
				this.WritePackage(sender, method, ex.InnerException, "Inner");
			}
		}

		public void WritePackage(object sender, Exception ex, string prefix = "")
		{
			this.WriteLog($"Class ({sender.GetType().Name}) {prefix}Exception ({ex.GetType().Name}) Message =({ex.Message}) StackTrace =({ex.StackTrace})");

			if (ex.InnerException != null)
			{
				this.WritePackage(sender, ex.InnerException, "Inner");
			}
		}

		/// <summary>
		/// Возьмем из лога страницу ошибки
		/// </summary>
		/// <returns></returns>
		public string Read()
		{
			string[] lines = null;
			try
			{
				lines = System.IO.File.ReadAllLines(_fileName);
				if (lines.Length == 0)
				{
					return string.Empty;
				}

			}
			catch (StackOverflowException ex)
			{
				return "Ошибка чтения из лог файла: \n " + ex.Message;
			}
			catch (IOException ex)
			{
				return "Ошибка чтения из лог файла: \n " + ex.Message;
			}
			catch (NullReferenceException ex)
			{
				return "Ошибка чтения из лог файла: \n " + ex.Message;
			}
			catch (Exception ex)
			{
				return "Ошибка чтения из лог файла: \n " + ex.Message;
			}

			int numberPage = 0;
			StringBuilder LogText = new StringBuilder();
			int lastPageNeed = 0;

			foreach (string line in lines)
			{
				numberPage++;
				try
				{
					if (string.IsNullOrEmpty(line))
						continue;

					string currentDate = DateTime.Now.ToString("dd.MM.yyyy");
					if (line.StartsWith("[" + currentDate))
					{
						lastPageNeed = numberPage;
					}
				}
				catch (ArgumentOutOfRangeException ex)
				{
					string error = ex.Message;
				}
				catch (FormatException ex)
				{
					string error = ex.Message;
				}
				catch (NullReferenceException ex)
				{
					string error = ex.Message;
				}
				catch (Exception ex)
				{
					string error = ex.Message;
				}
			}

			for (int i = lastPageNeed - 1; i < numberPage; i++)
			{
				LogText.AppendLine(lines[i]);
				lastPageNeed++;
			}
			return LogText.ToString();
		}

		#endregion Методы

	}
}
