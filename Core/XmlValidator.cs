using System;
using System.Xml;
using System.Xml.Schema;

namespace Core
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
        /// <summary>
        /// Callback
        /// </summary>
        public Model.ICallback Callback { get; set; }
        #endregion

        //======================================================================================================
        //======================================================================================================
        //======================================================================================================

        private void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            Console.ForegroundColor = (e.Severity.ToString().Equals("Error")) ? ConsoleColor.Red : ConsoleColor.DarkYellow;
            Helper.Log($"  {e.Severity}: {e.Message}", LogMode.Ошибка);
            Console.ResetColor();
            errQnt++;
        }

        int errQnt = 0;

        /// <summary>
        /// Выполнить проверку XML по XSD схеме
        /// </summary>
        public bool Validate()
        {
            XmlDocument xsdDoc = new XmlDocument();
            XmlSchemaSet xsdSchema = new XmlSchemaSet();

            try
            {   
                Helper.Log("Загрузка XSD файла");
                xsdDoc.Load(PathXsd);
                //Формирование XSD DOM
                xsdSchema.Add(null, new XmlNodeReader(xsdDoc));
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message, LogMode.Ошибка);
                return false;
            }

            XmlDocument xml = new XmlDocument();

            try
            {
                Helper.Log("Загрузка XML файла");
                xml.Load(PathXml);

                Helper.Log(">>> Начат процесс валидации...");
                xml.Schemas.Add(xsdSchema);
                xml.Validate(new ValidationEventHandler(ValidationCallBack));

                Helper.Log(">>> Валидация завершена");
            }
            catch (Exception ex)
            {
                Helper.Log(ex.Message, LogMode.Ошибка);
                return false;
            }

            return errQnt == 0;
        }
    }
}
