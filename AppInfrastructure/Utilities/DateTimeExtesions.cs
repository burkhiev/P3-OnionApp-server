using NodaTime;

namespace AppInfrastructure.Utilities
{
    public static class DateTimeExtesions
    {
        /// <summary>
        /// <para>
        /// Создает структуру <see cref="Instant"/> из <see cref="DateTime"/>.
        /// </para>
        /// <para>
        /// Обратите внимание, что данный метод для создания <see cref="Instant"/> структуры создает
        /// новую структуру <see cref="DateTime"/> с Kind = <see cref="DateTimeKind.Utc"/>.
        /// Иначе возникает исключение.
        /// </para>
        /// </summary>
        /// <param name="dateTime">
        /// Структура <see cref="DateTime"/>, 
        /// из которой необходимо вывести <see cref="Instant"/>.
        /// </param>
        /// <returns>Получившаяся структура <see cref="Instant"/>.</returns>
        public static Instant ToInstant(this DateTime dateTime)
        {
            return Instant.FromDateTimeUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
        }
    }
}
