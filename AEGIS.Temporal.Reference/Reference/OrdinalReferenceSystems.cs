﻿/// <copyright file="OrdinalReferenceSystems.cs" company="Eötvös Loránd University (ELTE)">
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

namespace ELTE.AEGIS.Temporal.Reference
{
    /// <summary>
    /// Represents a collection of known <see cref="OrdinalReferenceSystem" /> instances.
    /// </summary>
    [IdentifiedObjectCollection(typeof(OrdinalReferenceSystem))]
    public static class OrdinalReferenceSystems
    {
        #region Query fields

        private static OrdinalReferenceSystem[] _all;

        #endregion

        #region Query properties

        /// <summary>
        /// Gets all <see cref="OrdinalReferenceSystem" /> instances within the collection.
        /// </summary>
        /// <value>A read-only list containing all <see cref="OrdinalReferenceSystem" /> instances within the collection.</value>
        public static IList<OrdinalReferenceSystem> All
        {
            get
            {
                if (_all == null)
                    _all = typeof(OrdinalReferenceSystems).GetProperties().
                                                           Where(property => property.Name != "All").
                                                           Select(property => property.GetValue(null, null) as OrdinalReferenceSystem).
                                                           ToArray();
                return Array.AsReadOnly(_all);
            }
        }

        #endregion

        #region Query methods

        /// <summary>
        /// Returns all <see cref="OrdinalReferenceSystem" /> instances matching a specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A list containing the <see cref="OrdinalReferenceSystem" /> instances that match the specified identifier.</returns>
        public static IList<OrdinalReferenceSystem> FromIdentifier(String identifier)
        {
            if (identifier == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Identifier, identifier)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns all <see cref="OrdinalReferenceSystem" /> instances matching a specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>A list containing the <see cref="OrdinalReferenceSystem" /> instances that match the specified name.</returns>
        public static IList<OrdinalReferenceSystem> FromName(String name)
        {
            if (name == null)
                return null;

            return All.Where(obj => System.Text.RegularExpressions.Regex.IsMatch(obj.Name, name)).ToList().AsReadOnly();
        }

        #endregion

        #region Private static fields

        private static OrdinalReferenceSystem _geologicTimeScale;

        #endregion

        #region Public static properties

        /// <summary>
        /// Geologic time scale.
        /// </summary>
        public static OrdinalReferenceSystem GeologicTimeScale
        {
            get 
            {
                if (_geologicTimeScale == null)
                {
                    _geologicTimeScale = new OrdinalReferenceSystem("AEGIS::859901", "Geologic time scale");
                    _geologicTimeScale.AddEra("AEGIS::859101", "Paleozoic era", Calendars.GregorianCalendar.GetDate(-541000000, 1), Calendars.GregorianCalendar.GetDate(-252200000, 1));
                    _geologicTimeScale.AddEra("AEGIS::859102", "Mesozoic era", Calendars.GregorianCalendar.GetDate(-252200000, 1), Calendars.GregorianCalendar.GetDate(-66000000, 1));
                    _geologicTimeScale.AddEra("AEGIS::859103", "Cenozoic era", Calendars.GregorianCalendar.GetDate(-66000000, 1), Calendars.GregorianCalendar.EndOfUse);
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859110", "Cambrian period", Calendars.GregorianCalendar.GetDate(-541000000, 1), Calendars.GregorianCalendar.GetDate(-485400000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859111", "Ordovician period", Calendars.GregorianCalendar.GetDate(-485400000, 1), Calendars.GregorianCalendar.GetDate(-443400000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859112", "Silurian period", Calendars.GregorianCalendar.GetDate(-443400000, 1), Calendars.GregorianCalendar.GetDate(-419200000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859113", "Devonian period", Calendars.GregorianCalendar.GetDate(-419200000, 1), Calendars.GregorianCalendar.GetDate(-358900000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859114", "Carboniferous period", Calendars.GregorianCalendar.GetDate(-358900000, 1), Calendars.GregorianCalendar.GetDate(-298900000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859115", "Permian period", Calendars.GregorianCalendar.GetDate(-298900000, 1), Calendars.GregorianCalendar.GetDate(-252200000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859116", "Triassic period", Calendars.GregorianCalendar.GetDate(-252200000, 1), Calendars.GregorianCalendar.GetDate(-201300000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859117", "Jurassic period", Calendars.GregorianCalendar.GetDate(-201300000, 1), Calendars.GregorianCalendar.GetDate(-152100000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859118", "Cretaceous period", Calendars.GregorianCalendar.GetDate(-152100000, 1), Calendars.GregorianCalendar.GetDate(-145000000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859119", "Paleogene period", Calendars.GregorianCalendar.GetDate(-145000000, 1), Calendars.GregorianCalendar.GetDate(-23030000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859120", "Neogene period", Calendars.GregorianCalendar.GetDate(-23030000, 1), Calendars.GregorianCalendar.GetDate(-2588000, 1));
                    _geologicTimeScale.Eras[0].AddEra("AEGIS::859121", "Quaternary period", Calendars.GregorianCalendar.GetDate(-2588000, 1), Calendars.GregorianCalendar.EndOfUse);
                    _geologicTimeScale.Eras[0].Eras[11].AddEra("AEGIS::859162", "Pleistocene epoch", Calendars.GregorianCalendar.GetDate(-2588000, 1), Calendars.GregorianCalendar.GetDate(-11700, 1));
                    _geologicTimeScale.Eras[0].Eras[11].AddEra("AEGIS::859163", "Holocene epoch", Calendars.GregorianCalendar.GetDate(-11700, 1), Calendars.GregorianCalendar.EndOfUse);
                }

                return _geologicTimeScale;
            }
        }

        #endregion
    }
}
