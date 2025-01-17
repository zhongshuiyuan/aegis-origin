﻿/// <copyright file="WindingNumberAlgorithm.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Máté Cserép</author>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Algorithms
{
    /// <summary>
    /// Represents a type for performing the Winding Number algorithm.
    /// </summary>
    /// <remarks>
    /// The Winding Number algorithm counts the number of times the polygon winds around a coordinate. 
    /// When the coordinate is outside, the value of the winding number is zero; otherwise, the coordinate is inside.
    /// However, the winding number is not defined for coordinates on the boundary of the polygon, it might be both a non-zero or a zero value.
    /// For an input consisting of n line segments, the Winding Number algorithm has a linear complexity of O(2n).
    /// The algorithm assumes that the specified coordinates are valid, ordered, distinct and in the same plane.
    /// </remarks>
    public class WindingNumberAlgorithm
    {
        #region Private fields

        /// <summary>
        /// The polygon shell.
        /// </summary>
        private IList<Coordinate> _shell;

        /// <summary>
        /// The coordinate.
        /// </summary>
        private Coordinate _coordinate;

        /// <summary>
        /// A value indicating whether to verify if the given coordinate is on the shell.
        /// </summary>
        private Boolean _verifyBoundary;

        /// <summary>
        /// A value indicating whether the result is computed.
        /// </summary>
        private Boolean _hasResult;

        /// <summary>
        /// The result of the algorithm.
        /// </summary>
        private Int32 _result;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the precision model.
        /// </summary>
        /// <value>The precision model used for computing the result.</value>
        public PrecisionModel PrecisionModel { get; private set; }

        /// <summary>
        /// Gets or sets the coordinates of the polygon shell.
        /// </summary>
        /// <value>The coordinates of the polygon shell.</value>
        /// <exception cref="System.InvalidOperationException">The value is null.</exception>
        public IList<Coordinate> Shell
        {
            get 
            {
                if (_shell.IsReadOnly)
                    return _shell;
                else
                    return _shell.AsReadOnly(); 
            }
        }

        /// <summary>
        /// Gets or sets the coordinate for which the Winding Number is computed.
        /// </summary>
        /// <value>The coordinate.</value>
        public Coordinate Coordinate
        {
            get { return _coordinate; }
            set
            {
                if (_coordinate != value)
                { 
                    _coordinate = value; 
                    _hasResult = false; 
                }
            }
        }

        /// <summary>
        /// Gets of sets a values indicating whether to verify if the given coordinate is on the shell.
        /// </summary>
        /// <value><c>true</c> to verify whether <see cref="Coordinate" /> is on the <see cref="Shell" />; otherwise <c>false</c>.</value>
        public Boolean VerifyBoundary
        {
            get { return _verifyBoundary; }
            set
            {
                if (_verifyBoundary != value) 
                { 
                    _verifyBoundary = value; 
                    _hasResult = false; 
                }
            }
        }

        /// <summary>
        /// Gets the result of the algorithm.
        /// </summary>
        /// <value>The Winding Number of the specified coordinate.</value>
        public Int32 Result
        {
            get
            {
                if (!_hasResult)
                    Compute();
                return _result;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the given coordinate is on the shell.
        /// </summary>
        /// <remarks>
        /// Requesting the value of this property does not execute the computation of the algorithm.
        /// </remarks>
        /// <value><c>true</c> if the <see cref="Coordinate" /> is <b>certainly</b> on the <see cref="Shell" />, <c>false</c> if <b>certainly</b> not; otherwise <c>null</c>.</value>
        public Boolean? IsOnBoundary { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindingNumberAlgorithm" /> class.
        /// </summary>
        /// <param name="shell">The coordinates of the polygon shell.</param>
        /// <param name="coordinate">The coordinate for which the Winding Number is calculated.</param>
        /// <param name="verifyBoundary">A value indicating whether to verify if the coordinate is on the boundary.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">
        /// The shell must contain at least 3 different coordinates.
        /// or
        /// The first and the last coordinates must be equal.
        /// </exception>
        public WindingNumberAlgorithm(IList<Coordinate> shell, Coordinate coordinate, Boolean verifyBoundary, PrecisionModel precisionModel = null)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");
            if (shell.Count < 4)
                throw new ArgumentException("The shell must contain at least 3 different coordinates.", "shell");

            if (shell[0].Equals(shell[shell.Count - 1]))
            {
                _shell = shell;
            }
            else
            {
                _shell = new List<Coordinate>(shell);
                _shell.Add(shell[0]);
            }

            _coordinate = coordinate;
            _verifyBoundary = verifyBoundary;
            _hasResult = false;
            PrecisionModel = precisionModel ?? PrecisionModel.Default;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Computes the Winding Number.
        /// </summary>
        public void Compute()
        {
            // source: http://geomalgorithms.com/a03-_inclusion.html

            _result = 0;
            IsOnBoundary = null;

            // Loop through all edges of the polygon.
            for (Int32 coordIndex = 0; coordIndex < _shell.Count - 1 || (coordIndex < _shell.Count && _shell[0] != _shell[_shell.Count - 1]); coordIndex++)
            {
                Orientation orientation;

                // An upward crossing.
                if (_shell[coordIndex].Y <= _coordinate.Y && _shell[(coordIndex + 1) % _shell.Count].Y > _coordinate.Y)
                {
                    orientation = Coordinate.Orientation(_coordinate, _shell[coordIndex], _shell[(coordIndex + 1) % _shell.Count], PrecisionModel);
                    switch (orientation)
                    {
                        case Orientation.CounterClockwise:
                            // Has a valid up intersect.
                            ++_result;
                            break;
                        case Orientation.Collinear:
                            // The winding number is not defined for coordinates on the boundary.
                            IsOnBoundary = true;
                            break;
                    }
                }
                // A downward crossing.
                else if (_shell[coordIndex].Y > _coordinate.Y && _shell[(coordIndex + 1) % _shell.Count].Y <= _coordinate.Y)
                {
                    orientation = Coordinate.Orientation(_coordinate, _shell[coordIndex], _shell[(coordIndex + 1) % _shell.Count], PrecisionModel);
                    switch (orientation)
                    {
                        case Orientation.Clockwise:
                            // Has a valid down intersect.
                            --_result;
                            break;
                        case Orientation.Collinear:
                            // The winding number is not defined for coordinates on the boundary.
                            IsOnBoundary = true;
                            break;
                    }
                }
            }

            if (!IsOnBoundary.HasValue && _verifyBoundary)
                CheckBoundary();

            _hasResult = true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Checks whether the given coordinate is on the shell.
        /// </summary>
        private void CheckBoundary()
        {
            IsOnBoundary = false;

            for (Int32 coordIndex = 0; coordIndex < _shell.Count - 1 || (coordIndex < _shell.Count && _shell[0] != _shell[_shell.Count - 1]); coordIndex++)
                if (LineAlgorithms.Contains(_shell[coordIndex], _shell[(coordIndex + 1) % _shell.Count], _coordinate, PrecisionModel))
                {
                    IsOnBoundary = true;
                    break;
                }
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="source">The polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon.
        /// </remarks>
        public static Boolean IsInsidePolygon(IBasicPolygon source, Coordinate coordinate)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (source.Shell == null || source.Shell.Coordinates == null)
                return false;

            return IsInsidePolygon(source.Shell, source.Holes, coordinate);
        }

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="source">The polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The source is null.</exception>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon.
        /// </remarks>
        public static Boolean IsInsidePolygon(IBasicPolygon source, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (source == null)
                throw new ArgumentNullException("source", "The source is null.");
            if (source.Shell == null || source.Shell.Coordinates == null)
                return false;

            return IsInsidePolygon(source.Shell, source.Holes, coordinate, precisionModel);
        }

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon.
        /// </remarks>
        public static Boolean IsInsidePolygon(IBasicLineString shell, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return IsInsidePolygon(shell.Coordinates, null, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon.
        /// </remarks>
        public static Boolean IsInsidePolygon(IBasicLineString shell, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return IsInsidePolygon(shell.Coordinates, null, coordinate, precisionModel);
        }

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon.
        /// </remarks>
        public static Boolean IsInsidePolygon(IList<Coordinate> shell, Coordinate coordinate)
        {
            return IsInsidePolygon(shell, null, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate"/>; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon.
        /// </remarks>
        public static Boolean IsInsidePolygon(IList<Coordinate> shell, Coordinate coordinate, PrecisionModel precisionModel)
        {
            return IsInsidePolygon(shell, null, coordinate, precisionModel);
        }

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate" />; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">A hole is null.</exception>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon or the holes.
        /// </remarks>
        public static Boolean IsInsidePolygon(IBasicLineString shell, IEnumerable<IBasicLineString> holes, Coordinate coordinate)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return IsInsidePolygon(shell.Coordinates, holes != null ? holes.Select(hole => hole.Coordinates) : null, coordinate, PrecisionModel.Default);
        }
        
        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate" />; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">The shell is null.</exception>
        /// <exception cref="System.ArgumentException">A hole is null.</exception>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon or the holes.
        /// </remarks>
        public static Boolean IsInsidePolygon(IBasicLineString shell, IEnumerable<IBasicLineString> holes, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            return IsInsidePolygon(shell.Coordinates, holes != null ? holes.Select(hole => hole.Coordinates) : null, coordinate, precisionModel);
        }
        
        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate"/>; otherwise <c>false</c>.</returns>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon or the holes.
        /// </remarks>
        public static Boolean IsInsidePolygon(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, Coordinate coordinate)
        {
            return IsInsidePolygon(shell, holes, coordinate, PrecisionModel.Default);
        }

        /// <summary>
        /// Determines whether a coordinate is inside a polygon.
        /// </summary>
        /// <param name="shell">The shell of the polygon.</param>
        /// <param name="holes">The holes of the polygon.</param>
        /// <param name="coordinate">The coordinate.</param>
        /// <param name="precisionModel">The precision model.</param>
        /// <returns><c>true</c> if the polygon contains <paramref name="coordinate"/>; otherwise <c>false</c>.</returns>
        /// <remarks>
        /// This method might also result <c>true</c> for coordinates on the boundary of the polygon or the holes.
        /// </remarks>
        public static Boolean IsInsidePolygon(IList<Coordinate> shell, IEnumerable<IList<Coordinate>> holes, Coordinate coordinate, PrecisionModel precisionModel)
        {
            if (shell == null)
                throw new ArgumentNullException("shell", "The shell is null.");

            if (new WindingNumberAlgorithm(shell, coordinate, false, precisionModel).Result == 0)
                return false;

            if (holes != null)
            {
                foreach (IList<Coordinate> hole in holes)
                {
                    if (hole == null)
                        throw new ArgumentException("A hole is null.", "hole");

                    if (new WindingNumberAlgorithm(hole, coordinate, false, precisionModel).Result != 0)
                        return false;
                }
            }

            return true;
        }

        #endregion
    }
}
