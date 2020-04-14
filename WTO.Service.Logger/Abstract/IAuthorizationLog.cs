using WTO.Service.Logger.Enums;

namespace WTO.Service.Logger.Abstract
{
	public interface IAuthorizationLog : ILogger
	{
		private const  LogerType _logerTypeValue = LogerType.AuthorizationLog;
	}
}
