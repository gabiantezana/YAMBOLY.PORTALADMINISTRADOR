using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation
{
    public class RutaDataAccess
    {
        public List<MSS_RUTAType> GetList(DataContext dataContext)
        {
            return dataContext.oDataService.MSS_RUTA.ToList();
        }
    }
}
