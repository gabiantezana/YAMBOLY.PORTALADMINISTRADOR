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
    public class ProvinciaDataAccess
    {
        public IEnumerable<JsonEntityTwoString> GetList(DataContext dataContext, string departamentoId)
        {
            return dataContext.oDataService.MSSL_UBI.Where(x => x.U_MSSL_CDP == departamentoId.ToSafeString())
                                                     .Select(x => new JsonEntityTwoString() { id = x.U_MSSL_NPR, text = x.U_MSSL_NPR }).ToList()
                                                     .Distinct(new JsonEntityComparer());
        }
    }


}
