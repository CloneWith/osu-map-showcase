# osu.Game.Rulesets.MapShowcase

An automatic **mappool showcase** tool for [osu!lazer](https://github.com/ppy/osu), packaged as a ruleset.

## Showcase profiles

Profiles are plain JSON files under `showcase/` in your osu! data directory. In default, it is `%APPDATA%/osu/showcase/<name>.json`.

The showcase references some image resources that can be customized:

| Resource | File path | Usage |
| --- | --- | --- |
| Transition background | `<profile>.json/transition` | Full-screen background shown while the wipe transition plays between showcase screens. |
| Tournament logo | `<tournament name>` | Main logo displayed on the intro and outro screens. Falls back to osu!'s own logo when missing. |
| Original badge icon | `<tournament name>/original-badge` | Icon on the left of the "Tournament Original" badge at the top-right during gameplay. |
| Mod icon | `<tournament name>/<mod><index>` | Badge drawn on each beatmap entry in the showcase editor list, e.g. `NM1`, `HD2`, `TB`. |
| Chess piece icon | `Board/<mod><index>` (falls back to `Board/<mod>`) | Piece shown on the map pool card for that mod. |
| Tiebreaker piece icon | `Board/chess-win` | Piece shown for tiebreaker (`TB`) entries in the map pool. |

All paths are relative to the `showcase/` directory mentioned above.

## Keyboard Shortcuts

You can use these shortcuts to control the showcase:

| Action | Default |
| --- | --- |
| Start showcase | `Ctrl`+`Enter` |
| Replay current | `Ctrl`+`Alt`+`R` |
| Show previous | `Ctrl`+`Alt`+`←` |
| Show next | `Ctrl`+`Alt`+`→` |
| Toggle auto showcase | `Ctrl`+`A` |
| Exit showcase | `Ctrl`+`Alt`+`Q` |

They can be changed in the ruleset's key bindings panel.

## Building

.NET 10 SDK should be available in your development environment.

```sh
dotnet build osu.Game.Rulesets.MapShowcase.sln -c Release
```

Then copy `osu.Game.Rulesets.MapShowcase.dll` (and its `.pdb`, if you want stack traces) into the client's
`Rulesets/` directory and restart osu!.

## Licensing

MIT. See [LICENSE](LICENSE).
