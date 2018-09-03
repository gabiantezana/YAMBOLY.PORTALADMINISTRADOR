using System.Collections.Generic;
using System.Web.Mvc;
using YAMBOLY.PORTALADMINISTRADOR.Controllers;
using YAMBOLY.PORTALADMINISTRADOR.EXCEPTION;
using YAMBOLY.PORTALADMINISTRADOR.Filters;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;
using YAMBOLY.PORTALADMINISTRADOR.LOGIC.Administration;
using YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.Administration.User;

namespace YAMBOLY.PORTALADMINISTRADOR.Areas.Administration.Controllers
{
    public class UserController : BaseController
    {
        [AppViewAuthorize(ConstantHelper.Views.Administration.User.LIST)]
        public ActionResult List()
        {
            var model = new UserLogic().GetListViewModel(GetDataContext());
            return View(model);
        }

        [AppViewAuthorize(ConstantHelper.Views.Administration.User.ADD_UPDATE)]
        public ActionResult AddUpdate(int? id)
        {
            var model = new UserLogic().Get(GetDataContext(), id);
            return View(model);
        }

        #region JsonResult
        public JsonResult GetJList(string searchString)
        {
            var jList = new UserLogic().GetJList(GetDataContext(), searchString);
            return Json(jList);
        }
        #endregion

        #region Post
        [HttpPost]
        [AppViewAuthorize(ConstantHelper.Views.Administration.User.ADD_UPDATE)]
        public ActionResult AddUpdate(UserViewModel model)
        {
            ModelState.Remove("Roles");
            if (!ModelState.IsValid) return RedirectToAction(nameof(AddUpdate), model);
            try
            {
                new UserLogic().AddUpdate(GetDataContext(), model);
                PostMessage(MessageType.Success);
                return RedirectToAction(nameof(List));
            }
            catch (CustomException ex)
            {
                PostMessage(ex);
                return RedirectToAction(nameof(AddUpdate), model);
            }

        }

        [AppViewAuthorize(ConstantHelper.Views.Administration.User.ADD_UPDATE)]
        public ActionResult ChangeState(int? userId)
        {
            try
            {
                new UserLogic().ChangeState(GetDataContext(), userId);
                PostMessage(MessageType.Success);
                return RedirectToAction(nameof(List));
            }
            catch (CustomException ex)
            {
                PostMessage(ex);
                return RedirectToAction(nameof(List));
            }
        }

        [HttpPost]
        [AppViewAuthorize(ConstantHelper.Views.Administration.User.ADD_UPDATE)]
        public ActionResult ResetPass(int? userId)
        {
            try
            {
                new UserLogic().ChangeState(GetDataContext(), userId);
                PostMessage(MessageType.Success);
                return RedirectToAction(nameof(List));
            }
            catch (CustomException ex)
            {
                PostMessage(ex);
                return RedirectToAction(nameof(AddUpdate), userId);
            }
        }

        #endregion

        #region PartialView
        public ActionResult _ListPartialView(int? id, int? p)
        {
            var model = new UserLogic().GetList(GetDataContext(), null, id, p);
            return PartialView("PartialView/_ListPartialView", model);
        }
        #endregion
    }
}