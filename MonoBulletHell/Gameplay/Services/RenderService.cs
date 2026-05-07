using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;
using MonoBulletHell.Gameplay.Rendering;
using MonoBulletHell.Helpers;
using MonoBulletHell.Services;

namespace MonoBulletHell.Gameplay.Services;

public interface IRenderService
{
    void SetBackgroundBatch(Texture2D texture, int verticalOffset);

    void AddSprite(Sprite sprite, Vector2 position, float rotation, Layer layer, Effect effect = null);

    void PrepareDraw();
    void Draw(SpriteBatch spriteBatch);
}

public class RenderService : IRenderService
{
    private readonly IContentService _contentService;

    private static readonly SamplerState CurrentSamplerState = SamplerState.PointClamp;

    private static readonly Dictionary<Layer, int> LayerOrderLookup = new Dictionary<Layer, int>()
    {
        { Layer.Ship, 0 },
        { Layer.Enemies, 1 },
        { Layer.Bullets, 2 },
        { Layer.Particles, 3 },
    };

    private BackgroundRenderRequest _backgroundBatch;
    private readonly List<SpriteRenderRequest> _spriteRequests = new List<SpriteRenderRequest>(512);

    public RenderService(IContentService contentService)
    {
        _contentService = contentService;
    }

    public void SetBackgroundBatch(Texture2D texture, int verticalOffset)
    {
        _backgroundBatch = new BackgroundRenderRequest(texture, verticalOffset, ColorHelper.FromHex(_contentService.GetColorConfig().GameplayBackgroundTexture));
    }

    public void AddSprite(Sprite sprite, Vector2 position, float rotation, Layer layer, Effect effect = null)
    {
        _spriteRequests.Add(new SpriteRenderRequest(sprite, position, rotation, layer, effect));
    }

    public void PrepareDraw()
    {
        _spriteRequests.Sort((a, b) =>
        {
            var layerA = LayerOrderLookup[a.Layer];
            var layerB = LayerOrderLookup[b.Layer];

            // by layers
            var layerCompare = layerA.CompareTo(layerB);
            if (layerCompare != 0)
            {
                return layerCompare;
            }

            // then by effects
            if (a.Effect == b.Effect)
            {
                return 0;
            }

            if (a.Effect == null)
            {
                return -1;
            }

            if (b.Effect == null)
            {
                return 1;
            }

            return a.PositionY.CompareTo(b.PositionY);
        });
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // background
        if (_backgroundBatch != null)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointWrap);
            _backgroundBatch.Draw(spriteBatch);
            spriteBatch.End();

            _backgroundBatch = null;
        }

        // sprites
        Effect currentEffect = null;

        spriteBatch.Begin(samplerState: CurrentSamplerState);

        foreach (var spriteRequest in _spriteRequests)
        {
            if (spriteRequest.Effect != currentEffect)
            {
                spriteBatch.End();
                spriteBatch.Begin(samplerState: CurrentSamplerState, effect: spriteRequest.Effect);
                currentEffect = spriteRequest.Effect;
            }

            spriteRequest.Draw(spriteBatch);
        }

        spriteBatch.End();

        _spriteRequests.Clear();
    }
}