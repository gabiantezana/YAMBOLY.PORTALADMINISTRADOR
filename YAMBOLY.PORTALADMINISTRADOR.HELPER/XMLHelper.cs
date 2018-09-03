﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace YAMBOLY.PORTALADMINISTRADOR.HELPER
{
    public class XMLHelper
    {
        public static string GetXMLString(EmbebbedFileName xmlFile)
        {
            var resourceFullName = Assembly.GetCallingAssembly().GetManifestResourceNames().ToList().FirstOrDefault(x => x.Contains(xmlFile.ToString()));
            if (string.IsNullOrEmpty(resourceFullName))
                throw new Exception("ResourceName not found: " + xmlFile.ToString());

            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceFullName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static string GetXMLString(Assembly assembly, EmbebbedFileName xmlFile)
        {
            var resourceFullName = assembly.GetManifestResourceNames().ToList().FirstOrDefault(x => x.Contains(xmlFile.ToString()));
            if (string.IsNullOrEmpty(resourceFullName))
                throw new Exception("ResourceName not found: " + xmlFile.ToString());

            using (Stream stream = assembly.GetManifestResourceStream(resourceFullName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

    }
}
