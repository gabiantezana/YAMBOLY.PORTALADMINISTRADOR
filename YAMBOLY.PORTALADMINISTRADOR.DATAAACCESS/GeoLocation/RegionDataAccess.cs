﻿using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation
{
    public class RegionDataAccess
    {
        public List<MSS_REGIType> GetSAPList(DataContext dataContext)
        {
            return dataContext.oDataService.MSS_REGI.ToList();
        }
    }
}
