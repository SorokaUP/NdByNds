using System;
using Core.Model;

namespace Core
{
    public static class Core
    {
        /// <summary>
        /// Выполнить обработку
        /// </summary>
        /// <param name="modeType">Режим работы</param>
        /// <param name="bookType">Тип книги/журнала</param>
        /// <param name="importFilePaths">Пути к файлам Excel/XML</param>
        /// <param name="versionSbis">Версия структуры СБИС по которой идет обработка</param>
        /// <param name="roundType">Тип округления</param>
        /// <param name="correctNum">Номер корректировки (по умолчанию 0)</param>
        /// <param name="pathSaveFile">Путь для сохранения результатов</param>
        /// <param name="callback">Процедура обработки сообщений из UI</param>
        public static void Execute(
            ModeType modeType,
            BookType bookType, 
            string[] importFilePaths, 
            VersionSbis versionSbis, 
            byte correctNum,
            string pathSaveFile,
            ICallback callback)
        {
            Helper.callback = callback;
            ModelMaster model = ModelMapper.GetModelByVersionSbis(versionSbis, bookType, correctNum);

            if (model != null)
            {
                Console.WriteLine($"Версия модели: {model.versionName}");
                Console.WriteLine($"Режим работы: {modeType}");

                switch (modeType)
                {
                    case ModeType.ExcelToXml:
                        ModelMaster.ExcelToXml(model, importFilePaths, pathSaveFile);
                        break;

                    case ModeType.Summary:
                        model.Summary(importFilePaths[0]);
                        break;

                    case ModeType.Validate:
                        model.Validate(importFilePaths[0]);
                        break;
                }
            }
            else
            {
                Helper.Log("Не удалось определить Модель работы для выбранной версии СБИС");
            }
        }
    }
}
