﻿using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Ucodia.GeographicalLocation
{
    /// <summary>
    /// Helper class to retrieve geographical locations through a P/Invoke wrapper.
    /// </summary>
    public static class GeographicalLocationHelper
    {
        /// <summary>
        /// The list of geographical locations.
        /// </summary>
        private static List<GeographicalLocation> _geographicalLocations;

        /// <summary>
        /// The list of geographical ids.
        /// </summary>
        private static List<int> _geoIds;

        /// <summary>
        /// The EnumGeoInfoProc callback delegate.
        /// </summary>
        private static SystemGeographicalLocation.EnumGeoInfoProc _callback;

        /// <summary>
        /// The current application locale id.
        /// </summary>
        private static int _lcid;

        /// <summary>
        /// Initializes static members of the <see cref="GeographicalLocationHelper"/> class.
        /// </summary>
        static GeographicalLocationHelper()
        {
            _geographicalLocations = new List<GeographicalLocation>();
            _geoIds = new List<int>();
            _callback = EnumGeoInfoCallback;
            _lcid = CultureInfo.CurrentCulture.LCID;
        }

        /// <summary>
        /// Gets the geographical locations.
        /// </summary>
        /// <returns>The geographical locations.</returns>
        public static IEnumerable<GeographicalLocation> GetGeographicalLocations()
        {
            if (_geographicalLocations.Count == 0)
            {
                SystemGeographicalLocation.EnumSystemGeoID(SystemGeographicalLocation.GEOCLASS_NATION, 0, _callback);

                foreach (var geoId in _geoIds)
                {
                    var location = new GeographicalLocation();

                    location.Nation             = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_NATION, _lcid);
                    location.Latitude           = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_LATITUDE, _lcid);
                    location.Longitude          = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_LONGITUDE, _lcid);
                    location.ISO2               = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_ISO2, _lcid);
                    location.ISO3               = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_ISO3, _lcid);
                    location.Rfc1766            = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_RFC1766, _lcid);
                    location.Lcid               = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_LCID, _lcid);
                    location.FriendlyName       = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_FRIENDLYNAME, _lcid);
                    location.OfficialName       = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_OFFICIALNAME, _lcid);
                    location.TimeZones          = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_TIMEZONES, _lcid);
                    location.OfficialLanguages  = GetGeoInfo(geoId, SystemGeographicalLocation.SYSGEOTYPE.GEO_OFFICIALLANGUAGES, _lcid);

                    _geographicalLocations.Add(location);
                } 
            }

            return _geographicalLocations;
        }

        /// <summary>
        /// Retrieves information about a specified geographical location.
        /// </summary>
        /// <param name="location">Identifier for the geographical location for which to get information.</param>
        /// <param name="geoType">Type of information to retrieve.</param>
        /// <param name="langId">Identifier for the language.</param>
        /// <returns>
        /// Returns the queried geographical data as a string.
        /// </returns>
        private static string GetGeoInfo(int location, SystemGeographicalLocation.SYSGEOTYPE geoType, int langId)
        {
            var geoDataBuilder = new StringBuilder();
            int bufferSize = 0;

            bufferSize = SystemGeographicalLocation.GetGeoInfo(location, geoType, geoDataBuilder, 0, langId);

            if (bufferSize > 0)
            {
                geoDataBuilder.Capacity = bufferSize;
                SystemGeographicalLocation.GetGeoInfo(location, geoType, geoDataBuilder, bufferSize, langId);
            }

            return geoDataBuilder.ToString();
        }

        /// <summary>
        /// EnumGeoInfoProc callback method.
        /// </summary>
        /// <param name="geoId">The geo id.</param>
        /// <returns>Returns true to continue enumeration or false otherwise.</returns>
        private static bool EnumGeoInfoCallback(int geoId)
        {
            if (geoId != 0)
            {
                _geoIds.Add(geoId);
                return true;
            }

            return false;
        }
    }
}
