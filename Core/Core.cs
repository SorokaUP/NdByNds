using System;
using System.IO;
using ExcelDataReader;
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
            Console.WriteLine($"Определяем версию модели");
            ModelMaster model = versionSbis.GetModel(bookType, correctNum);

            if (model != null)
            {
                Console.WriteLine($"Версия модели: {model.versionName}");
                Console.WriteLine($"Режим работы: {modeType}");

                switch (modeType)
                {
                    case ModeType.ExcelToXml:                        
                        ExcelToXml(bookType, model, importFilePaths, pathSaveFile);
                        break;

                    case ModeType.CheckSum:
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

        /// <summary>
        /// По версии структуры СБИС определяем обработчик (класс Model/Version_X_XX.cs)
        /// </summary>
        /// <param name="versionSbis">Версия структуры СБИС по которой идет обработка</param>
        /// <param name="bookType">Тип книги/журнала</param>
        /// <param name="roundType">Тип округления</param>
        /// <param name="correctNum">Номер корректировки (по умолчанию 0)</param>
        /// <returns>Модель данных</returns>
        private static ModelMaster GetModel(this VersionSbis versionSbis, BookType bookType, byte correctNum)
        {
            switch (versionSbis)
            {
                case VersionSbis.v5_08:
                    return new Model_5_08(bookType, correctNum);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Конвертор фалов Excel в XML по версии структуры СБИС
        /// </summary>
        /// <param name="modeType">Режим работы</param>
        /// <param name="model">Модель данных</param>
        /// <param name="importFilePaths">Пути к файлам Excel</param>
        /// <param name="pathSaveFile">Путь для сохранения результатов</param>
        private static void ExcelToXml(BookType bookType, ModelMaster model, string[] filePaths, string pathSaveFile)
        {
            if (!model.CheckNumberLineValues())
            {
                Helper.Log($"Версия модели {model.versionName} содержит не верные номера начала считывания строк.");
                return;
            }

            string filePath = $@"{pathSaveFile}\{model.fileName}.xml";
            StreamWriter xml = new StreamWriter(filePath);//, false, Encoding.GetEncoding("Windows-1251"));

            Helper.Log($"Создан файл: {filePath}");
            try
            {
                Helper.Log($"Запись шапки");
                xml.WriteLine(model.GetHeader());
                Helper.Log($"Начало считывания строк данных...");

                DateTime startJob = DateTime.Now;
                ExcelToXml_Process(filePaths, model.GetBodyBook(), model.LineStartReadExcel, xml);
                TimeSpan TotalTime = DateTime.Now.Subtract(startJob);
                Helper.Log($"Итоговое время: {TotalTime.TimeFormat()}", LogMode.Успех);

                xml.WriteLine(model.GetFooter());
            }
            catch (Exception e)
            {
                Helper.Log($"Ошибка на уровне обработки: {e.Message}", LogMode.Ошибка);
            }
            finally
            {
                Helper.Log($"Сохранение файла...");
                xml.Close();
                Helper.Log($"Выполнено");

                xml.Dispose();
                GC.Collect();
                Console.WriteLine($"Сборка мусора окончена");
            }
        }

        /// <summary>
        /// Обработка файлов Excel в один XML
        /// </summary>
        /// <param name="filePaths">Пути к файлам Excel</param>
        /// <param name="getBodyBook">Ссылка на Метод обработки книги/журнала</param>
        /// <param name="iLineBegin">Строка начала считывания данных из Excel файла (одинаково для всех выбранных Excel файлов в рамках выбранной книги/журнала)</param>
        /// <param name="xml">Поток записи в файл XML</param>
        private static void ExcelToXml_Process(string[] filePaths, ModelMaster.DGetBodyBook getBodyBook, int iLineBegin, StreamWriter xml)
        {
            //Открываем по очереди каждый выбранный файл Excel
            foreach (string filePath in filePaths)
            {
                try
                {
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        Helper.Log($"Открываем Excel файл {filePath}");

                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            Helper.Log($"Начало считывания. Обнаружено {reader.RowCount} строк данных");

                            //Пропускаем лишние строки, в соответствии с настройками версии и книги
                            int iLine = 0;
                            while (reader.Read())
                            {
                                iLine++;
                                if (iLine < iLineBegin)
                                {
                                    continue;
                                }

                                try
                                {
                                    //Преобразуем строку данных Excel в массив
                                    object[] data = new object[reader.FieldCount + 1];
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        data[i + 1] = reader.GetValue(i);
                                    }

                                    //Выполняем обработку строки
                                    xml.WriteLine(getBodyBook(data));

                                    //Логирование
                                    if (iLine % Helper.LogLines == 0)
                                    {
                                        Helper.Log($"   Считано: {iLine}", LogMode.Сообщение, false);
                                        Helper.callback?.OnProgress(iLine, reader.RowCount);
                                    }
                                }
                                catch (Exception exReader)
                                {
                                    Helper.Log($"Ошибка на уровне чтения строки: {exReader.Message}", LogMode.Ошибка);
                                }
                            }

                            //Сообщаем в UI о продвижении
                            Helper.callback?.OnProgress(reader.RowCount, reader.RowCount);
                        }
                    }
                }
                catch (Exception exFile)
                {
                    Helper.Log($"Ошибка на уровне чтения файла: {exFile.Message}", LogMode.Ошибка);
                }
            }
        }
    }
}
