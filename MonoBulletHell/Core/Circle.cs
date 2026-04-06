using System;
using Microsoft.Xna.Framework;

namespace MonoBulletHell.Core;

public readonly struct Circle : IEquatable<Circle>
{
    public readonly float X;
    public readonly float Y;
    public readonly float Radius;

    public Vector2 Location => new Vector2(X, Y);

    public Circle(float x, float y, float radius)
    {
        X = x;
        Y = y;
        Radius = radius;
    }

    public bool Intersects(Circle other)
    {
        var radiiSquared = (Radius + other.Radius) * (Radius + other.Radius);
        var distanceSquared = Vector2.DistanceSquared(Location, other.Location);
        return distanceSquared < radiiSquared;
    }

    public override bool Equals(object obj) => obj is Circle other && Equals(other);
    public bool Equals(Circle other) => X == other.X && Y == other.Y && Radius == other.Radius;
    public override int GetHashCode() => HashCode.Combine(X, Y, Radius);
    public static bool operator ==(Circle lhs, Circle rhs) => lhs.Equals(rhs);
    public static bool operator !=(Circle lhs, Circle rhs) => !lhs.Equals(rhs);
}