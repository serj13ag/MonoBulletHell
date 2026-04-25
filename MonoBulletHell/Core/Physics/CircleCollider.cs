using Microsoft.Xna.Framework;

namespace MonoBulletHell.Core.Physics;

public class CircleCollider
{
    private readonly Vector2 _offset;
    private readonly float _radius;

    private Circle _circle;

    public Vector2 Center => _circle.Location;
    public float Radius => _circle.Radius;

    public CircleCollider(Vector2 offset, float radius)
    {
        _offset = offset;
        _radius = radius;
    }

    public void Update(Vector2 position)
    {
        _circle = new Circle(position.X + _offset.X, position.Y + _offset.Y, _radius);
    }

    public bool Intersects(CircleCollider collider)
    {
        return _circle.Intersects(collider._circle);
    }
}