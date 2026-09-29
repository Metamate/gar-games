using GARCore.ECS.Components;
using GARCore.ECS;
using GARCore.Physics;
using GeometryWars1.Utils;

namespace GeometryWars1.Components.Physics;

// Rotates an entity to face its current velocity.
public sealed class FaceVelocity : Component
{
    private Transform _transform;
    private Rigidbody _rigidbody;

    public override void OnStart(Entity owner)
    {
        _transform = owner.Transform;
        _rigidbody = owner.RequireComponent<Rigidbody>();
    }

    public override void PostUpdate(Entity owner)
    {
        if (_rigidbody.Velocity.LengthSquared() > 0.01f)
            _transform.Orientation = _rigidbody.Velocity.ToAngle();
    }
}
