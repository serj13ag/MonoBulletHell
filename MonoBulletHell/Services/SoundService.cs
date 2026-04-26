using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using MonoBulletHell.Enums;

namespace MonoBulletHell.Services;

public interface ISoundService : IDisposable
{
    void Initialize();
    void Update();

    void PlaySong(SongType songType, bool isRepeating = true);
    SoundEffectInstance PlaySoundEffect(SfxType sfxType);

    void PauseAll();
    void ResumeAll();
    void StopAll();
}

public class SoundService : ISoundService
{
    private const string MenuMusic = "audio/music_fmceretta_426699";
    private const string GameplayMusic = "audio/music_foolboymedia_320232";

    private const string PlayerShootSfx = "audio/sfx_kenney_laserSmall_000";
    private const string EnemyDiedSfx = "audio/sfx_kenney_lowFrequency_explosion_001";

    private const string ClickSfx = "audio/sfx_kenney_click_001";

    private readonly ContentManager _content;

    private readonly List<SoundEffectInstance> _activeSoundEffectInstances = new List<SoundEffectInstance>();

    private Dictionary<SongType, Song> _songs;
    private Dictionary<SfxType, SoundEffect> _soundEffects;

    public SoundService(ContentManager content)
    {
        _content = content;
    }

    public void Initialize()
    {
        _songs = new Dictionary<SongType, Song>()
        {
            { SongType.Menu, _content.Load<Song>(MenuMusic) },
            { SongType.Gameplay, _content.Load<Song>(GameplayMusic) },
        };

        _soundEffects = new Dictionary<SfxType, SoundEffect>
        {
            { SfxType.Click, _content.Load<SoundEffect>(ClickSfx) },
            { SfxType.PlayerShoot, _content.Load<SoundEffect>(PlayerShootSfx) },
            { SfxType.EnemyDied, _content.Load<SoundEffect>(EnemyDiedSfx) },
        };
    }

    public void Update()
    {
        for (var i = _activeSoundEffectInstances.Count - 1; i >= 0; i--)
        {
            var instance = _activeSoundEffectInstances[i];

            if (instance.State == SoundState.Stopped)
            {
                if (!instance.IsDisposed)
                {
                    instance.Dispose();
                }

                _activeSoundEffectInstances.RemoveAt(i);
            }
        }
    }

    public void PlaySong(SongType songType, bool isRepeating = true)
    {
        if (MediaPlayer.State == MediaState.Playing)
        {
            MediaPlayer.Stop();
        }

        var song = _songs[songType];
        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = isRepeating;
    }

    public SoundEffectInstance PlaySoundEffect(SfxType sfxType)
    {
        var soundEffect = _soundEffects[sfxType];
        return PlaySoundEffect(soundEffect, 1.0f, 0.0f, 0.0f, false);
    }

    public void PauseAll()
    {
        MediaPlayer.Pause();

        foreach (var soundEffectInstance in _activeSoundEffectInstances)
        {
            soundEffectInstance.Pause();
        }
    }

    public void ResumeAll()
    {
        MediaPlayer.Resume();

        foreach (var soundEffectInstance in _activeSoundEffectInstances)
        {
            soundEffectInstance.Resume();
        }
    }

    public void StopAll()
    {
        MediaPlayer.Stop();
        StopAllSoundEffects();
    }

    private SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect, float volume, float pitch, float pan, bool isLooped)
    {
        var soundEffectInstance = soundEffect.CreateInstance();

        soundEffectInstance.Volume = volume;
        soundEffectInstance.Pitch = pitch;
        soundEffectInstance.Pan = pan;
        soundEffectInstance.IsLooped = isLooped;

        soundEffectInstance.Play();

        _activeSoundEffectInstances.Add(soundEffectInstance);

        return soundEffectInstance;
    }

    private void StopAllSoundEffects()
    {
        foreach (var soundEffectInstance in _activeSoundEffectInstances)
        {
            soundEffectInstance.Stop();
            soundEffectInstance.Dispose();
        }

        _activeSoundEffectInstances.Clear();
    }

    public void Dispose()
    {
        StopAllSoundEffects();
    }
}