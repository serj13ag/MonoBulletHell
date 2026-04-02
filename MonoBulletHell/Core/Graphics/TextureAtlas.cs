using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoBulletHell.Core.Graphics;

public class TextureAtlas
{
    private readonly Texture2D _texture;
    private readonly Dictionary<string, TextureRegion> _regions;

    public TextureAtlas(Texture2D texture)
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

        using var stream = TitleContainer.OpenStream(filePath);
        using var reader = XmlReader.Create(stream);

        var doc = XDocument.Load(reader);
        var root = doc.Root;

        // The <Texture> element contains the content path for the Texture2D to load.
        // So we will retrieve that value then use the content manager to load the texture.
        var texturePath = root.Element("Texture").Value;
        var texture = content.Load<Texture2D>(texturePath);

        var atlas = new TextureAtlas(texture);

        // The <Regions> element contains individual <Region> elements, each one describing
        // a different texture region within the atlas.  
        //
        // Example:
        // <Regions>
        //      <Region name="spriteOne" x="0" y="0" width="32" height="32" />
        //      <Region name="spriteTwo" x="32" y="0" width="32" height="32" />
        // </Regions>
        //
        // So we retrieve all the <Region> elements then loop through each one
        // and generate a new TextureRegion instance from it and add it to this atlas.
        var regions = root.Element("Regions")?.Elements("Region");

        if (regions != null)
        {
            foreach (var region in regions)
            {
                var name = region.Attribute("name")?.Value;
                var x = int.Parse(region.Attribute("x")?.Value ?? "0");
                var y = int.Parse(region.Attribute("y")?.Value ?? "0");
                var width = int.Parse(region.Attribute("width")?.Value ?? "0");
                var height = int.Parse(region.Attribute("height")?.Value ?? "0");

                if (!string.IsNullOrEmpty(name))
                {
                    atlas.AddRegion(name, x, y, width, height);
                }
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