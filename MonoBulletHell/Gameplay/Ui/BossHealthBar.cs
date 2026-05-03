using Microsoft.Xna.Framework;
using MonoBulletHell.Data.DTOs;
using MonoBulletHell.Gameplay.Services;
using MonoGameGum.GueDeriving;

namespace MonoBulletHell.Gameplay.Ui;

public class BossHealthBar : ColoredRectangleRuntime
{
    private const float TotalWidth = 300;

    private readonly IBossService _bossService;

    public BossHealthBar(IBossService bossService)
    {
        _bossService = bossService;

        Anchor(Gum.Wireframe.Anchor.Top);

        Y = 5;
        Width = TotalWidth;
        Height = 5;
        Color = Color.LightGreen;
    }

    public void Enable()
    {
        SetWidth(1f);
        _bossService.Boss.HealthChanged += OnBossHealthChanged;

        Visible = true;
    }

    public void Disable()
    {
        if (_bossService.Boss != null)
        {
            _bossService.Boss.HealthChanged -= OnBossHealthChanged;
        }

        Visible = false;
    }

    private void OnBossHealthChanged(HealthChangedDTO healthChangedDto)
    {
        var t = healthChangedDto.NewHealth / (float)healthChangedDto.MaxHealth;
        SetWidth(t);
    }

    private void SetWidth(float t)
    {
        Width = MathHelper.Lerp(0f, TotalWidth, t);
    }
}