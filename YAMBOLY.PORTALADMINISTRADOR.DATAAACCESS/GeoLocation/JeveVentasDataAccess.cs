using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation
{
    public class JefeVentasDataAccess
    {
        //TODO: REFACTORIZAR!!!!!!!!!!!!!!!
        public List<MSS_JEVEType> GetSAPList(DataContext dataContext, int? codigoVendedor)
        {
            var listaVendedores = dataContext.oDataService.OSLP.ToList();
            var codigoJefeVentas = (listaVendedores.Where(x => x.SlpCode == codigoVendedor).ToList().FirstOrDefault())?.U_MSS_JEVE.ToSafeString();
            codigoJefeVentas = codigoJefeVentas.ToSafeString();
            var jefeventasList= dataContext.oDataService.MSS_JEVE.ToList();
            return jefeventasList.Where(x => x.Code == codigoJefeVentas).ToList();
        }
    }
}
