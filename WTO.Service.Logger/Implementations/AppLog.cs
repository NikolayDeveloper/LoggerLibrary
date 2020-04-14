using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WTO.Service.Logger.Abstract;
using WTO.Service.Logger.Enums;

namespace WTO.Service.Logger.Implementations
{
	public class AppLog :Logger,IAppLog
	{
		/// <summary>
		/// Текущий тип сообщений для логирования (Распределение по файлам)
		/// </summary>
		protected override LogerType LogerTypeValue { get => _logerTypeValue;}
		private const LogerType _logerTypeValue = LogerType.AppLog;
		public AppLog(IOptions<LoggerSettings> loggerSettings, IHostEnvironment env, IHttpContextAccessor contextAccessor) : base(loggerSettings, env, contextAccessor)
		{
		}
	}
}
