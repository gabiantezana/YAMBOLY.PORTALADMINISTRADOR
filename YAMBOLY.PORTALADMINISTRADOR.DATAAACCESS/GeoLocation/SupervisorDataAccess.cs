using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation
{
    public class SupervisorDataAccess
    {
        //TODO: REFACTORIZAR!!!!!!!!!!!!!!!
        public List<MSS_SUPEType> GetSAPList(DataContext dataContext, int? codigoVendedor)
        {
            var list = dataContext.oDataService.OSLP.ToList();
            var codigoSupervisor = (list.Where(x => x.SlpCode == codigoVendedor).ToList().FirstOrDefault())?.U_MSS_SUPE.ToSafeString();
            codigoSupervisor = codigoSupervisor.ToSafeString();
            var supervisorList = dataContext.oDataService.MSS_SUPE.ToList();
            return supervisorList.Where(x => x.Code == codigoSupervisor).ToList();
        }
    }
}
