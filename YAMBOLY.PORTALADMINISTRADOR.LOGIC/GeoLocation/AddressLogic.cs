using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation;
using YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.Queries;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;
using static YAMBOLY.PORTALADMINISTRADOR.HELPER.ConstantHelper;

namespace YAMBOLY.PORTALADMINISTRADOR.LOGIC.GeoLocation
{
    public class AddressLogic
    {
        public List<Address> GetList(DataContext dataContext)
        {
            return new DireccionesDataAccess().GetList(dataContext).Select(x => new Address()
            {
                CodigoInterno = GetCodeForAddress(x),
                CardCode = x.CardCode,
                RazonSocial = x.CardName.FirstCharToUpper(),
                Region = x.U_MSS_REGI,
                Departamento = x.State,
                Provincia = x.County,
                Distrito = x.City,
                Direccion = x.Street,
                ZonaId = x.U_MSS_ZONA,
                Canal = x.U_MSS_CANA,
                Giro = x.U_MSS_GIRO,
                ConActivos = x.U_MSSM_POA.ToSafeString() == "Y" ? true : false,
                CodigoActivo = string.Empty,//TODO:,
                TipoCliente = string.Empty,//TODO:,
                Vendedor = x.U_MSS_COVE,
                Supervisor = x.U_MSS_SUPE,
                JefeDeVentas = x.U_MSS_JEVE,
                DiaDeVisitaLunes = x.U_MSS_DVLU.ToSafeString() == "1" ? true : false,
                DiaDeVisitaMartes = x.U_MSS_DVMA.ToSafeString() == "1" ? true : false,
                DiaDeVisitaMiercoles = x.U_MSS_DVMI.ToSafeString() == "1" ? true : false,
                DiaDeVisitaJueves = x.U_MSS_DVJU.ToSafeString() == "1" ? true : false,
                DiaDeVisitaViernes = x.U_MSS_DVVI.ToSafeString() == "1" ? true : false,
                DiaDeVisitaSabado = x.U_MSS_DVSA.ToSafeString() == "1" ? true : false,
                DiaDeVisitaDomingo = x.U_MSS_DVDO.ToSafeString() == "1" ? true : false,
                FrecuenciaVisita = x.U_MSS_FREC,
                RutaId = x.U_MSS_RUTA,
                GeoOptions = (!string.IsNullOrEmpty(x.U_MSSM_LAT) && !string.IsNullOrEmpty(x.U_MSSM_LON)) ? new GeoOptions() { coords = new Path() { lat = Convert.ToDouble(x.U_MSSM_LAT), lng = Convert.ToDouble(x.U_MSSM_LON) } } : null, //TODO:

                Ruc = x.CardCode,

            }).ToList();
        }

        public Address Get(DataContext dataContext, string id)
        {
            return GetList(dataContext).FirstOrDefault(x => x.CodigoInterno == id);
        }

        public string GetQuery(DataContext dataContext, TreeViewNode node)
        {
            var geoOptions = new GeoOptions();
            if (node.GeoOptions?.coords != null)
            {
                var coordinates = new MapLogic().GetCoordinatesArray(node.GeoOptions, ShapeType.Address);
                geoOptions = new MapLogic().GetGeoOptionsFromCoordinates(coordinates, ShapeType.Address);
            }

            var query = Queries.GetStringContent(EmbebbedFileName.CRD1_Update);
            query = query.Replace(QueryParameters.PARAM1, GetCardCodeAndAddressFromCode(node.Id)[0])
                         .Replace(QueryParameters.PARAM2, geoOptions?.coords?.lat.ToSafeString())
                         .Replace(QueryParameters.PARAM3, geoOptions?.coords?.lng.ToSafeString())
                         .Replace(QueryParameters.PARAM4, GetCardCodeAndAddressFromCode(node.Id)[1]);
            return query;
        }

        public string GetCodeForAddress(OCRDType address)
        {
            return address.CardCode + "-" + address.Address;
        }

        public static string[] GetCardCodeAndAddressFromCode(string code)
        {
            string[] values = new string[2] { string.Empty, string.Empty };
            var items = code.Split('-');
            if (items?.Length > 1 && items?.Length == 2)
                values = items;
            return values;
        }

    }
}
