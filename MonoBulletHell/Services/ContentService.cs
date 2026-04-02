using Microsoft.Xna.Framework.Content;
using MonoBulletHell.Core.Graphics;

namespace MonoBulletHell.Services;

public class ContentService
{
    private TextureAtlas _atlas;

    public void Load(ContentManager content)
    {
        _atlas = TextureAtlas.FromFile(content, "images/atlas-definition.json");
    }

    public Sprite GetShipSprite()
    {
        var shipSprite = _atlas.CreateSprite("ship");
        shipSprite.CenterOrigin();
        return shipSprite;
    }
}