using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Data;
using Newtonsoft.Json;

namespace MonoBulletHell.Core.Graphics;

public class TextureAtlas
{
    private readonly Texture2D _texture;
    private readonly Dictionary<string, TextureRegion> _regions;

    private TextureAtlas(Texture2D texture)
    {
        _texture = texture;
        _regions = new Dictionary<string, TextureRegion>();
    }

    public Sprite CreateSprite(string regionName)
    {
        var region = GetRegion(regionName);
        return new Sprite(region);
    }

    public static TextureAtlas FromFile(ContentManager content, string fileName)
    {
        var filePath = Path.Combine(content.RootDirectory, fileName);
        var json = File.ReadAllText(filePath);

        var atlasData = JsonConvert.DeserializeObject<TextureAtlasData>(json);

        var texture = content.Load<Texture2D>(atlasData.Texture);

        var atlas = new TextureAtlas(texture);
        if (atlasData.Regions != null)
        {
            foreach (var regionData in atlasData.Regions)
            {
                atlas.AddRegion(regionData.Name, regionData.X, regionData.Y, regionData.Width, regionData.Height);
            }
        }

        return atlas;
    }

    private void AddRegion(string name, int x, int y, int width, int height)
    {
        var region = new TextureRegion(_texture, x, y, width, height);
        _regions.Add(name, region);
    }

    private TextureRegion GetRegion(string name)
    {
        return _regions[name];
    }
}