using System;

namespace YAMBOLY.GESTIONRUTAS.UDTCONFIGURATION._MSS_GEO_SHAPE
{
    [DBStructure]
    [SAPTable("MSS_GEOSHAPE", TableType = SAPbobsCOM.BoUTBTableType.bott_MasterData)]
    public class MSS_GEOSHAPE
    {
        [SAPField(FieldDescription = "Tipo de forma",
        ValidValues = new[] { "Z", "R", "C" }, ValidDescription = new[] {    "Zona", "Ruta", "Cliente" })]
        public string SHAPETYPE { get; set; }

        [SAPField(FieldDescription = "Foreign key de la tabla ruta, zona o cliente")]
        public string ID { get; set; }

        /// <summary>
        /// Id del path de coordenadas relacionado a la forma marker
        /// </summary>
        [SAPField(FieldDescription = "idPath")]
        public string PATHID { get; set; }

        /*
        [SAPField(FieldDescription = "clickable",
        ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string clickable { get; set; }

        [SAPField(FieldDescription = "draggable",
        ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string draggable { get; set; }

        [SAPField(FieldDescription = "editable",
        ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string editable { get; set; }

        [SAPField(FieldDescription = "fillColor")]
        public string fillColor { get; set; }

        [SAPField(FieldDescription = "fillOpacity")]
        public string fillOpacity { get; set; }

        [SAPField(FieldDescription = "strokeColor",
        FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string strokeColor { get; set; }

        [SAPField(FieldDescription = "strokeOpacity",
        FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string strokeOpacity { get; set; }

        [SAPField(FieldDescription = "strokeWeight",
        FieldType = SAPbobsCOM.BoFieldTypes.db_Float, FieldSubType = SAPbobsCOM.BoFldSubTypes.st_Measurement)]
        public string strokeWeight { get; set; }

        [SAPField(FieldDescription = "visible",
        ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string visible { get; set; }

        [SAPField(FieldDescription = "ZIndex",
        ValidValues = new[] { "Y", "N" }, ValidDescription = new[] { "Si", "No" }, DefaultValue = "N")]
        public string ZIndex { get; set; }*/

    }

}
