# JJS AutoPlayer

Private WinForms autoplayer prototype for Roblox Jujutsu Shenanigans piano.

## Run

Open the built executable from:

`JJSAutoplayer/bin/Release/net8.0-windows/win-x64/publish/JJsAutoplayer.exe`

Press **Start**, then focus the Roblox/JJS piano window before the start delay finishes.

## Modes

- **Simple**: plays only the first key in each chord. Use this for basic JJS piano mode.
- **Advanced**: plays full chords. Use chord spread if Roblox misses simultaneous key presses.

## Song Format

Songs are `.txt` files in `JJSAutoplayer/songs`.

```txt
# comments are allowed
Z 180
[ZXC] 350
- 120
Q 0.25
```

- A single key like `Z 180` taps `Z`, then waits 180 ms.
- A chord like `[ZXC] 350` plays multiple keys in Advanced mode.
- A rest like `- 120` waits without pressing keys.
- Delays can be milliseconds (`180`, `180ms`) or seconds (`0.18`).
