using SAPADDON.HELPER;
using SAPbobsCOM;
using System;

namespace YAMBOLY.PORTALADMINISTRADOR.UDTCONFIGURATION
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                CreateSchema();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                Console.Read();
            }
        }

        public static void CreateSchema()
        {
            DBSchema dBSchema = new UserModel().GetDBSchema();
            var model = GetCompanyViewModelFromFile();
            Connect(model);
            dBSchema.TableList.ForEach(x => SapMethodsHelper.CreateTable(GetCommpany(), x));
            dBSchema.FieldList.ForEach(x => SapMethodsHelper.CreateField(GetCommpany(), x));
            dBSchema.UDOList.ForEach(x => SapMethodsHelper.CreateUDO(GetCommpany(), x));
        }

        private static Company Company = null;

        public static Company GetCommpany()
        {
            return Company;
        }

        public static void Connect(CompanyViewModel model)
        {
            if (Company == null)
                Company = CreateNewCompanyFromModel(model);
            if (!Company.Connected)
                Connect();
        }

        public static void ConnectNewCompany(CompanyViewModel model)
        {
            Disconnect();
            Company = CreateNewCompanyFromModel(model);
            Connect();
        }

        private static Company CreateNewCompanyFromModel(CompanyViewModel model)
        {
            Company _Company = new Company
            {
                DbServerType = model.DbServerType,
                Server = model.Server,
                UseTrusted = false,
                DbUserName = model.DbUserName,
                DbPassword = model.DbPassword,
                CompanyDB = model.CompanyDB,
                UserName = model.UserName,
                Password = model.Password,
                LicenseServer = model.LicenseServer,
            };
            return _Company;
        }

        public static void Disconnect()
        {
            if (Company != null)
            {
                if (Company.Connected)
                    Company.Disconnect();
                Company = null;
            }
        }

        private static void Connect()
        {
            Int32 resultReturn = Company.Connect();
            if (resultReturn != 0)
            {
                var message = Company.GetLastErrorDescription();
                throw new Exception(message);
            }
        }

        public static void TryConnectCurrentCompany()
        {
            if (Company == null)
                throw new Exception("Any company for connect.");
            if (!Company.Connected)
                Connect();
        }

        private static CompanyViewModel GetCompanyViewModelFromFile()
        {
            String xml = System.IO.File.ReadAllText(XMLParametersPath);
            CompanyViewModel model = GenerateViewModel(SerializeHelper.XMLToObject(xml, typeof(CompanyXMLModel)));
            return model;
        }

        private static string XMLParametersPath
        {
            get
            {
                string pathDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                string pathArch = System.IO.Path.Combine(pathDir, ConstantHelper.ParameterPath);
                var localPath = new Uri(pathArch).LocalPath;
                return localPath;
            }
        }

        public static CompanyViewModel GenerateViewModel(CompanyXMLModel xmlModel)
        {
            CompanyViewModel model = ReflectionHelper.CopyAToB(xmlModel, typeof(CompanyViewModel), true);

            model.DbServerType = GetSAPEnum(typeof(BoDataServerTypes), xmlModel.DbServerType);
            model.language = String.IsNullOrEmpty(xmlModel.language) ? BoSuppLangs.ln_English_Sg : GetSAPEnum(typeof(BoSuppLangs), xmlModel.language);

            return model;
        }

        private static dynamic GetSAPEnum(Type type, String valueToParse)
        {
            var response = Enum.Parse(type, valueToParse);
            return response;
        }
    }
}
