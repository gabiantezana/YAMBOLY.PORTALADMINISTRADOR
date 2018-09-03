using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.PORTALADMINISTRADOR.EXCEPTION;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation
{
    public class UserDefinedValuesDataAccess
    {
        public List<UFD1Type> GetList(DataContext dataContext)
        {
            try
            {
                return dataContext.oDataService.UFD1.ToList();
            }
            catch (DataServiceTransportException ex)
            {
                throw new CustomException(new TempDataEntityException() { Mensaje = "No existe conexión  con  el servidor Hana.", TipoMensaje = MessageTypeException.Warning }, dataContext);
            }
        }
    }
}
