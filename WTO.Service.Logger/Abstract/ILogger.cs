using WTO.Service.Logger.Enums;
using WTO.Service.Logger.Utils.StringTemplate;

namespace WTO.Service.Logger.Abstract
{
	public interface ILogger
	{
		void Write(string msg = "", bool returnDate = true);
		void WriteLog(string msg = "", params object[] args); // TODO: Logger.WriteLog
		void WriteLog(string msg = "", DiReplace args = null); // TODO: Logger.WriteLog
		void WriteLogLvl(LoggingLvlType LogLvl, string msg = "", params object[] args);
		void WriteLogLvl(LoggingLvlType LogLvl, string msg = "", DiReplace args = null);

	}
}
