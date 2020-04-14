using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using WTO.Service.Logger.Abstract;
using WTO.Service.Logger.Enums;

namespace WTO.Service.Logger.Implementations
{
	public class DebugLog : Logger, IDebugLog
	{
		/// <summary>
		/// Текущий тип сообщений для логирования (Распределение по файлам)
		/// </summary>
		protected override LogerType LogerTypeValue { get => _logerTypeValue;}
		public LogerType _logerTypeValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		//private LogerType _logerTypeValue = LogerType.DebugLog;


		public DebugLog(IOptions<LoggerSettings> loggerSettings, IHostEnvironment env, IHttpContextAccessor contextAccessor) : base(loggerSettings, env, contextAccessor)
		{
		}


	}
}
