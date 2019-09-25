namespace ELTE.AEGIS.IO.Lasfile
{
    using System;

    /// <summary>
    /// Defines behavior of LAS point geometries.
    /// </summary>
    public interface ILasPoint : IBasicPoint
    {
        /// <summary>
        /// Gets the point data record format associated with the point.
        /// </summary>
        Byte Format { get; }
    }
}
