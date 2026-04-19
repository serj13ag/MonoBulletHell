using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoBulletHell.Data;

namespace MonoBulletHell.Helpers;

public static class FormationHelper
{
    public static List<Vector2> GetSpawnPositions(FormationData formation)
    {
        switch (formation.Type)
        {
            case FormationType.Line:
                return GetLinePositions(formation);
            case FormationType.Circle:
                return GetCirclePositions(formation);
            case FormationType.Grid:
                return GetGridPositions(formation);
            case FormationType.Undefined:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private static List<Vector2> GetLinePositions(FormationData formationData)
    {
        var positions = new List<Vector2>();

        var totalWidth = (formationData.Count - 1) * formationData.Spacing;
        var startX = -totalWidth / 2f;

        for (var i = 0; i < formationData.Count; i++)
        {
            var offset = new Vector2(startX + i * formationData.Spacing, 0);
            positions.Add(RotateAroundCenter(offset, formationData.Rotation));
        }

        return positions;
    }

    private static List<Vector2> GetCirclePositions(FormationData formationData)
    {
        var positions = new List<Vector2>();

        for (var i = 0; i < formationData.Count; i++)
        {
            var angle = i / (float)formationData.Count * MathF.PI * 2;
            var offset = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * formationData.Radius;
            positions.Add(RotateAroundCenter(offset, formationData.Rotation));
        }

        return positions;
    }

    private static List<Vector2> GetGridPositions(FormationData formation)
    {
        var positions = new List<Vector2>();

        var columns = formation.Columns;
        var rows = formation.Rows;

        var width = (columns - 1) * formation.SpacingX;
        var height = (rows - 1) * formation.SpacingY;

        var startX = -width / 2f;
        var startY = -height / 2f;

        var count = columns * rows;

        for (var i = 0; i < count; i++)
        {
            var col = i % columns;
            var row = i / columns;

            var offset = new Vector2(
                startX + col * formation.SpacingX,
                startY + row * formation.SpacingY
            );

            positions.Add(RotateAroundCenter(offset, formation.Rotation));
        }

        return positions;
    }

    private static Vector2 RotateAroundCenter(Vector2 v, float degrees)
    {
        var rad = MathHelper.ToRadians(degrees);
        var cos = MathF.Cos(rad);
        var sin = MathF.Sin(rad);

        return new Vector2(
            v.X * cos - v.Y * sin,
            v.X * sin + v.Y * cos
        );
    }
}