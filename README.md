# Discord Audio Stream

Windows utility that allows Discord to stream (with audio!) the entire desktop or a specific window (including an OBS composition).

Made out of necessity and continuous frustration. Built with WinForms (C#).


<p align="center">
    <img alt="Logo" src="docs/img/logo-100.png">
</p>
<p align="center">
    <a href="https://github.com/p-rivero/DiscordAudioStream/releases">
        <img alt="Total downloads" src="https://img.shields.io/github/downloads/p-rivero/DiscordAudioStream/total?label=total%20downloads">
    </a>
    <a href="https://github.com/p-rivero/DiscordAudioStream/releases">
        <img alt="Downloads of latest release" src="https://img.shields.io/github/downloads/p-rivero/DiscordAudioStream/latest/total?label=downloads%20(latest%20release)">
    </a>
    <br>
    <a href="https://github.com/p-rivero/DiscordAudioStream/issues">
        <img alt="Issues" src="https://img.shields.io/github/issues/p-rivero/DiscordAudioStream">
    </a>
    <a href="https://github.com/p-rivero/DiscordAudioStream/pulls">
        <img alt="Pull requests" src="https://img.shields.io/github/issues-pr/p-rivero/DiscordAudioStream">
    </a>
    <a href="https://github.com/p-rivero/DiscordAudioStream/blob/master/LICENSE">
        <img alt="License" src="https://img.shields.io/github/license/p-rivero/DiscordAudioStream">
    </a>
</p>



## Who is this for?

If you use Discord regularly, you may be used to screen-sharing (streaming) a specific window. You may also have tried to stream the entire screen, only to realize that your friends cannot hear any audio from your desktop.

Similarly, you may be an advanced user who wants to step up your Discord streams by using OBS. While you can stream the OBS video output easily, it's not possible to stream the audio without configuring an advanced audio setup.

If any of these are true, this tool may be for you.


## Does it only work for Discord?

Not at all, you can use this tool with any video conferencing software that allows screen-sharing a specific window.

However, keep in mind that this utility has been built and tested only for Discord, so some of the steps below will be different and you may encounter errors.


## Does it only work on Windows?

Yes, this program uses Windows APIs that are not available on MacOS or Linux.


## Why is it licensed under GPLv3?

This program is a fork of [quick-screen-recorder](https://github.com/ModuleArt/quick-screen-recorder), by [ModuleArt](https://github.com/ModuleArt), which is licensed under the GPLv3 license.

Unfortunately, GPLv3 forces any derivative work to also be licensed under GPLv3, which means I'm unable to offer a more permissive license.


# How do I use it?

First, download the program from the [GitHub releases page](https://github.com/p-rivero/DiscordAudioStream/releases). You should be able to execute it directly without installing anything.

The following steps depend on what you want to do.

<details>
<summary><b>Stream the entire desktop with audio</b></summary>
<br>

1. Decide which programs you want to share the audio from.

    **Tip:** your answer should never be *"all of them"*. At least, you should exclude Discord (otherwise, the viewers of your stream will hear themselves).
    
    > **Update:** Discord now seems to create 2 outputs: one for the call audio (voices of the call members) and another for the media audio (videos sent in a text channel). This means that you are now able to stream the Discord window (for example, to group watch videos from a text channel) without the viewers hearing themselves. You need to determine which of the 2 outputs is the media audio and only share that one (do not include the call audio).
    
2. Open the Windows volume mixer. You can do this from the DiscordAudioStream window by using `Ctrl+V` or clicking the mixer icon: [<img src="docs/img/mixer-light.png" align="top"></img>](#gh-light-mode-only)[<img src="docs/img/mixer-dark.png" align="top"></img>](#gh-dark-mode-only).

3. *For each* of the programs whose audio you want to share, change its *output* device from "Default" to another device (one that you are not currently using). For example, if you have Steam installed you should have a virtual audio device called "Steam Streaming Speakers" that you can use (unless you are using it for other purposes).

    - Set the output of *all* the desired programs to **the same** audio device.
    - Don't worry if you stop hearing the audio from the programs you are sharing. Later you will be able to hear them again.
    - Make sure that there are no other programs outputting audio to the device you selected. Everything that gets sent to this device will be shared with your viewers.

    <br>
    <details>
    <summary>I want to use an audio capture card</summary>

    Capture cards and microphones are audio input devices, but DiscordAudioStream only shows output devices by default. Open DiscordAudioStream settings [<img src="docs/img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="docs/img/settings-dark.png" align="top"></img>](#gh-dark-mode-only) > Debug and enable "Show audio input devices". You should now see your capture cards and microphones in the audio capture dropdown (input devices have the `[IN]` prefix).  
    Keep in mind that Discord already shares your microphone when you enter a call, so you don't need to capture it in DiscordAudioStream.

    </details>
    <details>
    <summary>I don't have any unused audio device!</summary>

    You can use [VB-CABLE](https://vb-audio.com/Cable/), which creates 2 virtual audio devices: `CABLE Input` (virtual output device) and `CABLE Output` (virtual microphone). Set the output of the programs you want to capture to `CABLE Input` and try to capture `CABLE Input (VB-Audio Virtual Cable)` in DiscordAudioStream (see step 4 below).  
    When you start capturing the audio in step 7, you may encounter an error. If this happens, you will need to open DiscordAudioStream settings [<img src="docs/img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="docs/img/settings-dark.png" align="top"></img>](#gh-dark-mode-only) > Debug and enable "Show audio input devices". Go back to the audio capture dropdown and capture `[IN] CABLE Output (VB-Audio Virtual Cable)` instead.

    </details>
    
    ![Change audio device in volume mixer](docs/img/audio-device.png)

4. In the *Audio capture input* dropdown, select the (previously unused) audio device that you have chosen in step 3.

5. (Optional) You can use the *Video capture scale* dropdown to change the size of the output window (see step 7), in order to make it easier to move around.
   - For the best visual results, I strongly recommend using the default value (720p), or 1080p for Discord Nitro users. Using a small scale can make the Discord stream look blurry.
   - Changing this setting **will NOT** make the video capture smoother or more efficient: this is only a downscaling performed *after* the video has been captured at full size.

6. In the *Video capture area* dropdown, select the screen or window you want to share.

7. Click the *Start Stream* button. This will create a new window. You should now be able to hear the audio from the programs you stopped hearing in step 3.

   ![Video and audio dropdowns](docs/img/dropdowns.png)

8. In Discord, select "Share Your Screen". This will show a list of open windows. Select the window called "Discord Audio Stream" (the output window that was created in step 7).

    - In DiscordAudioStream settings [<img src="docs/img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="docs/img/settings-dark.png" align="top"></img>](#gh-dark-mode-only), you can change the *Stream title* (the default value is "Discord Audio Stream"). If you have changed this setting, select the window with the title you have chosen instead. Discord usually doesn't show this title to the viewers, so you can set it to something like "⚠️THIS⚠️" to make the window easier to find.

9. You are now sharing your screen with the audio from the selected programs. When you are done, you can close DiscordAudioStream.

    - **Very important:** remember to restore the output device of your programs to "Default". Otherwise, you won't be able to hear their audio unless DiscordAudioStream is running.

**Keep in mind:**
   
- If you minimize the DiscordAudioStream window (created in step 7), your Discord stream will be paused. You may want to hide this window behind other windows, without minimizing it.

- You might see 2 cursors in your stream:
  - Cursor added by DiscordAudioStream (if the *Show cursor* option is enabled): shows the correct mouse location.
  - Cursor added by Discord: shows the mouse location relative to the streamed DiscordAudioStream window.

  The solution would be to disable cursor capture in Discord, but this setting currently doesn't exist. You can work around this by moving the DiscordAudioStream window mostly off-screen, where the cursor won't be hovering it (first go to DiscordAudioStream settings [<img src="docs/img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="docs/img/settings-dark.png" align="top"></img>](#gh-dark-mode-only) > Debug and make sure that "Force screen redraw" is enabled).

</details>
<br>

<details>
<summary><b>Stream to Discord using OBS</b></summary>
<br>

First, follow steps 1-4 above (expand the "Stream the entire desktop with audio" dropdown).

> **Important:** the program for which you want to share the audio **is NOT** OBS (OBS doesn't actually output any audio). Instead, you must identify which programs you are recording and share their audio (change their output device) separately.
> 
> For example, if you have added the following sources to your OBS composition:
> - The game you are playing
> - Your webcam + microphone
> - Stream alerts (with audio), using Google Chrome
> 
> Then, the programs for which you want to share the audio are 1. the game and 2. Google Chrome. Do not worry about the microphone, since Discord already shares it when you enter a call.

Now follow these steps:

5. In OBS, right-click the preview and select "Windowed Projector (Preview)". This will create a new window. You can now minimize OBS (but not the preview window). You may want to make this window slightly bigger, in order to improve video quality.

6. Open DiscordAudioStream (this program). In the *Video capture area* dropdown, select the window "Windowed Projector (Preview)" (the one that was created in step 5).

7. Click the *Start Stream* button. This will create a new window. You should now be able to hear the audio from the programs you stopped hearing in step 3.

8. In Discord, select "Share Your Screen". This will show a list of open windows. Select the window called "Discord Audio Stream" (the one that was created in step 7).

    - In DiscordAudioStream settings [<img src="docs/img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="docs/img/settings-dark.png" align="top"></img>](#gh-dark-mode-only), you can change the *Stream title* (the default value is "Discord Audio Stream"). If you have changed this setting, select the window with the title you have chosen instead. Discord usually doesn't show this title to the viewers, so you can set it to something like "⚠️THIS⚠️" to make the window easier to find.

9. You are now sharing your OBS composition with the audio from the selected programs. When you are done, you can close DiscordAudioStream.

    - **Very important:** remember to restore the output device of your programs to "Default". Otherwise, you won't be able to hear their audio unless DiscordAudioStream is running.

**Keep in mind:**
   
- If you minimize the DiscordAudioStream window (created in step 7), your Discord stream will be paused. You may want to hide this window behind other windows, without minimizing it.

- You might see 2 cursors in your stream:  
  - Cursor added by DiscordAudioStream (if the *Show cursor* option is enabled): shows the correct mouse location.
  - Cursor added by Discord: shows the mouse location relative to the streamed DiscordAudioStream window.
  
  The solution would be to disable cursor capture in Discord, but this setting currently doesn't exist. You can work around this by moving the DiscordAudioStream window mostly off-screen, where the cursor won't be hovering it (first go to DiscordAudioStream settings [<img src="docs/img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="docs/img/settings-dark.png" align="top"></img>](#gh-dark-mode-only) > Debug and make sure that "Force screen redraw" is enabled).

</details>
<br>

---

## Changing the capture method

You can change which API is used for capturing video by going to Settings [<img src="docs/img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="docs/img/settings-dark.png" align="top"></img>](#gh-dark-mode-only) > Capture.

Read [this document](/docs/CaptureMethods.md) for more information about the pros and cons of each method. 

## Reporting a bug/crash

If the program crashes, it will attempt to generate a trace file called `DiscordAudioStream_stack_trace.txt`. This file will be located in the same folder as your executable (`DiscordAudioStream.exe`).
Create a [new issue](https://github.com/p-rivero/DiscordAudioStream/issues/new) and make sure to attach your trace file to help me solve your crash more easily.

If the stack trace is not generated automatically, go to Settings [<img src="docs/img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="docs/img/settings-dark.png" align="top"></img>](#gh-dark-mode-only) > Debug and enable *Output log file*.
Close and reopen the program for this change to take effect.

When this option is active, your actions inside the program are logged to a file called `DiscordAudioStream_log.txt` (also located in the same folder as the executable).
Reproduce the crash (with the least number of steps, if possible) and send me the log file to help me locate the source of the crash. 

> **Privacy notice:** The logged information may include your Windows version, the resolution of your screen(s) and the name of the audio devices you are using. For privacy reasons, the titles/names of your opened windows/programs are **NOT** logged. The captured video/audio is **NOT** logged either. 
> 
> The logs never leave your computer unless you send them to me on GitHub, so feel free to read them before sending them and manually remove any unwanted information.
