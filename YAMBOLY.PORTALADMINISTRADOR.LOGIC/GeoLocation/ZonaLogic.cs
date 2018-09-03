using MSS_YAMBOLY_GEOLOCATION.Services.ODataService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.GeoLocation;
using YAMBOLY.PORTALADMINISTRADOR.DATAAACCESS.Queries;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.LOGIC.GeoLocation
{
    public class ZonaLogic
    {
        private List<MSS_ZONAType> GetSAPList(DataContext dataContext)
        {
            return new ZonaDataAccess().GetList(dataContext);
        }

        public List<Zone> GetList(DataContext dataContext)
        {
            return GetSAPList(dataContext).Select(x => new Zone()
            {
                Id = x.Code,
                Name = x.Name,
                GeoOptions = new MapLogic().GetGeoOptionsFromCoordinates(x.U_COORDINATESARRAY, ShapeType.Zone)
            }).ToList();
        }

        public Zone Get(DataContext dataContext, string id)
        {
            return GetList(dataContext).FirstOrDefault(x => x.Id== id);
        }


        public string GetQuery(DataContext dataContext, TreeViewNode zone)
        {
            string coordinates = string.Empty;
            if (zone.GeoOptions != null)
                coordinates = new MapLogic().GetCoordinatesArray(zone.GeoOptions, ShapeType.Zone);

            var query = Queries.GetStringContent(EmbebbedFileName.MSS_ZONA_Update);
            query = query.Replace("PARAM1", zone.Id)
                         .Replace("PARAM2", coordinates);

            return query;
        }

        public void DoQuery(string finalQuery)
        {
            var response = WebHelper.GetJsonResponse(finalQuery, null, null);

        }

        public List<JsonEntityTwoString> GetJList(DataContext dataContext)
        {
            return GetSAPList(dataContext).Select(x => new JsonEntityTwoString()
            {
                id = x.Code,
                text = x.Name,
            }).ToList();
        }

    }
}
