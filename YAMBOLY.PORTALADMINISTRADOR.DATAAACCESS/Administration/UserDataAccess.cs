using System;
using System.Collections.Generic;
using System.Linq;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;
using YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.Administration.User;
using YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.General;

namespace YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.Administration
{
    public class UserDataAccess
    {
        public List<Users> Get(DataContext dataContext)
        {
            return dataContext.context.Users.ToList();
        }

        public Users Get(DataContext dataContext, int? userId)
        {
            return dataContext.context.Users.Find(userId);
        }

        public void AddUpdate(DataContext dataContext, UserViewModel model)
        {
            Users user = new Users();
            Boolean itsUpdate = dataContext.context.Users.Find(model.UserId) != null;
            if (itsUpdate)
            {
                user = dataContext.context.Users.Find(model.UserId);
            }
            else
            {
                var byteArrayPassword = EncryptionHelper.EncryptTextToMemory(ConstantHelper.PASSWORD_DEFAULT, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
                user.Pass = byteArrayPassword;
            }
            user.UserName = model.UserName;
            user.RolId = model.RolId;
            user.Nombres = model.Nombres;
            user.Documento = model.Documento;
            user.Correo = model.Correo;
            user.Telefono = model.Telefono;

            if (itsUpdate)
                dataContext.context.Entry(user);
            else
                dataContext.context.Users.Add(user);
            dataContext.context.SaveChanges();
        }

        public void ChangePassword(DataContext dataContext, int? userId, ChangePasswordViewModel model)
        {
            var entity = dataContext.context.Users.Find(userId);
            if (entity != null)
            {
                entity.Pass = EncryptionHelper.EncryptTextToMemory(model.Password, ConstantHelper.ENCRIPT_KEY, ConstantHelper.ENCRIPT_METHOD);
                dataContext.context.SaveChanges();
            }
        }

        public void ChangeState(DataContext dataContext, int? userId, bool? newState)
        {
            var entity = dataContext.context.Users.Find(userId);
            if (userId != null)
            {
                entity.isActive = newState;
                dataContext.context.Entry(entity);
                dataContext.context.SaveChanges();
            }
        }
    }
}
