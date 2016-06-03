using System.Data.Entity.Spatial;

namespace Drs.Model.Address
{
    public static class GeoHelper
    {
        private const int SRID_GOOGLE_MAPS = 4326;
        private const int SRID_CUSTOM_MAP = 3857;



        public static DbGeography PointFromText(string lat, string lng)
        {
            return DbGeography.PointFromText(string.Format("POINT({0} {1})", lng, lat), SRID_GOOGLE_MAPS);
        }

        public static DbGeography PolygonFromText(string sPolygon)
        {
            return DbGeography.PolygonFromText(string.Format("POLYGON({0})", sPolygon), SRID_GOOGLE_MAPS);
        }
    }
}
