using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace YAMBOLY.PORTALADMINISTRADOR.MODEL
{
    public class DataContext
    {
        public HttpSessionStateBase session { get; set; }
        public YAMBOLY_PORTALADMINISTRADOREntities context { get; set; }
        public String currentCulture { get; set; }
        public String systemNameSpace { get; set; }
        public HttpBrowserCapabilitiesBase Browser { get; set; }
        public ODataService oDataService { get; set; }

    }
}
