using System;
using System.Collections.Generic;
using System.Linq;
using Survivors4;
using Microsoft.Xna.Framework;
using Xunit;

namespace Survivors.Tests;

// An optimization must give the same answers as the slow, obvious code it replaces. These tests
// compare the grid with checking every point, on thousands of random points.
public class FlatGridTests
{
    private static Vector2[] RandomPoints(int count, int seed)
    {
        var random = new Random(seed);
        return Enumerable.Range(0, count)
                         .Select(_ => new Vector2(random.NextSingle() * 2000 - 1000, random.NextSingle() * 2000 - 1000))
                         .ToArray();
    }

    // The slow, obvious search: check every point.
    private static List<int> CheckEveryPoint(Vector2[] points, Vector2 center, float radius)
        => Enumerable.Range(0, points.Length)
                     .Where(i => Vector2.DistanceSquared(points[i], center) <= radius * radius)
                     .ToList();

    // A grid of 32 x 32 cells of 48 pixels: 1536 pixels across, so some points are outside it.
    private static FlatGrid GridWith(Vector2[] points)
    {
        var grid = new FlatGrid(48, 32, 32);
        grid.Build(Vector2.Zero, points, points.Length);
        return grid;
    }

    // A [Theory] runs once for each [InlineData]: here, with radii smaller than a cell, about
    // one cell, and many cells.
    [Theory]
    [InlineData(10f)]
    [InlineData(48f)]
    [InlineData(150f)]
    [InlineData(500f)]
    public void The_grid_finds_the_same_points_as_checking_every_point(float radius)
    {
        Vector2[] points = RandomPoints(2000, seed: 1);
        FlatGrid grid = GridWith(points);
        var random = new Random(2);
        var results = new List<int>();

        for (int n = 0; n < 100; n++)
        {
            var center = new Vector2(random.NextSingle() * 2200 - 1100, random.NextSingle() * 2200 - 1100);
            grid.Query(center, radius, results);
            Assert.Equal(CheckEveryPoint(points, center, radius), results.OrderBy(i => i));
        }
    }

    [Fact]
    public void Points_far_outside_the_grid_are_still_found()
    {
        Vector2[] points = [new(5000, 0), new(5003, 4), new(-4000, -4000), new(0, 0)];
        FlatGrid grid = GridWith(points);
        var results = new List<int>();

        grid.Query(new Vector2(5000, 0), 10, results);

        Assert.Equal([0, 1], results.OrderBy(i => i));
    }

    [Fact]
    public void Every_item_is_in_exactly_one_cell()
    {
        Vector2[] points = RandomPoints(500, seed: 3);
        FlatGrid grid = GridWith(points);

        Assert.Equal(points.Length, grid.CellStart[grid.Columns * grid.Rows]);
        Assert.Equal(Enumerable.Range(0, points.Length), grid.Items.Take(points.Length).OrderBy(i => i));
    }
}
