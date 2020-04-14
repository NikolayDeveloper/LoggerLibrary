using WTO.Service.Logger.Enums;

namespace WTO.Service.Logger.Abstract
{
	public interface IDBLog : ILogger
	{
		private const  LogerType _logerTypeValue = LogerType.DBLog;
	}
}
