using System;

namespace YAMBOLY.GESTIONRUTAS.UDTCONFIGURATION._MSS_GEO_SHAPE
{
    [DBStructure]
    [SAPTable("GEO_SHAPE_D", TableType = SAPbobsCOM.BoUTBTableType.bott_MasterData)]
    public class MSS_GEOSHAPE_D
    {
        [SAPField(FieldDescription = "Id tabla padre MSS_GEOSHAPE")]
        public string GEOSHAPEID { get; set; }

        [SAPField(FieldDescription = "lat",
        FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double LAT { get; set; }

        [SAPField(FieldDescription = "lng",
        FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public double LNG { get; set; }

        [SAPField(FieldDescription = "Enabled",
        FieldType = SAPbobsCOM.BoFieldTypes.db_Alpha,
        ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Yes", "No"})]
        public double ENABLED { get; set; }
    }

}
