namespace Ucodia.GeographicalLocation
{
    /// <summary>
    /// Represents a geographical location.
    /// </summary>
    public class GeographicalLocation
    {
        /// <summary>
        /// Gets or sets the location nation code.
        /// </summary>
        /// <value>
        /// The location nation code.
        /// </value>
        public string Nation { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets the location longitude.
        /// </summary>
        /// <value>
        /// The location longitude.
        /// </value>
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets the location 2 letters ISO code.
        /// </summary>
        /// <value>
        /// The location 2 letters ISO code.
        /// </value>
        public string ISO2 { get; set; }

        /// <summary>
        /// Gets or sets the location 3 letters ISO code.
        /// </summary>
        /// <value>
        /// The location 3 letters ISO code..
        /// </value>
        public string ISO3 { get; set; }

        /// <summary>
        /// Gets or sets the location RFC1766 code.
        /// </summary>
        /// <value>
        /// The location RFC1766 code.
        /// </value>
        public string Rfc1766 { get; set; }

        /// <summary>
        /// Gets or sets the location LCID.
        /// </summary>
        /// <value>
        /// The location LCID.
        /// </value>
        public string Lcid { get; set; }

        /// <summary>
        /// Gets or sets the location friendly name.
        /// </summary>
        /// <value>
        /// The location friendly name.
        /// </value>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the location official name.
        /// </summary>
        /// <value>
        /// The location official name.
        /// </value>
        public string OfficialName { get; set; }

        /// <summary>
        /// Gets or sets the location time zones.
        /// </summary>
        /// <value>
        /// The location time zones.
        /// </value>
        public string TimeZones { get; set; }

        /// <summary>
        /// Gets or sets the location official languages.
        /// </summary>
        /// <value>
        /// The location official languages.
        /// </value>
        public string OfficialLanguages { get; set; }
    }
}
