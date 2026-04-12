using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics;

namespace MonoBulletHell.Gameplay.Services;

public interface IContentService
{
    void Load(ContentManager content);

    Sprite CreateSprite(string spriteName);
    Effect GetFlashEffect();
}

public class ContentService : IContentService
{
    private TextureAtlas _atlas;
    private Effect _flashEffect;

    public void Load(ContentManager content)
    {
        _atlas = TextureAtlas.FromFile(content, "images/atlas-definition.json");

        _flashEffect = content.Load<Effect>("shaders/flashEffect");
    }

    public Sprite CreateSprite(string spriteName)
    {
        return _atlas.CreateSprite(spriteName);
    }

    public Effect GetFlashEffect()
    {
        return _flashEffect;
    }
}