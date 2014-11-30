using System.Collections.ObjectModel;
using System.Linq;

namespace Ucodia.GeographicalLocation
{
    /// <summary>
    /// Represents the MainWindow view-model.
    /// </summary>
    public class MainWindowViewModel
    {
        /// <summary>
        /// The list of locations.
        /// </summary>
        private ObservableCollection<GeographicalLocation> _locations;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
        {
            _locations = new ObservableCollection<GeographicalLocation>();
            LoadLocations();
        }

        /// <summary>
        /// Gets the list of locations.
        /// </summary>
        public ObservableCollection<GeographicalLocation> Locations
        {
            get
            {
                return _locations;
            }
        }

        /// <summary>
        /// Loads current system available locations
        /// </summary>
        private void LoadLocations()
        {
            var locations = GeographicalLocationHelper.GetGeographicalLocations().OrderBy(gl => gl.FriendlyName);

            _locations.Clear();

            foreach (var location in locations)
            {
                _locations.Add(location);
            }
        }
     }
}
