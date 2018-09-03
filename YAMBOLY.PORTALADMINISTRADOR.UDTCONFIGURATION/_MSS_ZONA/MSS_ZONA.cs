namespace YAMBOLY.PORTALADMINISTRADOR.UDTCONFIGURATION._MSS_ZONA
{
    [DBStructure]
    [SAPTable("MSS_ZONA", TableType = SAPbobsCOM.BoUTBTableType.bott_MasterData)]
    public class MSS_ZONA
    {
        [SAPField(FieldDescription = "Array de coordenadas de polígono gmaps", FieldType =SAPbobsCOM.BoFieldTypes.db_Memo)]
        public string COORDINATESARRAY { get; set; }
    }
}