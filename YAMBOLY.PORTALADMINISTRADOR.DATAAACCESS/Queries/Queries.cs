using System;
using System.Configuration;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;

namespace YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.Queries
{
    public class Queries
    {
        public static string GetDBName()
        {
            var name = ConfigurationManager.AppSettings[ConstantHelper.HANADBNAME_KEY].ToString();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(ConstantHelper.HANADBNAME_KEY);
            return name;
        }
            

        public static string GetStringContent(EmbebbedFileName embebbedFileName)
        {
            var query = string.Empty;
            var fileString = XMLHelper.GetXMLString(System.Reflection.Assembly.GetExecutingAssembly(), embebbedFileName);
            var values = fileString.Split(new string[] { ConstantHelper.BEGINQUERY }, StringSplitOptions.None);
            if (values.Length > 1)
                query = values[1];

            query = query.Replace(ConstantHelper.QueryParameters.PARAM0, GetDBName());
            return query;
        }

        public static string GetUrlPath()
        {
            var ip = ConfigurationManager.AppSettings[ConstantHelper.XSJSPATH_KEY];
            return ip;
        }
    }
}
