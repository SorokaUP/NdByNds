namespace Core
{
    /// <summary>
    /// Версии формата СБИС
    /// </summary>
    public enum VersionSbis
    {
        v5_08,
        v5_07
    }

    /// <summary>
    /// Тип книги
    /// </summary>
    public enum BookType
    {
        //Книга покупок
        Book08 = 8, 
        //Книга продаж
        Book09 = 9,
        //Журнал выставленных сф
        Book10 = 10,
        //Журнал полученных сф
        Book11 = 11
    }

    /// <summary>
    /// Тип округления
    /// </summary>
    public enum RoundType
    {
        Matematic,
        Clipping
    }

    /// <summary>
    /// Режим работы
    /// </summary>
    public enum ModeType
    {
        ExcelToXml,
        Summary,
        Validate
    }

    /// <summary>
    /// Признак обязательности
    /// </summary>
    public enum Feature
    {
        О,
        Н
    }

    /// <summary>
    /// Тип вывода сообщения
    /// </summary>
    public enum LogMode
    {
        Сообщение,
        Ошибка,
        Успех
    }
}
