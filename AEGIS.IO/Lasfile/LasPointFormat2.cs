namespace ELTE.AEGIS.IO.Lasfile
{
    using System;

    /// <summary>
    /// Represents a LAS point format 2 geometry in Cartesian coordinate space.
    /// </summary>
    public class LasPointFormat2 : LasPointFormat0
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LasPointFormat2" /> class.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        public LasPointFormat2(Double x, Double y, Double z)
            : base(x, y, z)
        {
            Format = 2;
        }

        /// <summary>
        /// Gets or sets the normalised value of the red color channel associated with the point.
        /// </summary>
        public UInt16 RedChannel { get; set; }

        /// <summary>
        /// Gets or sets the normalised value of the green color channel associated with the point.
        /// </summary>
        public UInt16 GreenChannel { get; set; }

        /// <summary>
        /// Gets or sets the normalised value of the blue color channel associated with the point.
        /// </summary>
        public UInt16 BlueChannel { get; set; }
    }
}
