namespace HexCS.Core
{
    /// <summary>
    /// A 3D coordinate with an x and y and z value where x and y are always integers. IMMUTABLE
    /// </summary>
    public struct DiscreteVector3
    {
        /// <summary>
        /// Make DiscreteVector2 with two int coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public DiscreteVector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// The X value of the coordinate
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// The Y value of the coordinate
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// The Z value of the coordinate
        /// </summary>
        public int Z { get; private set; }

        /// <summary>
        /// The zero vector
        /// </summary>
        public static DiscreteVector3 Zero => new DiscreteVector3(0, 0, 0);

        /// <summary>
        /// Returns new GridCoordinate2 with x and y values summed
        /// </summary>
        /// <param name="vec1">First GridCoordinate2</param>
        /// <param name="vec2">Second GridCoordinate2</param>
        /// <returns>addition</returns>
        public static DiscreteVector3 operator +(DiscreteVector3 vec1, DiscreteVector3 vec2)
        {
            return new DiscreteVector3(vec1.X + vec2.X, vec1.Y + vec2.Y, vec1.Z + vec2.Z);
        }

        /// <summary>
        /// Returns new GridCoordinate2 with x and y values subtracted.
        /// </summary>
        /// <param name="vec1">Minuhend</param>
        /// <param name="vec2">Subtrahend</param>
        /// <returns>difference</returns>
        public static DiscreteVector3 operator -(DiscreteVector3 vec1, DiscreteVector3 vec2)
        {
            return new DiscreteVector3(vec1.X - vec2.X, vec1.Y - vec2.Y, vec1.Z - vec2.Z);
        }

        /// <inheritdoc />
        public static bool operator ==(DiscreteVector3 vec1, DiscreteVector3 vec2) => vec1.X == vec2.X && vec1.Y == vec2.Y && vec1.Z == vec2.Z;

        /// <inheritdoc />
        public static bool operator !=(DiscreteVector3 vec1, DiscreteVector3 vec2) => !(vec1.X == vec2.X && vec1.Y == vec2.Y && vec1.Z == vec2.Z);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is DiscreteVector3 vector && X == vector.X && Y == vector.Y && Z == vector.Z;

        /// <inheritdoc />
        public override int GetHashCode() => UTHash.BasicHash(X, Y);
    }
}

