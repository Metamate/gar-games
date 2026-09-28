namespace Birds1.Physics;

// How a body behaves in the physics world: how heavy it is (per square metre), how much it
// grips, and how much it bounces.
public readonly record struct PhysicsMaterial(float Density, float Friction, float Restitution);
