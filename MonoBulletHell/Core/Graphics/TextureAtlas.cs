using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoBulletHell.Core.Graphics.Data;
using Newtonsoft.Json;

namespace MonoBulletHell.Core.Graphics;

public class TextureAtlas
{
    private readonly Texture2D _texture;
    private readonly Dictionary<string, TextureRegion> _regions;
    private readonly Dictionary<string, Animation> _animations;

    private TextureAtlas(Texture2D texture)
    {
        _texture = texture;
        _regions = new Dictionary<string, TextureRegion>();
        _animations = new Dictionary<string, Animation>();
    }

    public Sprite CreateSprite(string regionName)
    {
        var region = GetRegion(regionName);
        return new Sprite(region);
    }

    public AnimatedSprite CreateAnimatedSprite(string animationName)
    {
        var animation = GetAnimation(animationName);
        return new AnimatedSprite(animation);
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

        if (atlasData.Animations != null)
        {
            foreach (var animationData in atlasData.Animations)
            {
                var frames = new List<TextureRegion>();
                if (animationData.Frames != null)
                {
                    foreach (var frame in animationData.Frames)
                    {
                        frames.Add(atlas.GetRegion(frame));
                    }
                }

                var animation = new Animation(frames, animationData.Fps);
                atlas.AddAnimation(animationData.Name, animation);
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

    private void AddAnimation(string animationName, Animation animation)
    {
        _animations.Add(animationName, animation);
    }

    private Animation GetAnimation(string animationName)
    {
        return _animations[animationName];
    }
}