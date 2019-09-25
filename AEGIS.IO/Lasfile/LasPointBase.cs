﻿namespace ELTE.AEGIS.IO.Lasfile
{
    using System;
    using AEGIS;
    using AEGIS.Geometry;

    /// <summary>
    /// Represents the base data of a LAS point geometry in Cartesian coordinate space.
    /// </summary>
    public abstract class LasPointBase : Point, ILasPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LasPointBase" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        protected LasPointBase(Double x, Double y, Double z)
            : base(x, y, z, null, null) { }

        /// <summary>
        /// Gets or sets the key of the file that contains the point.
        /// </summary>
        public UInt16 PointSourceId { get; set; }

        /// <summary>
        /// Gets or sets the user data for the point. 
        /// </summary>
        public Byte UserData { get; set; }

        /// <summary>
        /// Gets the point data record format associated with the point.
        /// </summary>
        public Byte Format { get; protected set; }

        /// <summary>
        /// Gets or sets the standard ASPRS classification of the point.
        /// </summary>
        public Byte Classification { get; set; }

        /// <summary>
        /// Gets or sets the normalised integer representation of the pulse return magnitude.
        /// </summary>
        public UInt16 Intensity { get; set; }

        /// <summary>
        /// Gets or sets the return number for the given output pulse.
        /// </summary>
        public Byte ReturnNumber { get; set; }

        /// <summary>
        /// Gets or sets the total number of returns for the given output pulse.
        /// </summary>
        public Byte TotalReturnNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scan direction is positive.
        /// </summary>
        public Boolean IsScanDirectionPositive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scan direction is negative.
        /// </summary>
        public Boolean IsScanDirectionNegative { get => !IsScanDirectionPositive; set => IsScanDirectionPositive = !value; }

        /// <summary>
        /// Gets or sets a value indicating whether the point is the last point of a given scan line.
        /// </summary>
        public Boolean IsFlightLineEdge { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the point was created by a technique other than LiDAR collection.
        /// </summary>
        public Boolean IsSynthetic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the point is considered to be a model key-point.
        /// </summary>
        public Boolean IsKeyPoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the point should not be included in processing.
        /// </summary>
        public Boolean IsWithheld { get; set; }
    }
}
