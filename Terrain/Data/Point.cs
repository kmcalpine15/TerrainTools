﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terrain.Data
{
    public struct Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Point pt)
            {
                return this.X == pt.X && this.Y == pt.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
    }
}
