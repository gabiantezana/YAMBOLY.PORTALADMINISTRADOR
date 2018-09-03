using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation;
using YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.Queries;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.LOGIC.GeoLocation
{
    public class DepartamentoLogic
    {
        private List<OCSTType> GetList(DataContext dataContext)
        {
            return new DepartamentosDataAccess().GetSAPList(dataContext);
        }

        public List<JsonEntityTwoString> GetJList(DataContext dataContext)
        {
            return GetList(dataContext).Select(x => new JsonEntityTwoString()
            {
                id = x.Code,
                text = x.Name,
            }).ToList();
        }
    }
}
