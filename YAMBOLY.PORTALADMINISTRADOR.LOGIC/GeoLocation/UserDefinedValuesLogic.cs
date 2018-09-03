using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.LOGIC.GeoLocation
{
    public class UserDefinedValuesLogic
    {
        public List<JsonEntityTwoString> GetJList(DataContext dataContext, string tableName, int? index)
        {
            return new UserDefinedValuesDataAccess().GetList(dataContext).Where(x=> x.FieldID ==  index && x.TableID  == tableName)
                                                    .Select(x=> new JsonEntityTwoString() { id = x.FldValue, text = x.Descr}).ToList();
        }
    }
}
