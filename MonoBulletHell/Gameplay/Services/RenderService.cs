using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Rendering;

namespace MonoBulletHell.Gameplay.Services;

public interface IRenderService
{
    void AddBackground(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color,
        SamplerState samplerState);

    void AddSprite(Sprite sprite, Vector2 position, float rotation, Effect effect = null);

    void Draw(SpriteBatch spriteBatch);
}

public class RenderService : IRenderService
{
    private BackgroundRenderRequest _backgroundBatch;
    private readonly List<SpriteRenderRequest> _simpleBatches = new List<SpriteRenderRequest>(64);
    private readonly List<SpriteRenderRequest> _effectBatches = new List<SpriteRenderRequest>(32);

    public void AddBackground(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color,
        SamplerState samplerState)
    {
        _backgroundBatch = new BackgroundRenderRequest(texture, destinationRectangle, sourceRectangle, color, samplerState);
    }

    public void AddSprite(Sprite sprite, Vector2 position, float rotation, Effect effect = null)
    {
        if (effect != null)
        {
            _effectBatches.Add(new SpriteRenderRequest(sprite, position, rotation, effect));
        }
        else
        {
            _simpleBatches.Add(new SpriteRenderRequest(sprite, position, rotation));
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // background
        if (_backgroundBatch != null)
        {
            spriteBatch.Begin(samplerState: _backgroundBatch.SamplerState);
            _backgroundBatch.Draw(spriteBatch);
            spriteBatch.End();

            _backgroundBatch = null;
        }

        // simple
        spriteBatch.Begin(samplerState: Constants.SamplerState);

        foreach (var renderRequest in _simpleBatches)
        {
            renderRequest.Draw(spriteBatch);
        }

        spriteBatch.End();

        _simpleBatches.Clear();

        // effects
        foreach (var effect in _effectBatches)
        {
            spriteBatch.Begin(samplerState: Constants.SamplerState, effect: effect.Effect);
            effect.Draw(spriteBatch);
            spriteBatch.End();
        }

        _effectBatches.Clear();
    }
}