using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoBulletHell.Data;

namespace MonoBulletHell.Helpers;

public static class FormationHelper
{
    public static List<Vector2> GetSpawnPositions(FormationData formation, Vector2 position)
    {
        IEnumerable<Vector2> formationPositions;

        switch (formation.Type)
        {
            case FormationType.Line:
                formationPositions = GetLinePositions(formation);
                break;
            case FormationType.Circle:
                formationPositions = GetCirclePositions(formation);
                break;
            case FormationType.Grid:
                formationPositions = GetGridPositions(formation);
                break;
            case FormationType.VShape:
                formationPositions = GetVShapePositions(formation);
                break;
            case FormationType.Undefined:
            default:
                throw new ArgumentOutOfRangeException();
        }

        return formationPositions
            .Select(formationPosition => position + formationPosition)
            .ToList();
    }

    private static IEnumerable<Vector2> GetLinePositions(FormationData formationData)
    {
        var totalWidth = (formationData.Count - 1) * formationData.Spacing;
        var startX = -totalWidth / 2f;

        for (var i = 0; i < formationData.Count; i++)
        {
            var offset = new Vector2(startX + i * formationData.Spacing, 0);
            yield return RotateAroundCenter(offset, formationData.Rotation);
        }
    }

    private static IEnumerable<Vector2> GetCirclePositions(FormationData formationData)
    {
        for (var i = 0; i < formationData.Count; i++)
        {
            var angle = i / (float)formationData.Count * MathF.PI * 2;
            var offset = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * formationData.Radius;
            yield return RotateAroundCenter(offset, formationData.Rotation);
        }
    }

    private static IEnumerable<Vector2> GetGridPositions(FormationData formation)
    {
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

            yield return RotateAroundCenter(offset, formation.Rotation);
        }
    }

    private static IEnumerable<Vector2> GetVShapePositions(FormationData formation)
    {
        var direction = formation.Inverted ? 1f : -1f;

        for (var row = 0; row < formation.Rows; row++)
        {
            var y = row * formation.SpacingY * direction;

            if (row == 0)
            {
                var offset = new Vector2(0, 0);
                yield return RotateAroundCenter(offset, formation.Rotation);
                continue;
            }

            var left = new Vector2(-row * formation.SpacingX, y);
            var right = new Vector2(row * formation.SpacingX, y);

            yield return RotateAroundCenter(left, formation.Rotation);
            yield return RotateAroundCenter(right, formation.Rotation);
        }
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