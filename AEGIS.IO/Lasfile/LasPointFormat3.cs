namespace ELTE.AEGIS.IO.Lasfile
{
    using System;

    /// <summary>
    /// Represents a LAS point format 3 geometry in Cartesian coordinate space.
    /// </summary>
    public class LasPointFormat3 : LasPointFormat2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LasPointFormat3" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public LasPointFormat3(Double x, Double y, Double z)
            : base(x, y, z)
        {
            Format = 3;
        }

        /// <summary>
        /// Gets or sets the GPS time at which the point was acquired.
        /// </summary>
        public Double GPSTime { get; set; }
    }
}
