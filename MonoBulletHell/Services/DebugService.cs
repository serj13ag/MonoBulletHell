using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoBulletHell.Services;

public interface IDebugService
{
    void DrawCircle(Vector2 center, float radius, Color color, float thickness, int segments);
    void DrawRectangle(Rectangle rect, Color color, float thickness);

    void Update();
    void Render();
}

public class DebugService : IDebugService
{
    private struct DrawLineCommand
    {
        public Vector2 Start;
        public Vector2 End;
        public Color Color;
        public float Thickness;
    }

    private readonly SpriteBatch _spriteBatch;
    private readonly IInputService _inputService;

    private readonly Texture2D _pixelTexture;
    private readonly List<DrawLineCommand> _drawCommands = new(128);

    private bool _enabled;

    public DebugService(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, IInputService inputService)
    {
        _spriteBatch = spriteBatch;
        _inputService = inputService;

        _pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });
    }

    public void DrawCircle(Vector2 center, float radius, Color color, float thickness, int segments)
    {
        if (!_enabled)
        {
            return;
        }

        var lastPoint = center + new Vector2(radius, 0);

        for (var i = 1; i <= segments; i++)
        {
            var angle = MathHelper.TwoPi * i / segments;
            var nextPoint = center + radius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            AddDrawLineCommand(lastPoint, nextPoint, color, thickness);
            lastPoint = nextPoint;
        }
    }

    public void DrawRectangle(Rectangle rect, Color color, float thickness)
    {
        if (!_enabled)
        {
            return;
        }

        var topLeft = new Vector2(rect.Left, rect.Top);
        var topRight = new Vector2(rect.Right, rect.Top);
        var bottomLeft = new Vector2(rect.Left, rect.Bottom);
        var bottomRight = new Vector2(rect.Right, rect.Bottom);

        AddDrawLineCommand(topLeft, topRight, color, thickness);
        AddDrawLineCommand(topRight, bottomRight, color, thickness);
        AddDrawLineCommand(bottomRight, bottomLeft, color, thickness);
        AddDrawLineCommand(bottomLeft, topLeft, color, thickness);
    }

    public void Update()
    {
        if (_inputService.Keyboard.WasKeyJustPressed(Keys.F1))
        {
            _enabled = !_enabled;
        }
    }

    public void Render()
    {
        if (!_enabled || _drawCommands.Count == 0)
        {
            return;
        }

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (var drawCommand in _drawCommands)
        {
            DrawLine(_spriteBatch, _pixelTexture, drawCommand.Start, drawCommand.End, drawCommand.Color, drawCommand.Thickness);
        }

        _drawCommands.Clear();

        _spriteBatch.End();
    }

    private void AddDrawLineCommand(Vector2 start, Vector2 end, Color color, float thickness)
    {
        _drawCommands.Add(new DrawLineCommand
        {
            Start = start,
            End = end,
            Color = color,
            Thickness = thickness,
        });
    }

    private static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end, Color color,
        float thickness)
    {
        var edge = end - start;
        var angle = (float)Math.Atan2(edge.Y, edge.X);
        var length = edge.Length();

        var scale = new Vector2(length, thickness);

        spriteBatch.Draw(texture,
            position: start,
            sourceRectangle: null,
            color: color,
            rotation: angle,
            origin: Vector2.Zero,
            scale: scale,
            effects: SpriteEffects.None,
            layerDepth: 0f);
    }
}