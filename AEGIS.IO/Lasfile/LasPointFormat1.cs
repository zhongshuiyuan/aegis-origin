namespace ELTE.AEGIS.IO.Lasfile
{
    using System;

    /// <summary>
    /// Represents a LAS point format 1 geometry in Cartesian coordinate space.
    /// </summary>
    public class LasPointFormat1 : LasPointFormat0
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LasPointFormat1" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public LasPointFormat1(Double x, Double y, Double z)
            : base(x, y, z)
        {
            Format = 1;
        }

        /// <summary>
        /// Gets or sets the GPS time at which the point was acquired.
        /// </summary>
        public Double GPSTime { get; set; }
    }
}
