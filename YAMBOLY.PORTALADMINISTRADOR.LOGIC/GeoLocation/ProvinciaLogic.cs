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
    public class ProvinciaLogic
    {
        public List<JsonEntityTwoString> GetJList(DataContext dataContext, string departamentoId)
        {
            return new ProvinciaDataAccess().GetList(dataContext, departamentoId).ToList();
        }
    }
}
