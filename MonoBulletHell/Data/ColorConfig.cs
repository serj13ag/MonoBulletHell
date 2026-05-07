using System;

namespace MonoBulletHell.Data;

[Serializable]
public class ColorConfig
{
    public ColorData UiPanel { get; set; }
    public ColorData UiButton { get; set; }

    public ColorData TitleSceneBackground { get; set; }
    public ColorData GameplaySceneBackground { get; set; }
    public ColorData GameplayBackgroundTexture { get; set; }

    public ColorData PlayerShip { get; set; }
    public ColorData PlayerShipCore { get; set; }

    public ColorData Enemy { get; set; }

    public ColorData PlayerBullet { get; set; }
    public ColorData EnemyBullet { get; set; }
    public ColorData BulletImpact { get; set; }
}