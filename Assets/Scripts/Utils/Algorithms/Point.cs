using System;

namespace Algorithms
{
    public class Point
    {

        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "("+X+","+Y+")";
        }

        public float LengthSq
        {
            get
            {
                return X * X + Y * Y;
            }
        }

        public static float DistanceSq(Point v0, Point v1)
        {
            Point point = v0 - v1;
            return point.LengthSq;
        }

        public static float Distance(Point v0, Point v1)
        {
            return (float)Math.Sqrt((double)Point.DistanceSq(v0, v1));
        }

        public static bool operator ==(Point lhs, Point rhs)
        {
            if (object.Equals(lhs, null) || object.Equals(rhs, null))
            {
                return object.Equals(lhs, rhs);
            }
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public static bool operator !=(Point lhs, Point rhs)
        {
            return !(lhs == rhs);
        }

        public static Point operator -(Point v0, Point v1)
        {
            return new Point(0, 0)
            {
                X = v0.X - v1.X,
                Y = v0.Y - v1.Y
            };
        }

        public static Point operator -(Point v)
        {
            return new Point(-v.X, -v.Y);
        }
    }
}
