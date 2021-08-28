namespace Core.Model
{
    public static class ModelMapper
    {
        /// <summary>
        /// По версии структуры СБИС определяем обработчик (класс Model/Version_X_XX.cs)
        /// </summary>
        /// <param name="versionSbis">Версия структуры СБИС по которой идет обработка</param>
        /// <param name="bookType">Тип книги/журнала</param>
        /// <param name="correctNum">Номер корректировки (по умолчанию 0)</param>
        /// <returns>Модель данных</returns>
        public static ModelMaster GetModelByVersionSbis(VersionSbis versionSbis, BookType bookType, byte correctNum)
        {
            switch (versionSbis)
            {
                case VersionSbis.v5_08:
                    return new Model_5_08(bookType, correctNum);

                default:
                    return null;
            }
        }
    }
}
