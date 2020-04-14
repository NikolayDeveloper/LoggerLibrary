using WTO.Service.Logger.Enums;

namespace WTO.Service.Logger.Abstract
{
	public interface IAppLog: ILogger
	{
		private const  LogerType _logerTypeValue = LogerType.AppLog;

	}
}
