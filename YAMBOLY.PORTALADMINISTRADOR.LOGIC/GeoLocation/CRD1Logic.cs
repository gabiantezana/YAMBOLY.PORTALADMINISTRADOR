using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.GESTIONRUTAS.MODEL;

namespace YAMBOLY.GESTIONRUTAS.LOGIC.GeoLocation
{
    public class CRD1Logic
    {
        public List<CRD1Type> GetList(DataContext dataContext)
        {
            return new CRD1Logic().GetList(dataContext);
        }
    }
}
