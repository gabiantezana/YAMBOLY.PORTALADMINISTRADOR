using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMBOLY.PORTALADMINISTRADOR.HELPER
{
    public class EnumHelper
    {
    }

    public enum MessageType
    {
        Success,
        Warning,
        Info,
        Error
    }

    public enum AppRol
    {
        SUPERADMINISTRATOR = 0,
        ADMINISTRATOR = 1,
        ROUTESMANAGER = 2,
        MAPVIEWER =  3,
    }

    public enum SessionKey
    {
        UserId,
        Rol,
        UserName,
        UserNames,
        Views,
    }

    #region MODAL SIZE
    public enum ModalSize
    {
        Normal,
        Small,
        Large
    }
    #endregion


    public enum ShapeType
    {
        Zone = 1,
        Route = 2,
        Address = 3,
    }

    public enum EmbebbedFileName
    {
        @MSS_ZONA_Update = 0,
        @MSS_RUTA_Update = 1,
        CRD1_Update = 2,
        @MSS_GEOSHAPE_D_Disable = 22,
        @MSS_GEOSHAPE_D_Insert = 222,
        @MSS_GEOSHAPE_Insert = 223,
    }

    public enum ReportType
    {
        Excel = 1,
        Pdf = 2,
    }
}
