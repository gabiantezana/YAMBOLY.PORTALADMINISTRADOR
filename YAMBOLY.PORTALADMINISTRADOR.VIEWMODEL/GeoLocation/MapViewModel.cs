using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;

namespace YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.GeoLocation
{
    public class MapViewModel
    {
        public MapViewModel()
        {
            CanalJList = new List<JsonEntityTwoString>();
            DepartamentoJList = new List<JsonEntityTwoString>();
            DistritoJList = new List<JsonEntityTwoString>();
            FrecuenciaVisitaJList = new List<JsonEntityTwoString>();
            GiroJList = new List<JsonEntityTwoString>();
            JefeVentasJList = new List<JsonEntityTwoString>();
            ProvinciaJList = new List<JsonEntityTwoString>();
            RegionJList = new List<JsonEntityTwoString>();
            RutaJList = new List<JsonEntityTwoString>();
            SupervisorJList = new List<JsonEntityTwoString>();
            TipoClienteJList = new List<JsonEntityTwoString>();
            VendedorJList = new List<JsonEntityTwoString>();
            ZonaJList = new List<JsonEntityTwoString>();
            VisibleMarkers = new List<string>();
        }

        [Obsolete]
        public List<Zone> ZoneList { get; set; }
        [Obsolete]
        public List<Route> RouteList { get; set; }
        [Obsolete]
        public List<Address> ClientList { get; set; }

        public List<TreeViewNode> ShapeList { get; set; }

        /// <summary>
        /// Lista de ítems modificados en el mapa. Tipo: List<TreeViewNode>
        /// </summary>
        public dynamic PostedShapeList { get; set; }

        #region Campos de búsqueda de cliente

        public List<JsonEntityTwoString> RegionJList { get; set; }
        public string Region { get; set; }

        public List<JsonEntityTwoString> DepartamentoJList { get; set; }
        [Display(Name = "Depart.")]
        public string Departamento { get; set; }

        public List<JsonEntityTwoString> ProvinciaJList { get; set; }
        public string Provincia { get; set; }

        public List<JsonEntityTwoString> DistritoJList { get; set; }
        public string Distrito { get; set; }

        public List<JsonEntityTwoString> ZonaJList { get; set; }
        public string Zona { get; set; }

        public List<JsonEntityTwoString> RutaJList { get; set; }
        public string Ruta { get; set; }

        public List<JsonEntityTwoString> CanalJList { get; set; }
        public string Canal { get; set; }

        public List<JsonEntityTwoString> GiroJList { get; set; }
        public string Giro { get; set; }

        public string Codigo { get; set; }

        public string RazonSocial { get; set; }

        [Display(Name = "Con activos")]
        public bool ConActivos { get; set; }

        [Display(Name = "Código Activo")]
        public string CodigoActivo { get; set; }

        public List<JsonEntityTwoString> TipoClienteJList { get; set; }

        [Display(Name = "Tipo cliente")]
        public string TipoCliente { get; set; }

        public List<JsonEntityTwoString> VendedorJList { get; set; }
        public string Vendedor { get; set; }

        public List<JsonEntityTwoString> SupervisorJList { get; set; }
        [Display(Name = "Superv.")]
        public string Supervisor { get; set; }

        public List<JsonEntityTwoString> JefeVentasJList { get; set; }
        [Display(Name = "Jefe ventas")]
        public string JefeVentas { get; set; }

        [Display(Name = "Monto mín.")]
        public decimal? VentasMontoMinimo { get; set; }

        [Display(Name = "Monto máx.")]
        public decimal? VentasMontoMaximo { get; set; }

        [Display(Name = "Fecha inicio")]
        public DateTime? VentasFechaInicio { get; set; }

        [Display(Name = "Fecha fin")]
        public DateTime? VentasFechaFin { get; set; }

        [Display(Name = "L")]
        public bool DiaDeVisitaLunes { get; set; }

        [Display(Name = "M")]
        public bool DiaDeVisitaMartes { get; set; }

        [Display(Name = "M")]
        public bool DiaDeVisitaMiercoles { get; set; }

        [Display(Name = "J")]
        public bool DiaDeVisitaJueves { get; set; }

        [Display(Name = "V")]
        public bool DiaDeVisitaViernes { get; set; }

        [Display(Name = "S")]
        public bool DiaDeVisitaSabado { get; set; }

        [Display(Name = "D")]
        public bool DiaDeVisitaDomingo { get; set; }

        public List<JsonEntityTwoString> FrecuenciaVisitaJList { get; set; }
        [Display(Name = "Frecuencia visita")]
        public string FrecuenciaVisita { get; set; }

        #endregion

        #region VisibleMarkers
        public List<string> VisibleMarkers { get; set; }
        #endregion

        /// <summary>
        /// Tipo de formato de el reporte
        /// </summary>
        public ReportType ReportType { get; set; }
    }
}