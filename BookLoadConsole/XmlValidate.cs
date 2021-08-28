using System;
using System.Xml;
using System.Xml.Schema;

namespace BookLoadConsole
{
    public class XmlValidate
    {
        #region Свойства
        /// <summary>
        /// Путь к XML файлу
        /// </summary>
        public string PathXml { get; set; }
        /// <summary>
        /// Путь к XSD схеме
        /// </summary>
        public string PathXsd { get; set; }
        #endregion

        //======================================================================================================
        //======================================================================================================
        //======================================================================================================

        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            Console.ForegroundColor = (e.Severity.ToString() == "Error") ? ConsoleColor.Red : ConsoleColor.DarkYellow;
            Console.WriteLine("  {0}: {1}", e.Severity.ToString(), e.Message);
            Console.ResetColor();
        }

        public void Validate()
        {
            //XML DOM для чтения XML схемы
            XmlDocument _XSDDocument = new XmlDocument();
            //XSD DOM для работы с XML схемой
            XmlSchemaSet _XSDSchemaSet = new XmlSchemaSet();

            try
            {   //Чтение XSD из файла
                Console.WriteLine("Загрузка XSD файла");
                _XSDDocument.Load(PathXsd);
                //Формирование XSD DOM
                _XSDSchemaSet.Add(null, new XmlNodeReader(_XSDDocument));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            XmlDocument _XMLDocument = new XmlDocument();

            try
            {   //Чтение XML из файла
                Console.WriteLine("Загрузка XML файла");
                _XMLDocument.Load(PathXml);
                //Валидация XML по загруженной ранее XML схеме
                Console.WriteLine(">>> Начат процесс валидации...");
                _XMLDocument.Schemas.Add(_XSDSchemaSet);
                _XMLDocument.Validate(new ValidationEventHandler(ValidationCallBack));
                Console.WriteLine(">>> Валидация завершена");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}
