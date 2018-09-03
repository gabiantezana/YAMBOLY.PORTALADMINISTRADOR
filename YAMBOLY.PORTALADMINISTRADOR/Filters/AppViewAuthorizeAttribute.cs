using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;

namespace YAMBOLY.PORTALADMINISTRADOR.Filters
{
    public class AppViewAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly String[] _views;

        public AppViewAuthorizeAttribute(params String[] views)
        {
            _views = views;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var unauthorized = false;

            try
            {
                var views = filterContext.HttpContext.Session.GetViewPermissions();

                var intersect = views.Intersect(_views);
                if (intersect.Count() == 0)
                    unauthorized = true;
            }
            catch (Exception)
            {
                unauthorized = true;
            }

            if (unauthorized)
            {
                if (filterContext.HttpContext.Session.GetUserId() == null)
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Login", area = "" }));

                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "PermisoInsuficiente", area = "" }));
                }
            }
        }


    }
}