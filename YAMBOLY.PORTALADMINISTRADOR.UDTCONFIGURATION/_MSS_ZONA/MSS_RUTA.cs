namespace YAMBOLY.PORTALADMINISTRADOR.UDTCONFIGURATION._MSS_RUTA
{
    [DBStructure]
    [SAPTable("MSS_RUTA", TableType = SAPbobsCOM.BoUTBTableType.bott_MasterData)]
    public class MSS_RUTA
    {
        [SAPField(FieldDescription = "Array de coordenadas de polígono gmaps", FieldType = SAPbobsCOM.BoFieldTypes.db_Memo)]
        public string COORDINATESARRAY { get; set; }
    }

}
