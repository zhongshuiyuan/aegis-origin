namespace ELTE.AEGIS.IO.Lasfile
{
    using System;

    /// <summary>
    /// Represents a LAS point format 6 geometry in Cartesian coordinate space.
    /// </summary>
    public class LasPointFormat6 : LasPointBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LasPointFormat6" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public LasPointFormat6(Double x, Double y, Double z)
            : base(x, y, z)
        {
            Format = 6;
        }

        /// <summary>
        /// Gets or sets a flag indicating whether the point is within the overlap region of more swaths or takes.
        /// </summary>
        public Boolean IsOverlap { get; set; }

        /// <summary>
        /// Gets or sets the number of scanner channels.
        /// </summary>
        public Byte ScannerChannel { get; set; }

        /// <summary>
        /// Gets or sets the rotational position of the emitted laser pulse.
        /// </summary>
        public Int16 ScanAngle { get; set; }

        /// <summary>
        /// Gets or sets the GPS time at which the point was acquired.
        /// </summary>
        public Double GPSTime { get; set; }
    }
}
