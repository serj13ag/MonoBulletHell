# Mono Bullet Hell

A small bullet hell game built with C#, MonoGame, and Gum UI. The project focuses on data-driven enemy waves, reusable gameplay services, deterministic update flow, and a clean separation between rendering, input, audio, scene management, and gameplay logic.

This repository is intended as a project demonstrating practical game architecture in C# rather than only a gameplay prototype.

## Highlights

- Scene-based game flow.
- Data-driven level content using JSON configs for data.
- Bullet pooling to reduce runtime allocations during dense firing patterns.
- Service-oriented architecture with dependency injection through LightInject.
- Virtual-resolution rendering pipeline for consistent pixel-art scaling.
- Texture atlas loading for sprites and animations.
- Configurable boss phases with path and emitter changes based on health thresholds.
- Persistent local settings for screen scale and volume.
- Gum-based UI panels and controls.

## Gameplay

Survive enemy waves, dodge incoming bullet patterns, destroy enemies, and defeat the boss encounter.

Controls:

| Action | Input |
| --- | --- |
| Move | `WASD` or arrow keys |
| Fire | `Space` |
| Focus / slow movement | `Left Shift` |
| Pause / back | `Escape` |
| Debug overlay | `F1` |

## Architecture

The codebase is organized around small systems with clear responsibilities:

| Area | Responsibility |
| --- | --- |
| `Scenes` | High-level scene lifecycle and screen flow |
| `Gameplay/Services` | Runtime gameplay systems such as bullets, enemies, spawning, boss flow, particles, rendering, and time |
| `Gameplay/Entities` | Player ship, enemies, bullets, particles, emitters, and path blocks |
| `Core` | Reusable lower-level building blocks for input, graphics, physics, and scenes |
| `Data` | Config models, DTOs, and save data |
| `Services` | Global services for content, scene switching, screen scaling, audio, settings, saves, and serialization |
| `Ui` | Gum UI factories, panels, and custom controls |
| `Content/configs` | JSON-driven gameplay and presentation tuning |
| `MonoBulletHell.Tests` | NUnit test project for test coverage |

The main runtime composition happens in `CompositionRoot`, where global services are registered once and gameplay services are scoped per scene. This keeps scene-specific state isolated while allowing shared services such as content, settings, sound, and screen rendering to remain available across the application.

### Runtime Flow

```text
Program
  -> MonoBulletHellGame
      -> CompositionRoot
          -> Global services
          -> SceneService
              -> TitleScene
              -> GameplayScene
                  -> Gameplay services
                      -> EnemySpawnService
                      -> EnemyService
                      -> BossService
                      -> BulletService
                      -> ParticleService
                      -> RenderService
```

`MonoBulletHellGame` owns the MonoGame lifecycle and delegates update/draw work to the active scene. `SceneService` handles scene transitions and creates scene-scoped services through LightInject, keeping gameplay state isolated from menu state.

## Technical Details

- Language: C#
- Game framework: MonoGame
- UI: Gum.MonoGame
- DI: LightInject
- Serialization: Newtonsoft.Json
- Tests: NUnit

## Testing

The solution includes `MonoBulletHell.Tests`, an NUnit test project that references the main game project. Current coverage focuses on math in `GameMathHelper`.

## Content Configuration

Most gameplay tuning lives in JSON files under `MonoBulletHell/Content/configs`:

| File | Purpose |
| --- | --- |
| `gameConfig.json` | Player tuning and color palette |
| `levelConfig.json` | Enemy waves and boss setup |
| `enemies.json` | Enemy health, sprite, and collider settings |
| `paths.json` | Movement paths and path behavior |
| `emitters.json` | Bullet pattern definitions |

This makes it possible to iterate on wave design, enemy behavior, colors, and boss stages without changing gameplay code.

## Notes

This project demonstrates:

- Building a complete game loop in MonoGame.
- Designing a modular gameplay architecture.
- Managing content loading and validation.
- Separating configuration data from runtime systems.
- Handling scene lifecycle, UI flow, input, audio, rendering, and persistence.
- Using object pooling for high-frequency gameplay entities.

## Assets And Licensing

Source code: MIT License

Audio: 
- Kenney (https://kenney.nl)
- fmceretta (https://freesound.org/people/fmceretta/)
- FoolBoyMedia (https://freesound.org/people/FoolBoyMedia/)
