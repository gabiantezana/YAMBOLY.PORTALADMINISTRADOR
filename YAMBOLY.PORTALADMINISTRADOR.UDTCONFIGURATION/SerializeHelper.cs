using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace YAMBOLY.PORTALADMINISTRADOR.UDTCONFIGURATION
{
    public static class SerializeHelper
    {
        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, value);
                return stringWriter.ToString();
            }
        }

        public static string GetXMLFromObject(object o)
        {
            //Test
            /*object d = new { Username = "martin", Roles = new[] { "Developer", "Administrator" } };
            XElement xml = d.ToXml();
            return xml.ToString();*/

            //o = o.ToXml();

            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                serializer.Serialize(tw, o, emptyNs);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }

        public static dynamic XMLToObject(string xml, Type objectType, Boolean validateAttr = false)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            Object obj = null;
            if (String.IsNullOrEmpty(xml))
                throw new Exception("XML passed as parameter is null.");
            try
            {
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(objectType);
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlReader != null)
                    xmlReader.Close();
                if (strReader != null)
                    strReader.Close();
            }

            //Validar atributos
            //if (validateAttr)
            //   new ValidationHelper().Validate(obj);
            return obj;

            //Convertir atributos default
            //TODOG:
        }

    }
}
