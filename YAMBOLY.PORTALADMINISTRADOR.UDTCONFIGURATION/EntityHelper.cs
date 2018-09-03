using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace YAMBOLY.PORTALADMINISTRADOR.UDTCONFIGURATION
{
    public class EntityModel { }

    public class SAPTableEntity
    {
        public String TableName { get; set; } = String.Empty;
        public String TableDescription { get; set; } = String.Empty;
        public BoUTBTableType TableType { get; set; } = BoUTBTableType.bott_NoObject;
        public List<SAPFieldEntity> UserFieldList { get; set; } = new List<SAPFieldEntity>();
    }

    public class SAPFieldEntity
    {
        public String TableName { get; set; } = String.Empty;
        public String FieldName { get; set; } = String.Empty;
        public String FieldDescription { get; set; } = String.Empty;
        public BoFieldTypes FieldType { get; set; } = BoFieldTypes.db_Alpha;
        public BoFldSubTypes FieldSubType { get; set; } = BoFldSubTypes.st_None;
        public Int32 FieldSize { get; set; } = 200;
        public BoYesNoEnum IsRequired { get; set; } = BoYesNoEnum.tNO;
        public String[] ValidValues { get; set; } = new String[] { };
        public String[] ValidDescription { get; set; } = new String[] { };
        public String DefaultValue { get; set; } = String.Empty;
        public String VinculatedTable { get; set; } = String.Empty;
        public Boolean IsSearchField { get; set; } = false;
    }

    public class SAPUDOEntity
    {
        public SAPUDOEntity() { }

        public String Name { get; set; } = String.Empty;
        public String Code { get; set; } = String.Empty;
        public String HeaderTableName { get; set; } = String.Empty;
        public String[] FindColumns { get; set; }

        public BoUDOObjType ObjectType { get; set; } = BoUDOObjType.boud_MasterData;
        public BoYesNoEnum CanCancel { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum CanClose { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum CanCreateDefaultForm { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum CanFind { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum CanLog { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum ManageSeries { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum EnableEnhancedForm { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum RebuildEnhancedForm { get; set; } = BoYesNoEnum.tNO;
        public BoYesNoEnum CanDelete { get; set; } = BoYesNoEnum.tNO;

        public String[] FormColumnsName { get; set; } = new String[] { };
        public String[] FormColumnsDescription { get; set; } = new String[] { };
        public String[] ChildFormColumns { get; set; } = new String[] { };

        public String[] ChildTableNameList { get; set; }
    }

    [XmlRoot("object")]
    public class CompanyXMLModel
    {
        public Boolean? XMLAsString { get; set; } = null;
        public String Server { get; set; } = String.Empty;
        public String LicenseServer { get; set; } = String.Empty;
        public String DbServerType { get; set; } = String.Empty;
        public String DbUserName { get; set; } = String.Empty;
        public String DbPassword { get; set; } = String.Empty;
        public String CompanyDB { get; set; } = String.Empty;
        public String UserName { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
        public String language { get; set; } = String.Empty;
        public Boolean? UseTrusted { get; set; } = null;

        public CompanyXMLModel() { }
    }

    public class CompanyViewModel
    {
        public Boolean? XMLAsString { get; set; }
        public String Server { get; set; }
        public String LicenseServer { get; set; }
        public BoDataServerTypes DbServerType { get; set; }
        public String DbUserName { get; set; }
        public String DbPassword { get; set; }
        public String CompanyDB { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public BoSuppLangs language { get; set; }
        public Boolean UseTrusted { get; set; }
        public Boolean Connected { get; set; }

        public CompanyViewModel()
        {
            XMLAsString = null;
            LicenseServer = String.Empty;
            DbUserName = String.Empty;
            DbPassword = String.Empty;
            CompanyDB = String.Empty;
            UserName = String.Empty;
            Password = String.Empty;
        }
    }
}
