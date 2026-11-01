# Antlers-Harmonypatch

"Shed antlers should be picked up, not admired."

Fixes antlers that are shed while a creature's chunk is loading. Without this fix, those antlers stay visible on the ground but can never be picked up, and they disappear after reloading the world.

## Features

- Defers antler shedding from chunk loading to the creature's regular game tick, a few seconds later
- Shed antlers can be picked up normally, even if nobody was nearby when the shedding season started
- Lightweight and Vanilla friendly, no new items, blocks, settings or expensive calculations

## Installation

- In Multiplayer it is required for Server-side only. Clients do not need the mod to join.
- In Singleplayer, simply install it like any other mod.

## Compatibility

- Vintage Story 1.22.x, tested on 1.22.7
- Applies to every creature that calls the vanilla "antlergrowth" behavior, including modded deer that make use of it
- If a game update renames the patched method, the mod falls back to vanilla behavior and writes a warning to the server log. A fix would be to find the new method name and patch that in case the bug still appears.

## Known limitations

- Antlers that already became unpickable before installing the mod are not restored. These item entities are not saved with the chunk and disappear when the world is reloaded (different causes are possible).
- Once the bug is fixed in the base game, this mod can be removed
- This fix only works for 1.22.x versions due to the required .NET version and game requirement setup. Comment me if you need a game version that is not included, I'll see what I can do.

## Background

Bug report with reproduction steps: https://github.com/anegostudios/VintageStory-Issues/issues/5746

Thanks to the Antlerutil mod dev for the inspiring previous workaround. You can still use both mods alongside each other! https://mods.vintagestory.at/antlerutilmod#tab-description (tested in Singleplayer)

Thanks to the people who reported this bug previously, reading the findings helped me narrow it down to this point, good job community!

## License

CC BY-NC 4.0 with additional permissions, see the LICENSE file.

---

# [Update-Changelog]:

### Version 1.0.0

- Initial release.

End of file.
