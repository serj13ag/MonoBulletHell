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
    private readonly List<SpriteRenderRequest> _requestsWithEffect = new List<SpriteRenderRequest>();
    private readonly List<SpriteRenderRequest> _simpleRequests = new List<SpriteRenderRequest>();
    private BackgroundRenderRequest _backgroundRequest;

    public void AddBackground(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color,
        SamplerState samplerState)
    {
        _backgroundRequest = new BackgroundRenderRequest(texture, destinationRectangle, sourceRectangle, color, samplerState);
    }

    public void AddSprite(Sprite sprite, Vector2 position, float rotation, Effect effect = null)
    {
        if (effect != null)
        {
            _requestsWithEffect.Add(new SpriteRenderRequest(sprite, position, rotation, effect));
        }
        else
        {
            _simpleRequests.Add(new SpriteRenderRequest(sprite, position, rotation));
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // background
        spriteBatch.Begin(samplerState: _backgroundRequest.SamplerState);
        _backgroundRequest.Draw(spriteBatch);
        spriteBatch.End();

        _backgroundRequest = null;

        // requests with effects
        foreach (var effect in _requestsWithEffect)
        {
            spriteBatch.Begin(samplerState: Constants.SamplerState, effect: effect.Effect);
            effect.Draw(spriteBatch);
            spriteBatch.End();
        }

        _requestsWithEffect.Clear();

        // simple requests
        spriteBatch.Begin(samplerState: Constants.SamplerState);

        foreach (var renderRequest in _simpleRequests)
        {
            renderRequest.Draw(spriteBatch);
        }

        spriteBatch.End();

        _simpleRequests.Clear();
    }
}