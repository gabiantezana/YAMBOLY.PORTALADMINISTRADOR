using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation
{
    public class DepartamentosDataAccess
    {
        public List<OCSTType> GetSAPList(DataContext dataContext)
        {
            return dataContext.oDataService.OCST.Where(x => x.Country == "PE").ToList();//TODO:
        }
    }
}
