namespace Birds1.Physics;

// How a body takes part in the simulation.
// Static: never moves (the ground). Kinematic: moves at the velocity you give it, and pushes
// dynamic bodies without being pushed back (a moving platform). Dynamic: moved by gravity
// and collisions (everything else).
public enum BodyType { Static, Kinematic, Dynamic }
