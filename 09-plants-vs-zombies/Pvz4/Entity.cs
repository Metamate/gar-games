using System.Collections.Generic;
using Pvz4.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pvz4;

// A thing on the lawn, made of components. An entity has a position, a row, and a list of
// components; what it looks like and what it does is all in the components.
public class Entity(World world)
{
    private readonly List<Component> _components = [];

    public World World { get; } = world;
    public Vector2 Position { get; set; }
    public int Row { get; set; }
    public bool IsRemoved { get; private set; }

    public T Add<T>(T component) where T : Component
    {
        component.Owner = this;
        _components.Add(component);
        return component;
    }

    // For building entities in one expression: new Entity(world).With(a).With(b)
    public Entity With(Component component)
    {
        Add(component);
        return this;
    }

    // Components find each other through their owner: a Walker asks for the Eater.
    public T Get<T>() where T : Component
    {
        foreach (Component component in _components)
        {
            if (component is T match)
                return match;
        }
        return null;
    }

    public void Update(float deltaSeconds)
    {
        foreach (Component component in _components)
            component.Update(deltaSeconds);
    }

    // In the order the components were added: the body first, then what's drawn on it.
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Component component in _components)
            component.Draw(spriteBatch);
    }

    // Marks the entity; the world removes it after the update.
    public void Remove() => IsRemoved = true;
}
