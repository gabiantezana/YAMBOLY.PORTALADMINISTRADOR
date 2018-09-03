using System;

namespace YAMBOLY.PORTALADMINISTRADOR.UDTCONFIGURATION._CRD1
{
    [DBStructure]
    [SAPTable(IsSystemTable = true)]
    public class CRD1
    {
        [SAPField(FieldDescription = "Array de coordenadas de marker gmaps", FieldType = SAPbobsCOM.BoFieldTypes.db_Alpha, FieldSize = 200)]
        public string COORDINATESARRAY { get; set; }
    }
}
