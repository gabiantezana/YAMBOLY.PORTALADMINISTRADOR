using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.PORTALADMINISTRADOR.HELPER
{
    class EntityHelper
    {
    }
    public class JsonEntity
    {
        public Int32 id { get; set; }
        public String text { get; set; }
    }
    public class JsonEntityTwoString
    {
        public String id { get; set; }
        public String text { get; set; }
    }
    public class JsonEntityGroup
    {
        public List<JsonEntity2> children { get; set; }
        public Int32 id { get; set; }
        public String text { get; set; }
        public String search { get; set; }
    }
    public class JsonEntity2
    {
        public Int32 id { get; set; }
        public String text { get; set; }
        public Int32 type { get; set; }
    }
    public class TempDataEntity
    {
        public MessageType TipoMensaje { get; set; }
        public String Mensaje { get; set; }
    }

    public class GeoOptions
    {
        public GeoOptions()
        {
            paths = new List<Path>();
            coords = new Path();
        }
        #region Polygon Options
        /// <summary>
        /// Coordinates of polygon
        /// </summary>
        public List<Path> paths { get; set; }
        #endregion

        /// <summary>
        /// The coordinates of  marker
        /// </summary>
        #region Marker Options
        public Path coords { get; set; }
        #endregion
    }


    public class Zone
    {
        public GeoOptions GeoOptions { get; set; }
        #region Aditional Properties 
        public string Id { get; set; }
        public string Name { get; set; }
        #endregion
    }

    public class Route : Zone
    {
        public string ZoneId { get; set; }
    }

    public class Map
    {
        public List<Zone> ZoneList { get; set; }
        public List<Route> RouteList { get; set; }
        public List<Address> ClientList { get; set; }
    }

    public class Address
    {
        public GeoOptions GeoOptions { get; set; }
        public string RutaId { get; set; }
        public string ZonaId { get; set; }
        public string Region { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Direccion { get; set; }
        public string Canal { get; set; }
        public string Giro { get; set; }
        public bool ConActivos { get; set; }
        public string CodigoActivo { get; set; }
        public string CodigoInterno { get; set; }
        public string CardCode { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string TipoCliente { get; set; }
        public string Vendedor { get; set; }
        public string Supervisor { get; set; }
        public string JefeDeVentas { get; set; }
        public bool DiaDeVisitaLunes { get; set; }
        public bool DiaDeVisitaMartes { get; set; }
        public bool DiaDeVisitaMiercoles { get; set; }
        public bool DiaDeVisitaJueves { get; set; }
        public bool DiaDeVisitaViernes { get; set; }
        public bool DiaDeVisitaSabado { get; set; }
        public bool DiaDeVisitaDomingo { get; set; }
        public string FrecuenciaVisita { get; set; }
    }

    public class Path
    {
        public double? lat { get; set; }
        public double? lng { get; set; }
    }

    public class TreeViewNode
    {
        public TreeViewNode()
        {
            this.nodes = new List<TreeViewNode>();
        }

        #region Required Properties
        public string text { get; set; }
        public List<TreeViewNode> nodes { get; set; }
        #endregion

        #region Aditional Properties
        /// <summary>
        /// ShapeId
        /// </summary>
        public string Id { get; set; }
        public ShapeType ShapeType { get; set; }
        /// <summary>
        /// Google maps GeoOptions
        /// </summary>
        public GeoOptions GeoOptions { get; set; }

        /// <summary>
        /// ParentId of Shape (Zone-Route-Direction)
        /// </summary>
        public string ParentId { get; set; }

        #endregion
    }

    /// <summary>
    /// Para serializar y deserealizar las coordenadas guardadas en la base de datos
    /// </summary>
    public class RootObject
    {
        public RootObject()
        {
            coords = new List<List<double?>>();
        }
        public List<List<double?>> coords { get; set; }
    }


}
