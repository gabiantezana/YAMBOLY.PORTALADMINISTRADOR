using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.PORTALADMINISTRADOR.HELPER
{
    public class WebHelper
    {
        private static readonly HttpClient client = new HttpClient();

        public static dynamic GetJsonResponse(string path, Type deserealizeType = null, string propertyName = null)
        {
            var url = path;
            var webrequest = (HttpWebRequest)System.Net.WebRequest.Create(url);

            using (var response = webrequest.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                var respuesta = reader.ReadToEnd();
                if ((JsonConvert.DeserializeObject(respuesta) as dynamic).ResponseStatus == "Error")
                    throw new Exception("Internal error: " +
                                        (JsonConvert.DeserializeObject(respuesta) as dynamic).Response.message.value);

                var responseProperty = (JsonConvert.DeserializeObject(respuesta) as dynamic).Response;

                if (deserealizeType != null)
                {
                    if (deserealizeType.IsGenericType && deserealizeType.Name == "List`1")
                    {
                        var itemType = deserealizeType.GetGenericArguments().Single();
                        return ParseJsonToList(responseProperty.ToString(), itemType);
                    }
                    return JsonConvert.DeserializeObject(responseProperty["0"].ToString(), deserealizeType);
                }

                if (propertyName != null)
                {
                    try
                    {
                        return responseProperty["0"][propertyName].ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        return responseProperty["0"][propertyName.Replace("U_", "")].ToString();
                    }
                    catch (Exception ex)
                    {
                        return string.Empty;
                    }
                }

                return responseProperty.ToString();
            }
        }

        private static dynamic ParseJsonToList(dynamic jsonString, Type itemType)
        {
            var listType = typeof(List<>).MakeGenericType(new[] { itemType });
            var list = (IList)Activator.CreateInstance(listType);
            var parsedJson = JObject.Parse(jsonString);
            foreach (var jProperty in parsedJson.Properties())
            {
                var item = JsonConvert.DeserializeObject(jProperty.Value.ToString(), itemType);
                list.Add(item);
            }

            return list;
            /*
            string json = "{a: 10, b: 'aaaaaa', c: 1502}";

            var parsedJson = JObject.Parse(json);
            foreach (var jProperty in parsedJson.Properties())
            {
                Console.WriteLine(string.Format("Name: [{0}], Value: [{1}].", jProperty.Name, jProperty.Value));
                list.Add(jProperty.Value);
            }*/
        }

        public static void GetJsonPostResponse(string url, string content)//TODO: RETURN ERROR EN CATCH, SHOW ERROR IN SCREEN
        {
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                var _content = string.Empty;
                foreach(var line in content.Split(';'))
                {
                    _content += Uri.EscapeDataString(line);
                }

                data["queries"] = content;
                var response = wb.UploadValues(url, "POST", data);
                string responseInString = Encoding.UTF8.GetString(response);
            }
        }

    }
}
