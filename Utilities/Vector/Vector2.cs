using System;
using System.Collections.Generic;
using System.Text;

namespace HexCS.Core
{
    /// <summary>
    /// 2D Vector
    /// </summary>
    public struct Vector2
    {
        /// <summary>
        /// X coordinate of vector 2
        /// </summary>
        public float X;

        /// <summary>
        /// Y Coordinate of a vector 2
        /// </summary>
        public float Y;

        /// <summary>
        /// Construct Vector2
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Zero Vector
        /// </summary>
        public static Vector2 Zero => new Vector2(0, 0);

        /// <summary>
        /// Returns new GridCoordinate2 with x and y values summed
        /// </summary>
        /// <param name="vec1">First GridCoordinate2</param>
        /// <param name="vec2">Second GridCoordinate2</param>
        /// <returns>addition</returns>
        public static Vector2 operator +(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        /// <summary>
        /// Returns new GridCoordinate2 with x and y values subtracted.
        /// </summary>
        /// <param name="vec1">Minuhend</param>
        /// <param name="vec2">Subtrahend</param>
        /// <returns>difference</returns>
        public static Vector2 operator -(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }

        /// <inheritdoc />
        public static bool operator ==(Vector2 vec1, Vector2 vec2) => vec1.X == vec2.X && vec1.Y == vec2.Y;

        /// <inheritdoc />
        public static bool operator !=(Vector2 vec1, Vector2 vec2) => !(vec1.X == vec2.X && vec1.Y == vec2.Y);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Vector2 vector && X == vector.X && Y == vector.Y;

        /// <inheritdoc />
        public override int GetHashCode() => UTHash.BasicHash(X, Y);
    }
}
