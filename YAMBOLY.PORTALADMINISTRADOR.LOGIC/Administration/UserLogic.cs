using PagedList;
using System.Collections.Generic;
using System.Linq;
using YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.Administration;
using YAMBOLY.PORTALADMINISTRADOR.EXCEPTION;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;
using YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.Administration.User;
using YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.General;

namespace YAMBOLY.PORTALADMINISTRADOR.LOGIC.Administration
{
    public class UserLogic
    {
        public UserListViewModel GetListViewModel(DataContext dataContext)
        {
            var model = new UserListViewModel();
            model.List = GetList(dataContext, null, null, null);
            return model;
        }

        public IPagedList<Users> GetList(DataContext dataContext, string searchString, int? id, int? page)
        {
            var query = new UserDataAccess().Get(dataContext).AsQueryable();
            var p = page ?? 1;

            if (id != null)
                query = query.Where(x => x.UserId == id);

            else if (!string.IsNullOrEmpty(searchString))
            {
                foreach (var token in searchString.Split(' '))
                    query = query.Where(x => x.UserName.Contains(token));
            }

            return query.ToPagedList(p, ConstantHelper.DEFAULT_PAGE_SIZE);
        }

        public UserViewModel Get(DataContext dataContext, int? id)
        {
            var user = new UserDataAccess().Get(dataContext, id);
            var model = ConvertHelper.CopyAToB(user, new UserViewModel()) as UserViewModel;

            if (user?.Pass != null)
                model.Pass= user.Pass.Equals(EncryptionHelper.CreateMD5(ConstantHelper.PASSWORD_DEFAULT)) ? ConstantHelper.PASSWORD_DEFAULT: ConstantHelper.PASSWORD_HIDDEN;
            FillModelJLists(dataContext, ref model);

            return model;
        }

        public List<JsonEntity> GetJList(DataContext dataContext, string searchString)
        {
            return GetList(dataContext, searchString, null, null).Select(x => new JsonEntity() { id = x.UserId, text = x.UserName }).ToList();
        }

        private void FillModelJLists(DataContext dataContext, ref UserViewModel model)
        {
            model.Roles = new RolLogic().GetList(dataContext, null);
        }

        public void AddUpdate(DataContext dataContext, UserViewModel model)
        {
            new UserDataAccess().AddUpdate(dataContext, model);
        }

        public void ChangePassword(DataContext dataContext, ChangePasswordViewModel model)
        {
            var userId = dataContext.session.GetUserId();
            if (userId == null)
                throw new CustomException("No se encontró el id de usuario en la sesión.");
            new UserDataAccess().ChangePassword(dataContext, userId, model);
        }

        public void ChangeState(DataContext dataContext, int? userId)
        {
            var entity = new UserDataAccess().Get(dataContext, userId);
            if (entity != null)
            {
                var newState = !(entity.isActive ?? false);
                new UserDataAccess().ChangeState(dataContext, userId, newState);
            }
        }
    }
}
