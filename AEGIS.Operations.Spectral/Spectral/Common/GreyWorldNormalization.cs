﻿/// <copyright file="GreyWorldNormalization.cs" company="Eötvös Loránd University (ELTE)">
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
/// <author>Dóra Papp</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Numerics;
using ELTE.AEGIS.Operations.Management;
using System;
using System.Collections.Generic;

namespace ELTE.AEGIS.Operations.Spectral.Common
{
    /// <summary>
    /// Represent an operation performing gray world normalization.
    /// </summary>
    /// <remarks>
    /// The grey world normalization is a color normalization technique, which makes the assumption that changes in the lighting spectrum can be 
    /// modeled by constant factors applied to the different bands.
    /// </remarks>
    [OperationMethodImplementation("AEGIS::250285", "Grey world normalization")]
    public class GreyWorldNormalization : SpectralTransformation
    {
        #region Private fields

        /// <summary>
        /// The multiplier for each band which the pixel values of that band have to be multiplied with.
        /// </summary>
        private Double[] _multipliers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GreyWorldNormalization" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="result">The result.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The value of a parameter is not within the expected range.
        /// </exception>
        public GreyWorldNormalization(ISpectralGeometry source, ISpectralGeometry result, IDictionary<OperationParameter, Object> parameters)
            : base(source, result, SpectralOperationMethods.GreyWorldNormalization, parameters)
        {
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Prepares the result of the operation.
        /// </summary>
        /// <returns>The resulting object.</returns>
        protected override ISpectralGeometry PrepareResult()
        {
            _multipliers = new Double[Source.Raster.NumberOfBands];

            for (Int32 bandIndex = 0; bandIndex < Source.Raster.NumberOfBands; bandIndex++)
                ComputeParameters(bandIndex);

            return base.PrepareResult();
        }

        #endregion

        #region Protected SpectralTransformation methods

        /// <summary>
        /// Computes the specified spectral value.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the value.</param>
        /// <param name="columnIndex">The zero-based column index of the value.</param>
        /// <param name="bandIndex">The zero-based band index of the value.</param>
        /// <returns>The spectral value at the specified index.</returns>
        protected override UInt32 Compute(Int32 rowIndex, Int32 columnIndex, Int32 bandIndex)
        {
            if (_multipliers[bandIndex] == 0)
            {
                return RasterAlgorithms.RadiometricResolutionMax(Source.Raster.RadiometricResolution) / 2 + 1;
            }

            return RasterAlgorithms.Restrict(_multipliers[bandIndex] * Source.Raster.GetValue(rowIndex, columnIndex, bandIndex), Source.Raster.RadiometricResolution);
        }
        
        #endregion

        #region Private methods

        /// <summary>
        /// Computes the parameters of the specified band.
        /// </summary>
        /// <param name="bandIndex">The band index.</param>
        private void ComputeParameters(Int32 bandIndex)
        {
            Double assumedAverage = (Calculator.Pow(2, Source.Raster.RadiometricResolution) - 1) / 2;
            Double computedAverage = 0;

            for (Int32 rowIndex = 0; rowIndex < Source.Raster.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < Source.Raster.NumberOfColumns; columnIndex++)
                {
                    computedAverage += Source.Raster.GetValue(rowIndex, columnIndex, bandIndex);
                }
            }

            computedAverage /= Source.Raster.NumberOfRows * Source.Raster.NumberOfColumns;

            if (computedAverage != 0)
            {
                _multipliers[bandIndex] = assumedAverage / computedAverage;
            }
        }

        #endregion        
    }
}