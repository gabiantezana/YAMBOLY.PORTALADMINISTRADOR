using OfficeOpenXml;
using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using YAMBOLY.PORTALADMINISTRADOR.Controllers;
using YAMBOLY.PORTALADMINISTRADOR.EXCEPTION;
using YAMBOLY.PORTALADMINISTRADOR.Filters;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;
using YAMBOLY.PORTALADMINISTRADOR.LOGIC.GeoLocation;
using YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.GeoLocation;
using static System.Net.WebRequestMethods;

namespace YAMBOLY.PORTALADMINISTRADOR.Areas.GeoLocation.Controllers
{
    public class MapController : BaseController
    {
        // GET: GeoLocation/Zone
        [AppViewAuthorize(ConstantHelper.Views.GeoLocation.Map.VIEW)]
        public ActionResult Index()
        {
            try
            {
                var model = new MapLogic().GetMapViewModel(GetDataContext());
                return View(model);
            }
            catch (CustomException ex)
            {
                PostMessage(ex);
                return View(new MapViewModel());
            }
        }

        [AppViewAuthorize(ConstantHelper.Views.GeoLocation.Map.VIEW)]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// El modelo no debe retornar zonas repetidas, ni rutas repetidas
        /// </summary>
        /// <returns></returns>

        [AppViewAuthorize(ConstantHelper.Views.GeoLocation.Map.UPDATE)]
        [HttpPost]
        public ActionResult Index(MapViewModel model)
        {
            new MapLogic().AddUpdateMap(GetDataContext(), model);
            PostMessage(MessageType.Success);
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public virtual ActionResult SaveReport(MapViewModel model)
        {
            var report = new MapLogic().GetReport(GetDataContext(), model);
            switch (model.ReportType)
            {
                case ReportType.Excel:
                    return File((MemoryStream)report, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                case ReportType.Pdf:
                    return new FilePathResult(report, System.Net.Mime.MediaTypeNames.Application.Pdf);
                default://TODO
                    throw new Exception();//TODO:
            }
        }

        #region Ajax JsonResult
        [AppViewAuthorize(ConstantHelper.Views.GeoLocation.Map.VIEW)]
        [HttpPost]
        public JsonResult GetShapeInfo(string id, ShapeType shapeType)
        {
            try
            {
                var shapeInfo = new MapLogic().GetShapeInfo(GetDataContext(), id, shapeType);
                return Json(shapeInfo);
            }
            catch (Exception ex)
            {
                return AjaxException(TypeAjaxException.Error, ex);
            }
        }

        [AppViewAuthorize(ConstantHelper.Views.GeoLocation.Map.VIEW)]
        public JsonResult SearchClient(MapViewModel model)
        {
            try
            {
                var data = new MapLogic().GetFilteredClientList(GetDataContext(), model);
                return Json(data);
            }
            catch (Exception ex)
            {
                return AjaxException(TypeAjaxException.Error, ex);
            }
        }

        [AppViewAuthorize(ConstantHelper.Views.GeoLocation.Map.VIEW)]
        public JsonResult GetProvinciaJList(String CadenaBuscar)
        {
            try
            {

                var data = new ProvinciaLogic().GetJList(GetDataContext(), CadenaBuscar);
                return Json(data);
            }
            catch (Exception ex)
            {
                return AjaxException(TypeAjaxException.Error, ex);
            }
        }

        [AppViewAuthorize(ConstantHelper.Views.GeoLocation.Map.VIEW)]
        public JsonResult GetDistritoJList(String CadenaBuscar)
        {
            try
            {
                var data = new DistritoLogic().GetJList(GetDataContext(), CadenaBuscar);
                return Json(data);
            }
            catch (Exception ex)
            {
                return AjaxException(TypeAjaxException.Error, ex);
            }
        }

        [AppViewAuthorize(ConstantHelper.Views.GeoLocation.Map.VIEW)]
        public JsonResult GetRutaByZoneJList(string CadenaBuscar)
        {
            try
            {
                var data = new RutaLogic().GetJList(GetDataContext(), CadenaBuscar);
                return Json(data);
            }
            catch (Exception ex)
            {
                return AjaxException(TypeAjaxException.Error, ex);
            }
        }

        [AppViewAuthorize(ConstantHelper.Views.GeoLocation.Map.VIEW)]
        public JsonResult GetSupervisorJList(int CadenaBuscar)
        {
            try
            {
                var data = new SupervisorLogic().GetJList(GetDataContext(), CadenaBuscar);
                return Json(data);
            }
            catch (Exception ex)
            {
                return AjaxException(TypeAjaxException.Error, ex);
            }
        }

        [AppViewAuthorize(ConstantHelper.Views.GeoLocation.Map.VIEW)]
        public JsonResult GetJefeVentasJList(int CadenaBuscar)
        {
            try
            {
                var data = new JefeVentasLogic().GetJList(GetDataContext(), CadenaBuscar);
                return Json(data);
            }
            catch (Exception ex)
            {
                return AjaxException(TypeAjaxException.Error, ex);
            }
        }
        #endregion

    }
}