﻿/// <copyright file="CoordinateComparer.cs" company="Eötvös Loránd University (ELTE)">
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

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    /// <summary>
    /// Represents a comparer for <see cref="Coordinate" /> instances.
    /// </summary>
    public class CoordinateComparer : IComparer<Coordinate>
    {
        #region IComparer methods

        /// <summary>
        /// Compares two <see cref="Coordinate" /> instances and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="first">The first <see cref="Coordinate" /> to compare.</param>
        /// <param name="second">The second <see cref="Coordinate" /> to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="first" /> and <paramref name="second" />.</returns>
        public Int32 Compare(Coordinate first, Coordinate second)
        {
            if (first.X < second.X)
                return -1;
            if (first.X > second.X)
                return 1;
            if (first.Y < second.Y)
                return -1;
            return first.Y > second.Y ? 1 : 0;
        }

        #endregion
    }
}
