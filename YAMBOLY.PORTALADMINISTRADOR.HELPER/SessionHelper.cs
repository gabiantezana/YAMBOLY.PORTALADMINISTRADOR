using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace YAMBOLY.PORTALADMINISTRADOR.HELPER
{
    public static class SessionHelper
    {
        #region HasKey

        public static bool HasKey(this HttpSessionStateBase Session, SessionKey Key)
        {
            var temp = Get(Session, Key);

            if (temp == null)
                return false;

            return true;
        }

        public static bool HasKey(this HttpSessionState Session, SessionKey Key)
        {
            var temp = Get(Session, Key);

            if (temp == null)
                return false;

            return true;
        }

        #endregion

        #region HasKey

        public static bool RemoveKey(this HttpSessionStateBase Session, SessionKey Key)
        {
            var temp = Get(Session, Key);

            if (temp == null)
                return false;

            Session.Remove(Key.ToString());
            return true;
        }

        public static bool RemoveKey(this HttpSessionState Session, SessionKey Key)
        {
            var temp = Get(Session, Key);

            if (temp == null)
                return false;

            Session.Remove(Key.ToString());
            return true;
        }

        #endregion

        #region Set & Get
        public static object Get(HttpSessionStateBase Session, SessionKey Key)
        {
            return Session[Key.ToString()];
        }
        public static object Get(HttpSessionState Session, SessionKey Key)
        {
            return Session[Key.ToString()];
        }
        public static void Set(this HttpSessionStateBase Session, SessionKey Key, object value)
        {
            Session[Key.ToString()] = value;
        }
        public static void Set(this HttpSessionState Session, SessionKey Key, object value)
        {
            Session[Key.ToString()] = value;
        }
        #endregion

        #region GetUserId
        public static Int32? GetUserId(this HttpSessionStateBase Session)
        {
            return (Int32?)Get(Session, SessionKey.UserId);
        }
        public static Int32? GetUserId(this HttpSessionState Session)
        {
            return (Int32?)Get(Session, SessionKey.UserId);
        }

        #endregion

        #region GetUserName
        public static String GetUserName(this HttpSessionStateBase Session)
        {
            return Get(Session, SessionKey.UserName).ToString();
        }
        public static String GetUserName(this HttpSessionState Session)
        {
            return Get(Session, SessionKey.UserName).ToString();
        }
        #endregion

        #region GetName
        public static String GetName(this HttpSessionStateBase Session)
        {
            return Get(Session, SessionKey.UserNames).ToSafeString();
        }
        public static String GetName(this HttpSessionState Session)
        {
            return Get(Session, SessionKey.UserNames).ToSafeString();
        }
        #endregion


        #region GetRol
        public static AppRol GetRol(this HttpSessionStateBase Session)
        {
            return (AppRol)Get(Session, SessionKey.Rol);
        }
        public static AppRol GetRol(this HttpSessionState Session)
        {
            return (AppRol)Get(Session, SessionKey.Rol);
        }
        #endregion


        #region GetPermisosVista
        public static String[] GetViewPermissions(this HttpSessionStateBase Session)
        {
            return (String[])Get(Session, SessionKey.Views);
        }
        public static String[] GetViewPermissions(this HttpSessionState Session)
        {
            return (String[])Get(Session, SessionKey.Views);
        }
        #endregion

    }
}
