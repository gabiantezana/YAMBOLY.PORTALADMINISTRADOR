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
    public class JefeVentasLogic
    {
        private List<MSS_JEVEType> GetList(DataContext dataContext, int? codigoVendedor)
        {
            return new JefeVentasDataAccess().GetSAPList(dataContext, codigoVendedor);
        }

        public List<JsonEntityTwoString> GetJList(DataContext dataContext, int? codigoVendedor)
        {
            return GetList(dataContext, codigoVendedor).Select(x => new JsonEntityTwoString()
            {
                id = x.Name,
                text = x.Name,
            }).ToList();
        }
    }
}
