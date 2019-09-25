namespace ELTE.AEGIS.IO.Lasfile
{
    using System;

    /// <summary>
    /// Represents a LAS point format 0 geometry in Cartesian coordinate space.
    /// </summary>
    public class LasPointFormat0 : LasPointBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LasPointFormat0" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public LasPointFormat0(Double x, Double y, Double z)
            : base(x, y, z)
        {
            Format = 0;
        }

        /// <summary>
        /// Gets or sets the rotational position of the emitted laser pulse.
        /// </summary>
        public SByte ScanAngle { get; set; }
    }
}
