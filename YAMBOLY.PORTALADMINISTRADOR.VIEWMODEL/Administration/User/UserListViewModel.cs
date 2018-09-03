using PagedList;
using System;
using System.Collections.Generic;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.Administration.User
{
    public class UserListViewModel
    {
        public UserListViewModel()
        {
            DefaultList = new List<Users>();
        }

        public IPagedList<Users> List { get; set; }
        public String SearchString { get; set; }

        /// <summary>
        /// Solo para dar formato al select2 en la búsqueda 
        /// predictiva
        /// </summary>
        public List<Users> DefaultList { get; set; }
    }
}
