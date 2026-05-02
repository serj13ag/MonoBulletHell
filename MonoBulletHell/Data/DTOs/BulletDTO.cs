using Microsoft.Xna.Framework;

namespace MonoBulletHell.Data.DTOs;

public readonly struct BulletDTO
{
    public Vector2 Position { get; init; }
    public Vector2 Direction { get; init; }
    public float Speed { get; init; }
    public int Damage { get; init; }
    public bool IsPlayer { get; init; }
    public float Acceleration { get; init; }
    public float AngularVelocity { get; init; }
}