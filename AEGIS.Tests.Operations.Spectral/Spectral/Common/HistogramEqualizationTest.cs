﻿/// <copyright file="HistogramEqualizationTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://www.osedu.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spectral;
using ELTE.AEGIS.Operations.Spectral.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Common
{
    /// <summary>
    /// Test fixture for <see cref="HistogramEqualization"/> class.
    /// </summary>
    [TestFixture]
    public class HistogramEqualizationTest
    {
        /// <summary>
        /// The raster (mocked).
        /// </summary>
        private Mock<IRaster> _rasterMock;

        /// <summary>
        /// The histogram values for earch band of the raster.
        /// </summary>
        private Int32[][] _histogramValues;

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _rasterMock = new Mock<IRaster>(MockBehavior.Strict);
            _rasterMock.Setup(raster => raster.Factory).Returns(Factory.DefaultInstance<IRasterFactory>());
            _rasterMock.Setup(raster => raster.IsReadable).Returns(true);
            _rasterMock.Setup(raster => raster.NumberOfRows).Returns(20);
            _rasterMock.Setup(raster => raster.NumberOfColumns).Returns(15);
            _rasterMock.Setup(raster => raster.SpectralResolution).Returns(3);
            _rasterMock.Setup(raster => raster.RadiometricResolutions).Returns(new Int32[] { 8, 8, 8 });
            _rasterMock.Setup(raster => raster.Coordinates).Returns(Enumerable.Repeat(Coordinate.Empty, 4).ToArray());
            _rasterMock.Setup(raster => raster.SpectralRanges).Returns(SpectralRanges.RGB);
            _rasterMock.Setup(raster => raster.Mapper).Returns<RasterMapper>(null);
            _rasterMock.Setup(raster => raster.Representation).Returns(RasterRepresentation.Integer);
            _rasterMock.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)(rowIndex * columnIndex * bandIndex % 256)));
            _rasterMock.Setup(raster => raster.HistogramValues).Returns(ComputeHistogram(_rasterMock.Object));
        }

        /// <summary>
        /// Test for execution on each band.
        /// </summary>
        [TestCase]
        public void HistogramEqualizationExecuteTest()
        {
            HistogramEqualization operation = new HistogramEqualization(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_rasterMock.Object), null);
            operation.Execute();

            Assert.AreEqual(_rasterMock.Object.NumberOfRows, operation.Result.Raster.NumberOfRows);
            Assert.AreEqual(_rasterMock.Object.NumberOfColumns, operation.Result.Raster.NumberOfColumns);
            Assert.AreEqual(_rasterMock.Object.SpectralResolution, operation.Result.Raster.SpectralResolution);
            Assert.IsTrue(_rasterMock.Object.RadiometricResolutions.SequenceEqual(operation.Result.Raster.RadiometricResolutions));
            Assert.AreEqual(_rasterMock.Object.Representation, operation.Result.Raster.Representation);

            for (Int32 bandIndex = 0; bandIndex < operation.Result.Raster.SpectralResolution; bandIndex++)
            {
                AssertResultForBand(_rasterMock.Object, bandIndex, operation.Result.Raster, bandIndex);
            }
        }

        /// <summary>
        /// Test for execution on a single band.
        /// </summary>
        [TestCase]
        public void HistogramEqualizationExecuteForBandTest()
        {
            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(SpectralOperationParameters.BandIndex, 1);

            HistogramEqualization operation = new HistogramEqualization(Factory.DefaultInstance<IGeometryFactory>().CreateSpectralPolygon(_rasterMock.Object), parameters);
            operation.Execute();

            Assert.AreEqual(_rasterMock.Object.NumberOfRows, operation.Result.Raster.NumberOfRows);
            Assert.AreEqual(_rasterMock.Object.NumberOfColumns, operation.Result.Raster.NumberOfColumns);
            Assert.AreEqual(1, operation.Result.Raster.SpectralResolution);
            Assert.AreEqual(_rasterMock.Object.RadiometricResolutions[0], operation.Result.Raster.RadiometricResolutions[0]);
            Assert.AreEqual(_rasterMock.Object.Representation, operation.Result.Raster.Representation);

            Assert.AreEqual(_rasterMock.Object.HistogramValues[0].Sum(), operation.Result.Raster.HistogramValues[0].Sum());

            AssertResultForBand(_rasterMock.Object, 1, operation.Result.Raster, 0);
        }

        /// <summary>
        /// Asserts the results for the specified band.
        /// </summary>
        /// <param name="source">The source raster.</param>
        /// <param name="sourceBandIndex">The band index of the source raster.</param>
        /// <param name="result">The resulting raster.</param>
        /// <param name="resultBandIndex">The band index of the resulting raster.</param>
        private void AssertResultForBand(IRaster source, Int32 sourceBandIndex, IRaster result, Int32 resultBandIndex)
        {
            Assert.AreEqual(source.HistogramValues[sourceBandIndex].Sum(), result.HistogramValues[resultBandIndex].Sum());

            Assert.Greater(result.HistogramValues[resultBandIndex][0], 0);

            for (Int32 rowIndex = 0; rowIndex < result.NumberOfRows; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < result.NumberOfColumns; columnIndex++)
                {
                    Assert.GreaterOrEqual(result.GetValue(rowIndex, columnIndex, resultBandIndex), 0);
                    Assert.LessOrEqual(result.GetValue(rowIndex, columnIndex, resultBandIndex), RasterAlgorithms.RadiometricResolutionMax(source.RadiometricResolutions[sourceBandIndex]));
                }
        }


        /// <summary>
        /// Computes the histogram of the specified raster.
        /// </summary>
        /// <param name="raster">The raster.</param>
        /// <returns>The histogram of the raster.</returns>
        private Int32[][] ComputeHistogram(IRaster raster)
        {
            if (_histogramValues == null)
            {
                _histogramValues = new Int32[raster.SpectralResolution][];

                for (Int32 bandIndex = 0; bandIndex < raster.SpectralResolution; bandIndex++)
                {
                    _histogramValues[bandIndex] = new Int32[1UL << raster.RadiometricResolutions[bandIndex]];

                    for (Int32 rowIndex = 0; rowIndex < raster.NumberOfRows; rowIndex++)
                        for (Int32 columnIndex = 0; columnIndex < raster.NumberOfColumns; columnIndex++)
                        {
                            _histogramValues[bandIndex][raster.GetValue(rowIndex, columnIndex, bandIndex)]++;
                        }
                }
            }

            return _histogramValues;
        }
    }
}
