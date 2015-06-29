Get a list of Windows supported countries with C#
==

When developing the [LocaliZune][1] settings editor, one of the challenge was to display a full list of Windows countries to the user. My first approach to meet this challenge was to retrieve a list of countries using the [CultureInfo.GetCultures()][2] method and then get all the country information following [this example][3]. But I quickly realised that a lot of countries were missing from the list and that it does not matched the country list from the Windows control panel location setting. I investigated more deeply on how I could provide the full list of countries supported by the system.

![Geographical Location UI](img/geographical_location_ui.png)

These investigations finally led me to the ultimate solution: invoking [EnumSystemGeoID][4] and [GetGeoInfo][5] from kernel32 system library.

The only way to get a full list of countries is actually to do some P/Invoke because .NET 4.0 does not provides this capability. So I ended up writing my own wrapper to invoke native methods and finally get a full list of countries supported by the current Windows OS.

```csharp
public sealed class SystemGeographicalLocation
{
    public const int GEOCLASS_NATION = 0x10;

    private SystemGeographicalLocation()
    {
    }

    public delegate bool EnumGeoInfoProc(int GeoId);

    public enum SYSGEOTYPE
    {
        GEO_NATION = 0x0001,
        GEO_LATITUDE = 0x0002,
        GEO_LONGITUDE = 0x0003,
        GEO_ISO2 = 0x0004,
        GEO_ISO3 = 0x0005,
        GEO_RFC1766 = 0x0006,
        GEO_LCID = 0x0007,
        GEO_FRIENDLYNAME = 0x0008,
        GEO_OFFICIALNAME = 0x0009,
        GEO_TIMEZONES = 0x000A,
        GEO_OFFICIALLANGUAGES = 0x000B
    }

    [DllImport("Kernel32.dll", SetLastError = true)]
    public static extern int GetGeoInfo(int location, SYSGEOTYPE geoType, StringBuilder lpGeoData, int cchData, int langId);

    [DllImport("Kernel32.dll", SetLastError = true)]
    public static extern int EnumSystemGeoID(int geoClass, int parentGeoId, EnumGeoInfoProc lpGeoEnumProc);
}
```

Then to contain the data I declared a simple type that would match the SYSGEOTYPE enumeration.

```csharp
public class GeographicalLocation
{
    public string Nation { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string ISO2 { get; set; }
    public string ISO3 { get; set; }
    public string Rfc1766 { get; set; }
    public string Lcid { get; set; }
    public string FriendlyName { get; set; }
    public string OfficialName { get; set; }
    public string TimeZones { get; set; }
    public string OfficialLanguages { get; set; }
}
```

And finally, to make it easy to use, I developed a helper class that would do the platform invocation for me and return a list of GeographicalLocation.

```csharp
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Ucodia.GeographicalLocation
{
    public static class GeographicalLocationHelper
    {
        private static List<GeographicalLocation> _geographicalLocations;
        private static List<int> _geoIds;
        private static SystemGeographicalLocation.EnumGeoInfoProc _callback;
        private static int _lcid;

        static GeographicalLocationHelper()
        {
            _geographicalLocations = new List<GeographicalLocation>();
            _geoIds = new List<int>();
            _callback = EnumGeoInfoCallback;
            _lcid = CultureInfo.CurrentCulture.LCID;
        }

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
```

Now you will have a full list of countries available on the current system with their nation code and their RFC1766 code which you can use to instantiate the corresponding [RegionInfo][6] object. For some reasons, all the returned countries have the same LCID, therefore you cannot really use it for anything.

You can get the WPF demo project from the [code folder][7].

[1]: http://localizune.codeplex.com
[2]: http://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.getcultures.aspx
[3]: http://www.daniweb.com/software-development/csharp/threads/265484
[4]: http://msdn.microsoft.com/en-us/library/dd317826%28v=vs.85%29.aspx
[5]: http://msdn.microsoft.com/en-us/library/dd318099%28v=vs.85%29.aspx
[6]: http://msdn.microsoft.com/en-us/library/system.globalization.regioninfo.aspx
[7]: code
