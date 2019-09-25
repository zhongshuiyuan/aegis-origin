namespace ELTE.AEGIS.IO.Lasfile
{
    using System;

    /// <summary>
    /// Represents a LAS point format 8 geometry in Cartesian coordinate space.
    /// </summary>
    public class LasPointFormat8 : LasPointFormat7
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LasPointFormat8" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public LasPointFormat8(Double x, Double y, Double z)
            : base(x, y, z)
        {
            Format = 8;
        }

        /// <summary>
        /// Gets or sets the normalised value of the near infrared color channel associated with the point.
        /// </summary>
        public UInt16 NearInfraredChannel { get; set; }
    }
}
