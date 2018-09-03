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
    public class VendedorLogic
    {
        private List<OSLPType> GetList(DataContext dataContext)
        {
            return new VendedorDataAccess().GetSAPList(dataContext);
        }

        public List<JsonEntityTwoString> GetJList(DataContext dataContext)
        {
            return GetList(dataContext).Select(x => new JsonEntityTwoString()
            {
                id = x.SlpCode.ToSafeString(),
                text = x.SlpName,
            }).ToList();
        }
    }
}
