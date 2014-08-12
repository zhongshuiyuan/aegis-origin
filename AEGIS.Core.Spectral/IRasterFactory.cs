﻿/// <copyright file="IRasterFactory.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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
    /// Defines behavior for factories producing <see cref="IRaster" /> instances.
    /// </summary>
    public interface IRasterFactory : IFactory
    {
        #region Factory methods for rasters

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        IRaster CreateRaster(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        IRaster CreateRaster(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        IRaster CreateRaster(Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// </exception>
        IRaster CreateRaster(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolution">The radiometric resolution.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The radiometric resolution is less than 1.
        /// or
        /// The radiometric resolution is greater than the maximum allowed.
        /// </exception>
        IRaster CreateRaster(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, Int32 radiometricResolution, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="representation">The representation of the raster.</param>
        /// <param name="numberOfBands">The number of bands.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <param name="radiometricResolutions">The list of radiometric resolutions.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>The produced raster image.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The number of bands is less than 1.
        /// or
        /// The number of rows is less than 0.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// One or more radiometric resolutions are less than 1.
        /// or
        /// One or more radiometric resolutions are greater than the maximum allowed.
        /// </exception>
        /// <exception cref="System.ArgumentException">The number of radiometric resolutions does not match the number of bands.</exception>
        IRaster CreateRaster(RasterRepresentation representation, Int32 numberOfBands, Int32 numberOfRows, Int32 numberOfColumns, IList<Int32> radiometricResolutions, RasterMapper mapper);

        /// <summary>
        /// Creates a raster image.
        /// </summary>
        /// <param name="other">The other raster image.</param>
        /// <returns>The produced raster image matching <paramref name="other"/>.</returns>
        /// <exception cref="System.ArgumentNullException">The other raster is null.</exception>
        IRaster CreateRaster(IRaster other);

        #endregion

        #region Factory methods for raster services

        /// <summary>
        /// Creates a raster image for the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="mapper">The raster mapper.</param>
        /// <returns>The produced raster image.</returns>
        /// <exception cref="System.ArgumentNullException">The raster service is null.</exception>
        IRaster CreateRaster(IRasterService service, RasterMapper mapper);

        #endregion

        #region Factory methods for raster masks

        /// <summary>
        /// Creates a mask on a raster.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <param name="rowIndex">The starting row index.</param>
        /// <param name="columnIndex">The starting column index.</param>
        /// <param name="numberOfRows">The number of rows.</param>
        /// <param name="numberOfColumns">The number of columns.</param>
        /// <returns>The produced masked raster.</returns>
        /// <exception cref="System.ArgumentNullException">The raster is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The starting row index is less than 0.
        /// or
        /// The starting row index is equal to or greater than the number of rows in the source raster.
        /// or
        /// The starting column index is less than 0.
        /// or
        /// The starting column index is equal to or greater than the number of columns in the source raster.
        /// or
        /// The number of rows is less than 0.
        /// or  
        /// The starting row index and the number of rows is greater than the number of rows in the source raster.
        /// or
        /// The number of columns is less than 0.
        /// or
        /// The starting columns index and the number of columns is greater than the number of rows in the source raster.
        /// </exception>
        IRaster CreateMask(IRaster raster, Int32 rowIndex, Int32 columnIndex, Int32 numberOfRows, Int32 numberOfColumns);

        #endregion
    }
}
