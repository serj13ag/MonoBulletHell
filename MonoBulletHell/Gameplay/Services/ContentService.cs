using Microsoft.Xna.Framework.Content;
using MonoBulletHell.Core.Graphics;

namespace MonoBulletHell.Gameplay.Services;

public interface IContentService
{
    void Load(ContentManager content);

    Sprite CreateSprite(string spriteName);
}

public class ContentService : IContentService
{
    private TextureAtlas _atlas;

    public void Load(ContentManager content)
    {
        _atlas = TextureAtlas.FromFile(content, "images/atlas-definition.json");
    }

    public Sprite CreateSprite(string spriteName)
    {
        return _atlas.CreateSprite(spriteName);
    }
}