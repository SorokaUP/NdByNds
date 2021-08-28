namespace Core.Model
{
    public interface ICallback
    {
        /// <summary>
        /// Успешное завершение операции
        /// </summary>
        /// <param name="message">Сообщение</param>
        void OnSuccess(string message);
        /// <summary>
        /// Провал завершения операции
        /// </summary>
        /// <param name="message">Сообщение</param>
        void OnFailed(string message);
        /// <summary>
        /// Сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="isRewriteLine">Перезаписать текущую строку</param>
        void OnMessage(string message, bool isRewriteLine = false);
        /// <summary>
        /// Прогресс выполнения
        /// </summary>
        /// <param name="value">Текущее значение</param>
        /// <param name="max">Общее количество</param>
        void OnProgress(int value, int max);
    }
}
