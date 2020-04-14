using System;
using System.Collections.Generic;
using System.Text;

namespace WTO.Service.Logger.Enums
{
	//Уровень Логирования
	public enum LoggingLvlType
	{
		/// <summary>
		/// 0 — Без логов
		/// </summary>
		None = 0,

		/// <summary>
		/// 1 — Стандартное логирование
		/// </summary>
		Standart = 1,

		/// <summary>
		/// 2 — Расширенное логирование
		/// </summary>
		Extended = 2,

		/// <summary>
		/// 3 — Полное логирование
		/// </summary>
		Full = 3,

		/// <summary>
		/// 4 — Максимальное логирование
		/// </summary>
		Debug = 4,
	}
	
	/// <summary>
	/// Тип сообщений для логирования (Распределение по файлам)
	/// </summary>
	public enum LogerType
	{
		/// <summary>
		/// Общий лог приложения
		/// </summary>
		AppLog = 1,

		/// <summary>
		/// Лог для операций с базой данных
		/// </summary>
		DBLog = 2,

		/// <summary>
		/// Лог для отправки почты и SMS
		/// </summary>
		MessageLog = 3,

		/// <summary>
		/// Лог входа в систему
		/// </summary>
		AuthorizationLog = 4,

		/// <summary>
		/// Лог действий пользователя
		/// </summary>
		UserActionLog = 5,

		/// <summary>
		/// Лог для дебага
		/// </summary>
		DebugLog = 10,
	}
}
