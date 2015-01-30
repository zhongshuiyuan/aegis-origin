﻿/// <copyright file="ReferenceTransformationTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Operations;
using ELTE.AEGIS.Operations.Spatial;
using ELTE.AEGIS.Reference;
using ELTE.AEGIS.Reference.Operations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spatial
{
    /// <summary>
    /// Test fixture for the <see cref="ReferenceTransformation"/> class.
    /// </summary>
    [TestFixture]
    public class ReferenceTransformationTest
    {
        #region Test methods

        /// <summary>
        /// Test for operation execution with metadata.
        /// </summary>
        [TestCase]
        public void ReferenceTransformationExecuteTest()
        {
            // test case 1: with metadata

            TestExecuteForReferenceSystems(true);
            TestExecuteForGeometries(true);


            // test case 2: without metadata

            TestExecuteForReferenceSystems(false);
            TestExecuteForGeometries(false);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Test execution for multiple reference systems by using points.
        /// </summary>
        /// <param name="metadataPreservation">Indicates whether the metadata should be preserved.</param>
        private void TestExecuteForReferenceSystems(Boolean metadataPreservation)
        {
            // test case 1: projected to projected

            Coordinate sourceCoordinate = new Coordinate(10, 10);
            GeoCoordinate sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Reverse(sourceCoordinate);
            GeoCoordinate expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);
            Coordinate expectedCoordinate = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Forward(expectedGeoCoordinate);

            IPoint sourcePoint = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreatePoint(10, 10);

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);
            parameters.Add(CommonOperationParameters.MetadataPreservation, metadataPreservation);

            ReferenceTransformation transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            IPoint resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(expectedCoordinate, resultPoint.Coordinate);
            Assert.AreEqual(sourcePoint.Metadata, resultPoint.Metadata);


            // test case 2: geographic to projected

            sourceGeoCoordinate = new GeoCoordinate(Angle.FromDegree(45), Angle.FromDegree(45));
            expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);
            expectedCoordinate = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Forward(expectedGeoCoordinate);

            sourcePoint = Factory.GetInstance<IGeometryFactory>(Geographic2DCoordinateReferenceSystems.HD72).CreatePoint(45, 45);

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);

            transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(expectedCoordinate, resultPoint.Coordinate);


            // test case 3: projected to geographic

            sourceCoordinate = new Coordinate(10, 10);
            sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Reverse(sourceCoordinate);
            expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);

            sourcePoint = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreatePoint(sourceCoordinate);

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, Geographic2DCoordinateReferenceSystems.WGS84);

            transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(expectedGeoCoordinate.Latitude.GetValue(UnitsOfMeasurement.Degree), resultPoint.Coordinate.X);
            Assert.AreEqual(expectedGeoCoordinate.Longitude.GetValue(UnitsOfMeasurement.Degree), resultPoint.Coordinate.Y);


            // test case 4: projected to projected (reverse)

            sourceCoordinate = new Coordinate(10, 10);
            sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Reverse(sourceCoordinate);
            expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Reverse(sourceGeoCoordinate);
            expectedCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Forward(expectedGeoCoordinate);

            sourcePoint = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.WGS84_WorldMercator).CreatePoint(10, 10);

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.HD72_EOV);

            transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(expectedCoordinate, resultPoint.Coordinate);


            // test case 5: same reference system

            sourcePoint = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.WGS84_WorldMercator).CreatePoint(10, 10);

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);

            transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(sourcePoint.Coordinate, resultPoint.Coordinate);


            // test case 6: no source reference system

            sourcePoint = Factory.DefaultInstance<IGeometryFactory>().CreatePoint(10, 10);

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);

            transformation = new ReferenceTransformation(sourcePoint, parameters);
            transformation.Execute();

            resultPoint = transformation.Result as IPoint;

            Assert.AreEqual(sourcePoint.Coordinate, resultPoint.Coordinate);


            // test case 7: no target reference system

            parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, null);

            Assert.Throws<ArgumentException>(() => transformation = new ReferenceTransformation(sourcePoint, parameters));
        }

        /// <summary>
        /// Test execution for multiple geometry types.
        /// </summary>
        /// <param name="metadataPreservation">Indicates whether the metadata should be preserved.</param>
        private void TestExecuteForGeometries(Boolean metadataPreservation)
        {
            Coordinate[] sourceCoordinates = new Randomizer(0).GetDoubles(0, 10000, 100).Select(value => new Coordinate(value, value)).ToArray();
            Coordinate[] expectedCoordinates = new Coordinate[sourceCoordinates.Length];

            for (Int32 i = 0; i < sourceCoordinates.Length; i++)
            {
                GeoCoordinate sourceGeoCoordinate = ProjectedCoordinateReferenceSystems.HD72_EOV.Projection.Reverse(sourceCoordinates[i]);
                GeoCoordinate expectedGeoCoordinate = GeographicTransformations.HD72_WGS84_V1.Forward(sourceGeoCoordinate);
                expectedCoordinates[i] = ProjectedCoordinateReferenceSystems.WGS84_WorldMercator.Projection.Forward(expectedGeoCoordinate);
            }


            // test case 1: lines

            ILine sourceLine = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateLine(sourceCoordinates[0], sourceCoordinates[1]);

            IDictionary<OperationParameter, Object> parameters = new Dictionary<OperationParameter, Object>();
            parameters.Add(ReferenceOperationParameters.TargetReferenceSystem, ProjectedCoordinateReferenceSystems.WGS84_WorldMercator);
            parameters.Add(CommonOperationParameters.MetadataPreservation, metadataPreservation);

            ReferenceTransformation transformation = new ReferenceTransformation(sourceLine, parameters);
            transformation.Execute();

            Assert.IsInstanceOf<ILine>(transformation.Result);
            Assert.AreEqual(sourceLine.Metadata, transformation.Result.Metadata);

            ILine resultLine = transformation.Result as ILine;

            for (Int32 i = 0; i < resultLine.Count; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], resultLine.GetCoordinate(i));
            }


            // test case 2: line strings

            ILineString sourceLineString = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateLineString(sourceCoordinates);

            transformation = new ReferenceTransformation(sourceLineString, parameters);
            transformation.Execute();

            Assert.IsInstanceOf<ILineString>(transformation.Result);
            Assert.AreEqual(sourceLineString.Metadata, transformation.Result.Metadata);

            ILineString resultLineString = transformation.Result as ILineString;

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], resultLineString.GetCoordinate(i));
            }


            // test case 3: linear rings

            ILinearRing sourceLinearRing = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateLinearRing(sourceCoordinates);

            transformation = new ReferenceTransformation(sourceLinearRing, parameters);
            transformation.Execute();

            Assert.IsInstanceOf<ILinearRing>(transformation.Result);
            Assert.AreEqual(sourceLinearRing.Metadata, transformation.Result.Metadata);

            ILinearRing resultLinearRing = transformation.Result as ILinearRing;

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], resultLinearRing.GetCoordinate(i));
            }


            // test case 4: polygon without hole

            IPolygon sourcePolygon = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreatePolygon(sourceCoordinates);

            transformation = new ReferenceTransformation(sourcePolygon, parameters);
            transformation.Execute();

            Assert.IsInstanceOf<IPolygon>(transformation.Result);
            Assert.AreEqual(sourcePolygon.Metadata, transformation.Result.Metadata);

            IPolygon resultPolygon = transformation.Result as IPolygon;

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], resultPolygon.Shell.GetCoordinate(i));
            }


            // test case 5: polygon with hole

            sourcePolygon = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreatePolygon(sourceCoordinates, new Coordinate[][] { sourceCoordinates });

            transformation = new ReferenceTransformation(sourcePolygon, parameters);
            transformation.Execute();

            Assert.IsInstanceOf<IPolygon>(transformation.Result);
            Assert.AreEqual(sourcePolygon.Metadata, transformation.Result.Metadata);

            resultPolygon = transformation.Result as IPolygon;

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], resultPolygon.Shell.GetCoordinate(i));
            }

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], resultPolygon.Holes[0].GetCoordinate(i));
            }


            // test case 5: multi point

            IMultiPoint sourceMultiPoint = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateMultiPoint(sourceCoordinates);

            transformation = new ReferenceTransformation(sourceMultiPoint, parameters);
            transformation.Execute();

            Assert.IsInstanceOf<IMultiPoint>(transformation.Result);
            Assert.AreEqual(sourceMultiPoint.Metadata, transformation.Result.Metadata);

            IMultiPoint resultMultiPoint = transformation.Result as IMultiPoint;

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], resultMultiPoint[i].Coordinate);
            }


            // test case 6: multi linestring

            IMultiLineString sourceMultiLineString = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateMultiLineString(Enumerable.Repeat(sourceLineString, 5));

            transformation = new ReferenceTransformation(sourceMultiLineString, parameters);
            transformation.Execute();

            Assert.IsInstanceOf<IMultiLineString>(transformation.Result);
            Assert.AreEqual(sourceMultiLineString.Metadata, transformation.Result.Metadata);

            IMultiLineString resultMultiLineString = transformation.Result as IMultiLineString;

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], resultMultiLineString[0].GetCoordinate(i));
            }


            // test case 7: multi polygon

            IMultiPolygon sourceMultiPolygon = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateMultiPolygon(Enumerable.Repeat(sourcePolygon, 5));

            transformation = new ReferenceTransformation(sourceMultiPolygon, parameters);
            transformation.Execute();

            Assert.IsInstanceOf<IMultiPolygon>(transformation.Result);
            Assert.AreEqual(sourceMultiPolygon.Metadata, transformation.Result.Metadata);

            IMultiPolygon resultMultiPolygon = transformation.Result as IMultiPolygon;

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], resultMultiLineString[0].GetCoordinate(i));
            }


            // test case 8: geometry collection

            IGeometryCollection<IGeometry> sourceCollection = Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV).CreateGeometryCollection(new IGeometry[] { sourceLine, sourceLineString, sourceLinearRing, sourcePolygon });

            transformation = new ReferenceTransformation(sourceCollection, parameters);
            transformation.Execute();

            Assert.IsInstanceOf<IGeometryCollection<IGeometry>>(transformation.Result);
            Assert.AreEqual(sourceCollection.Metadata, transformation.Result.Metadata);

            IGeometryCollection<IGeometry> resultCollection = transformation.Result as IGeometryCollection<IGeometry>;

            for (Int32 i = 0; i < (resultCollection[0] as ILine).Count; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], (resultCollection[0] as ILine).GetCoordinate(i));
            }

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], (resultCollection[1] as ILineString).GetCoordinate(i));
            }

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], (resultCollection[2] as ILinearRing).GetCoordinate(i));
            }

            for (Int32 i = 0; i < expectedCoordinates.Length; i++)
            {
                Assert.AreEqual(expectedCoordinates[i], (resultCollection[3] as IPolygon).Shell.GetCoordinate(i));
            }


            // test case 9: not supported geometry

            Mock<IGeometry> geometryMock = new Mock<IGeometry>(MockBehavior.Loose);
            geometryMock.Setup(geometry => geometry.Factory).Returns(() => Factory.GetInstance<IGeometryFactory>(ProjectedCoordinateReferenceSystems.HD72_EOV));
            geometryMock.Setup(geometry => geometry.ReferenceSystem).Returns(() => ProjectedCoordinateReferenceSystems.HD72_EOV);

            transformation = new ReferenceTransformation(geometryMock.Object, parameters);
            Assert.Throws<InvalidOperationException>(() => transformation.Execute());
        }

        #endregion
    }
}
