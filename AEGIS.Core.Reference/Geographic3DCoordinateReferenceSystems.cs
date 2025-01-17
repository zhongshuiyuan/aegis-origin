﻿/// <copyright file="Geographic3DCoordinateReferenceSystems.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2022 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELTE.AEGIS.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="GeographicCoordinateReferenceSystem" /> instances for 3D coordinates.
    /// </summary>
    [IdentifiedObjectCollection(typeof(GeographicCoordinateReferenceSystem))]
    public static class Geographic3DCoordinateReferenceSystems
    {
        #region Query fields

        private static GeographicCoordinateReferenceSystem[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="GeographicCoordinateReferenceSystem" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="GeographicCoordinateReferenceSystem" /> instances within the collection.</value>
        public static IList<GeographicCoordinateReferenceSystem> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(Geographic3DCoordinateReferenceSystems).GetProperties().
                                                                          Where(property => property.Name != "All").
                                                                          Select(property => property.GetValue(null, null) as GeographicCoordinateReferenceSystem).
                                                                          ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="GeographicCoordinateReferenceSystem" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A read-only list containing the <see cref="GeographicCoordinateReferenceSystem" /> instances that match the specified identifier.</returns>
        public static IList<GeographicCoordinateReferenceSystem> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            // identifier correction
            identifier = Regex.Escape(identifier);

            return All.Where(obj => Regex.IsMatch(obj.Identifier, identifier, RegexOptions.IgnoreCase)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="GeographicCoordinateReferenceSystem" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A read-only list containing the <see cref="GeographicCoordinateReferenceSystem" /> instances that match the specified name.</returns>
        public static IList<GeographicCoordinateReferenceSystem> FromName(String name)
        {
            if (name == null)
                return null;

            // name correction
            name = Regex.Escape(name);

            return All.Where(obj => Regex.IsMatch(obj.Name, name, RegexOptions.IgnoreCase) ||
                                    obj.Aliases != null && obj.Aliases.Any(alias => Regex.IsMatch(alias, name, RegexOptions.IgnoreCase))).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static GeographicCoordinateReferenceSystem _ETRS89;
        private static GeographicCoordinateReferenceSystem _NAD83NSR2007;
        private static GeographicCoordinateReferenceSystem _WGS84;

        #endregion

        #region Public static properties

        /// <summary>
        /// European Terrestrial System 1989 (ETRS89).
        /// </summary>
        public static GeographicCoordinateReferenceSystem ETRS89
        {
            get
            {
                return _ETRS89 ?? (_ETRS89 = 
                    new GeographicCoordinateReferenceSystem("EPSG::4937", "ETRS89",
                                                            "The distinction in usage between ETRF89 and ETRS89 is confused: although in principle conceptually different in practice both are used as synonyms.",
                                                            new String[] { "ETRF89", "ETRS89-GRS80h", "ETRS 1989", "ETRF 1989", "European Terrestrial System 1989", "European Terrestrial Reference Frame 1989", "European Terrestrial System 1989 (ETRS89)", "European Terrestrial Reference Frame 1989 (ETRF89)" },
                                                            "Geodetic survey.",
                                                            CoordinateSystems.EllipsoidalLatLonHiDM,
                                                            GeodeticDatums.ETRS89,
                                                            AreasOfUse.EuropeETRS89));
            }
        }

        /// <summary>
        /// NAD83 (NSRS2007).
        /// </summary>
        public static GeographicCoordinateReferenceSystem NAD83NSR2007
        { 
            get 
            {
                return _NAD83NSR2007 ?? (_NAD83NSR2007 = 
                    new GeographicCoordinateReferenceSystem("EPSG::4893", "NAD83 (NSRS2007)",
                                                            "In continental US, replaced NAD83(HARN). Replaced by NAD83(2011) over full domain of validity.",
                                                            new String[] { "NAD 1983 (NSRS2007)", "North American Datum 1983 (NSRS2007)" },
                                                            "Geodetic survey.",
                                                            CoordinateSystems.EllipsoidalLatLonHiDM, 
                                                            GeodeticDatums.NAD83, 
                                                            AreasOfUse.USACONUSAlaskaPRVI));
            }
        }

        /// <summary>
        /// World Geodetic System 1984 (WGS84).
        /// </summary>
        public static GeographicCoordinateReferenceSystem WGS84
        { 
            get 
            {
                return _WGS84 ?? (_WGS84 =
                    new GeographicCoordinateReferenceSystem("EPSG::4979", "WGS84", 
                                                            null,
                                                            new String[] { "WGS 84", "WGS 1984", "World Geodetic System 1984", "World Geodetic System 1984 (WGS84)" },
                                                            "Used by the GPS satellite navigation system.",
                                                            CoordinateSystems.EllipsoidalLatLonHiDM, 
                                                            GeodeticDatums.WGS84, 
                                                            AreasOfUse.WorldByCountry));
            }
        }

        #endregion
    }
}
