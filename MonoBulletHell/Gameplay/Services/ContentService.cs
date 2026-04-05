using Microsoft.Xna.Framework.Content;
using MonoBulletHell.Core.Graphics;

namespace MonoBulletHell.Gameplay.Services;

public interface IContentService
{
    void Load(ContentManager content);

    Sprite GetShipSprite();
    Sprite GetShipCoreSprite();
    Sprite GetBulletSprite();
}

public class ContentService : IContentService
{
    private TextureAtlas _atlas;

    public void Load(ContentManager content)
    {
        _atlas = TextureAtlas.FromFile(content, "images/atlas-definition.json");
    }

    public Sprite GetShipSprite()
    {
        return _atlas.CreateSprite("ship");
    }

    public Sprite GetShipCoreSprite()
    {
        return _atlas.CreateSprite("shipCore");
    }

    public Sprite GetBulletSprite()
    {
        return _atlas.CreateSprite("bullet");
    }
}