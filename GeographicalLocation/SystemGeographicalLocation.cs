using System.Runtime.InteropServices;
using System.Text;

namespace Ucodia.GeographicalLocation
{
    /// <summary>
    /// P/Invoke wrapper for retrieving system geographical informations.
    /// </summary>
    public sealed class SystemGeographicalLocation
    {
        /// <summary>
        /// Type used to enumerate all geographical identifiers for nations on the operating system.
        /// </summary>
        public const int GEOCLASS_NATION = 0x10;

        /// <summary>
        /// Prevents a default instance of the <see cref="SystemGeographicalLocation"/> class from being created.
        /// </summary>
        private SystemGeographicalLocation()
        {
        }

        /// <summary>
        /// An application-defined callback function that processes enumerated geographical location information provided by the EnumSystemGeoID function.
        /// </summary>
        /// <param name="GeoId">Identifier of the geographical location to check.</param>
        /// <returns>Returns true to continue enumeration or false otherwise.</returns>
        public delegate bool EnumGeoInfoProc(int geoId);

        /// <summary>
        /// Defines the type of geographical location information requested in the GetGeoInfo function.
        /// </summary>
        public enum SYSGEOTYPE
        {
            /// <summary>
            /// The geographical location identifier (GEOID) of a nation.
            /// </summary>
            GEO_NATION = 0x0001,

            /// <summary>
            /// The latitude of the location.
            /// </summary>
            GEO_LATITUDE = 0x0002,

            /// <summary>
            /// The longitude of the location.
            /// </summary>
            GEO_LONGITUDE = 0x0003,

            /// <summary>
            /// The ISO 2-letter country/region code.
            /// </summary>
            GEO_ISO2 = 0x0004,

            /// <summary>
            /// The ISO 3-letter country/region code.
            /// </summary>
            GEO_ISO3 = 0x0005,

            /// <summary>
            /// The name for a string, compliant with RFC 4646 (Windows Vista and later), that is derived
            /// from the GetGeoInfo parameters language and GeoId.
            /// </summary>
            GEO_RFC1766 = 0x0006,

            /// <summary>
            /// A locale identifier derived using GetGeoInfo.
            /// </summary>
            GEO_LCID = 0x0007,

            /// <summary>
            /// The friendly name of the nation, for example, Germany.
            /// </summary>
            GEO_FRIENDLYNAME = 0x0008,

            /// <summary>
            /// The official name of the nation, for example, Federal Republic of Germany.
            /// </summary>
            GEO_OFFICIALNAME = 0x0009,

            /// <summary>
            /// Not implemented.
            /// </summary>
            GEO_TIMEZONES = 0x000A,

            /// <summary>
            /// Not implemented.
            /// </summary>
            GEO_OFFICIALLANGUAGES = 0x000B
        }

        /// <summary>
        /// Retrieves information about a specified geographical location.
        /// </summary>
        /// <param name="location">Identifier for the geographical location for which to get information.</param>
        /// <param name="geoType">Type of information to retrieve.</param>
        /// <param name="lpGeoData">Pointer to the buffer in which this function retrieves the information.</param>
        /// <param name="cchData">Size of the buffer.</param>
        /// <param name="langId">Identifier for the language.</param>
        /// <returns>
        /// Returns the number of bytes (ANSI) or words (Unicode) of geographical location information 
        /// retrieved in the output buffer.
        /// </returns>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern int GetGeoInfo(int location, SYSGEOTYPE geoType, StringBuilder lpGeoData, int cchData, int langId);

        /// <summary>
        /// Enumerates the geographical location identifiers (type GEOID) that are available on the operating system.
        /// </summary>
        /// <param name="geoClass">Geographical location class for which to enumerate the identifiers.</param>
        /// <param name="parentGeoId">Reserved. This parameter must be 0.</param>
        /// <param name="lpGeoEnumProc">Pointer to the application-defined callback function EnumGeoInfoProc.</param>
        /// <returns>Returns a nonzero value if successful, or 0 otherwise.</returns>
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern int EnumSystemGeoID(int geoClass, int parentGeoId, EnumGeoInfoProc lpGeoEnumProc);
    }
}
