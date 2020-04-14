using WTO.Service.Logger.Enums;

namespace WTO.Service.Logger.Abstract
{
	public interface IDebugLog : ILogger
	{
		private const  LogerType _logerTypeValue = LogerType.DebugLog;
	}
}
