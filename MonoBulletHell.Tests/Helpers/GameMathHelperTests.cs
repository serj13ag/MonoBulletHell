using System;
using Microsoft.Xna.Framework;
using MonoBulletHell.Helpers;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace MonoBulletHell.Tests.Helpers;

[TestFixture]
public class GameMathHelperTests
{
    private const float Tolerance = 0.0001f;

    #region GetRotation Tests

    [Test]
    public void GetRotation_RightDirection_ReturnsHalfPi()
    {
        var direction = new Vector2(1, 0);
        var result = GameMathHelper.GetRotation(direction);
        Assert.That(result, Is.EqualTo(MathHelper.PiOver2).Within(Tolerance));
    }

    [Test]
    public void GetRotation_UpDirection_ReturnsZero()
    {
        var direction = new Vector2(0, -1);
        var result = GameMathHelper.GetRotation(direction);
        Assert.That(result, Is.EqualTo(0f).Within(Tolerance));
    }

    [Test]
    public void GetRotation_DownDirection_ReturnsPi()
    {
        var direction = new Vector2(0, 1);
        var result = GameMathHelper.GetRotation(direction);
        Assert.That(result, Is.EqualTo(MathHelper.Pi).Within(Tolerance));
    }

    [Test]
    public void GetRotation_LeftDirection_ReturnsThreePiOverTwo()
    {
        var direction = new Vector2(-1, 0);
        var result = GameMathHelper.GetRotation(direction);
        Assert.That(result, Is.EqualTo(MathHelper.Pi + MathHelper.PiOver2).Within(Tolerance));
    }

    #endregion

    #region DegreeToDirection Tests

    [Test]
    public void DegreeToDirection_Zero_ReturnsRight()
    {
        var result = GameMathHelper.DegreeToDirection(0f);
        Assert.That(result.X, Is.EqualTo(1f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(0f).Within(Tolerance));
    }

    [Test]
    public void DegreeToDirection_90Degrees_ReturnsUp()
    {
        var result = GameMathHelper.DegreeToDirection(90f);
        Assert.That(result.X, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(-1f).Within(Tolerance)); // Y inverted
    }

    [Test]
    public void DegreeToDirection_180Degrees_ReturnsLeft()
    {
        var result = GameMathHelper.DegreeToDirection(180f);
        Assert.That(result.X, Is.EqualTo(-1f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(0f).Within(Tolerance));
    }

    [Test]
    public void DegreeToDirection_270Degrees_ReturnsDown()
    {
        var result = GameMathHelper.DegreeToDirection(270f);
        Assert.That(result.X, Is.EqualTo(0f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(1f).Within(Tolerance)); // Y inverted
    }

    [Test]
    public void DegreeToDirection_360Degrees_EqualsZeroDegrees()
    {
        var result360 = GameMathHelper.DegreeToDirection(360f);
        var result0 = GameMathHelper.DegreeToDirection(0f);
        Assert.That(result360.X, Is.EqualTo(result0.X).Within(Tolerance));
        Assert.That(result360.Y, Is.EqualTo(result0.Y).Within(Tolerance));
    }

    [Test]
    public void DegreeToDirection_ReturnsUnitVector()
    {
        foreach (var degree in new[] { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f })
        {
            var result = GameMathHelper.DegreeToDirection(degree);
            var length = MathF.Sqrt(result.X * result.X + result.Y * result.Y);
            Assert.That(length, Is.EqualTo(1f).Within(Tolerance), $"Failed at {degree} degrees");
        }
    }

    #endregion

    #region QuadraticBezier Tests

    [Test]
    public void QuadraticBezier_TAtZero_ReturnsP0()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 10);
        var p2 = new Vector2(10, 0);

        var result = GameMathHelper.QuadraticBezier(p0, p1, p2, 0f);

        Assert.That(result.X, Is.EqualTo(p0.X).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(p0.Y).Within(Tolerance));
    }

    [Test]
    public void QuadraticBezier_TAtOne_ReturnsP2()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 10);
        var p2 = new Vector2(10, 0);

        var result = GameMathHelper.QuadraticBezier(p0, p1, p2, 1f);

        Assert.That(result.X, Is.EqualTo(p2.X).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(p2.Y).Within(Tolerance));
    }

    [Test]
    public void QuadraticBezier_TAtHalf_ReturnsMidpoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 10);
        var p2 = new Vector2(10, 0);

        var result = GameMathHelper.QuadraticBezier(p0, p1, p2, 0.5f);

        // At t=0.5: 0.25*p0 + 0.5*p1 + 0.25*p2
        Assert.That(result.X, Is.EqualTo(5f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(5f).Within(Tolerance));
    }

    [Test]
    public void QuadraticBezier_CollinearPoints_ReturnsPointOnLine()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(5, 5);
        var p2 = new Vector2(10, 10);

        var result = GameMathHelper.QuadraticBezier(p0, p1, p2, 0.5f);

        Assert.That(result.X, Is.EqualTo(5f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(5f).Within(Tolerance));
    }

    #endregion

    #region CubicBezier Tests

    [Test]
    public void CubicBezier_TAtZero_ReturnsP0()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(2, 8);
        var p2 = new Vector2(8, 8);
        var p3 = new Vector2(10, 0);

        var result = GameMathHelper.CubicBezier(p0, p1, p2, p3, 0f);

        Assert.That(result.X, Is.EqualTo(p0.X).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(p0.Y).Within(Tolerance));
    }

    [Test]
    public void CubicBezier_TAtOne_ReturnsP3()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(2, 8);
        var p2 = new Vector2(8, 8);
        var p3 = new Vector2(10, 0);

        var result = GameMathHelper.CubicBezier(p0, p1, p2, p3, 1f);

        Assert.That(result.X, Is.EqualTo(p3.X).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(p3.Y).Within(Tolerance));
    }

    [Test]
    public void CubicBezier_TAtHalf_ReturnsExpectedMidpoint()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(2, 8);
        var p2 = new Vector2(8, 8);
        var p3 = new Vector2(10, 0);

        var result = GameMathHelper.CubicBezier(p0, p1, p2, p3, 0.5f);

        // At t=0.5: 0.125*p0 + 0.375*p1 + 0.375*p2 + 0.125*p3 = (5, 6)
        Assert.That(result.X, Is.EqualTo(5f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(6f).Within(Tolerance));
    }

    [Test]
    public void CubicBezier_CollinearPoints_ReturnsPointOnLine()
    {
        var p0 = new Vector2(0, 0);
        var p1 = new Vector2(2, 2);
        var p2 = new Vector2(8, 8);
        var p3 = new Vector2(10, 10);

        var result = GameMathHelper.CubicBezier(p0, p1, p2, p3, 0.5f);

        Assert.That(result.X, Is.EqualTo(5f).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(5f).Within(Tolerance));
    }

    [Test]
    public void CubicBezier_SamePointAllControls_ReturnsTheSamePoint()
    {
        var p = new Vector2(3, 7);

        var result = GameMathHelper.CubicBezier(p, p, p, p, 0.5f);

        Assert.That(result.X, Is.EqualTo(p.X).Within(Tolerance));
        Assert.That(result.Y, Is.EqualTo(p.Y).Within(Tolerance));
    }

    #endregion
}