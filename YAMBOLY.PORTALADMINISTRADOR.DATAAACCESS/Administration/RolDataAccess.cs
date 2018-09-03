using System.Collections.Generic;
using System.Linq;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.Administration
{
    public class RolDataAccess
    {
        public List<Roles> Get(DataContext dataContext)
        {
            return dataContext.context.Roles.ToList();
        }

        public Roles Get(DataContext dataContext, int? id)
        {
            return dataContext.context.Roles.Find(id);
        }
    }
}
