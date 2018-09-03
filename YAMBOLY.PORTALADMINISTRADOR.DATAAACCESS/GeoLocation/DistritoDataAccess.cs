using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;
using static ConvertHelper;

namespace YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation
{
    public class DistritoDataAccess
    {
        public IEnumerable<JsonEntityTwoString> GetList(DataContext dataContext, string provinciaId)
        {
            return dataContext.oDataService.MSSL_UBI.Where(x => x.U_MSSL_NDI == provinciaId.ToSafeString())
                                                     .Select(x => new JsonEntityTwoString() { id = x.U_MSSL_NDI, text = x.U_MSSL_NDI }).ToList()
                                                     .Distinct(new JsonEntityComparer());
        }
    }
}
