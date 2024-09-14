namespace Mazinator.Scripts
{
    /// <summary>
    /// Simple Coordinate struct which allows addition of two Coordinates and multiplication with scalar values
    /// </summary>
    internal struct Coordinate
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Coordinate(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Coordinate operator +(Coordinate coord1, Coordinate coord2)
        {
            return new Coordinate(coord1.X + coord2.X, coord1.Y + coord2.Y);
        }

        public static Coordinate operator *(Coordinate coord, int scalar)
        {
            return new Coordinate(coord.X * scalar, coord.Y * scalar);
        }

        public static Coordinate operator *(int scalar, Coordinate coord)
        {
            return new Coordinate(coord.X * scalar, coord.Y * scalar);
        }
    }
}
