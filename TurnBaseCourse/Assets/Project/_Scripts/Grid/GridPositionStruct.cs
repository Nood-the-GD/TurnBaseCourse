using System;
using System.Numerics;

namespace Game
{
    public struct GridPosition : IEquatable<GridPosition>
    {
        public int X;
        public int Z;
        public int floor;

        public GridPosition(int x, int z, int floor)
        {
            this.X = x;
            this.Z = z;
            this.floor = floor;
        }

        public override string ToString()
        {
            return $"x: {X}, z: {Z}, floor: {floor}";
        }

        public override bool Equals(object obj)
        {
            return obj is GridPosition position && X == position.X && Z == position.Z && floor == position.floor;
        }
        public bool Equals(GridPosition other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Z, floor);
        }
        public UnityEngine.Vector3 GetWorldPosition()
        {
            return LevelGrid.Instance.GetWorldPosition(this);
        }


        public static bool operator ==(GridPosition a, GridPosition b)
        {
            return a.X == b.X && a.Z == b.Z && a.floor == b.floor;
        }

        public static bool operator !=(GridPosition a, GridPosition b)
        {
            return !(a == b);
        }
        public static GridPosition operator +(GridPosition a, GridPosition b)
        {
            return new GridPosition(a.X + b.X, a.Z + b.Z, a.floor + b.floor);
        }
        public static GridPosition operator -(GridPosition a, GridPosition b)
        {
            return new GridPosition(a.X - b.X, a.Z - b.Z, a.floor + b.floor);
        }
    }
}