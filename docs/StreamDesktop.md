# Stream the entire desktop with audio

1. Decide which programs you want to share the audio from.

    **Tip:** your answer should never be *"all of them"*. At least, you should exclude Discord (otherwise, the viewers of your stream will hear themselves).
    
    > **Update:** Discord now seems to create 2 outputs: one for the call audio (voices of the call members) and another for the media audio (videos sent in a text channel). This means that you are now able to stream the Discord window (for example, to group watch videos from a text channel) without the viewers hearing themselves. You need to determine which of the 2 outputs is the media audio and only share that one (do not include the call audio).
    
2. Open the Windows volume mixer. You can do this from the DiscordAudioStream window by using `Ctrl+V` or clicking the mixer icon: [<img src="img/mixer-light.png" align="top"></img>](#gh-light-mode-only)[<img src="img/mixer-dark.png" align="top"></img>](#gh-dark-mode-only).

3. *For each* of the programs whose audio you want to share, change its *output* device from "Default" to another device (one that you are not currently using). For example, if you have Steam installed you should have a virtual audio device called "Steam Streaming Speakers" that you can use (unless you are using it for other purposes).

    - Set the output of *all* the desired programs to **the same** audio device.
    - Don't worry if you stop hearing the audio from the programs you are sharing. Later you will be able to hear them again.
    - Make sure that there are no other programs outputting audio to the device you selected. Everything that gets sent to this device will be shared with your viewers.

    <br>
    <details>
    <summary>I want to use an audio capture card</summary>

    Capture cards and microphones are audio input devices, but DiscordAudioStream only shows output devices by default. Open DiscordAudioStream settings [<img src="img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="img/settings-dark.png" align="top"></img>](#gh-dark-mode-only) > Debug and enable "Show audio input devices". You should now see your capture cards and microphones in the audio capture dropdown (input devices have the `[IN]` prefix).  
    Keep in mind that Discord already shares your microphone when you enter a call, so you don't need to capture it in DiscordAudioStream.

    </details>
    <details>
    <summary>I don't have any unused audio device!</summary>

    You can use [VB-CABLE](https://vb-audio.com/Cable/), which creates 2 virtual audio devices: `CABLE Input` (virtual output device) and `CABLE Output` (virtual microphone). Set the output of the programs you want to capture to `CABLE Input` and try to capture `CABLE Input (VB-Audio Virtual Cable)` in DiscordAudioStream (see step 4 below).  
    When you start capturing the audio in step 7, you may encounter an error. If this happens, you will need to open DiscordAudioStream settings [<img src="img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="img/settings-dark.png" align="top"></img>](#gh-dark-mode-only) > Debug and enable "Show audio input devices". Go back to the audio capture dropdown and capture `[IN] CABLE Output (VB-Audio Virtual Cable)` instead.

    </details>
    
    ![Change audio device in volume mixer](img/audio-device.png)

4. In the *Audio capture input* dropdown, select the (previously unused) audio device that you have chosen in step 3.

5. (Optional) You can use the *Video capture scale* dropdown to change the size of the output window (see step 7), in order to make it easier to move around.
   - For the best visual results, I strongly recommend using the default value (720p), or 1080p for Discord Nitro users. Using a small scale can make the Discord stream look blurry.
   - Changing this setting **will NOT** make the video capture smoother or more efficient: this is only a downscaling performed *after* the video has been captured at full size.

6. In the *Video capture area* dropdown, select the screen or window you want to share.

7. Click the *Start Stream* button. This will create a new window. You should now be able to hear the audio from the programs you stopped hearing in step 3.

   ![Video and audio dropdowns](img/dropdowns.png)

8. In Discord, select "Share Your Screen". This will show a list of open windows. Select the window called "Discord Audio Stream" (the output window that was created in step 7).

    - In DiscordAudioStream settings [<img src="img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="img/settings-dark.png" align="top"></img>](#gh-dark-mode-only), you can change the *Stream title* (the default value is "Discord Audio Stream"). If you have changed this setting, select the window with the title you have chosen instead. Discord usually doesn't show this title to the viewers, so you can set it to something like "⚠️THIS⚠️" to make the window easier to find.

9. You are now sharing your screen with the audio from the selected programs. When you are done, you can close DiscordAudioStream.

    - **Very important:** remember to restore the output device of your programs to "Default". Otherwise, you won't be able to hear their audio unless DiscordAudioStream is running.

**Keep in mind:**
   
- If you minimize the DiscordAudioStream window (created in step 7), your Discord stream will be paused. You may want to hide this window behind other windows, without minimizing it.

- You might see 2 cursors in your stream:
  - Cursor added by DiscordAudioStream (if the *Show cursor* option is enabled): shows the correct mouse location.
  - Cursor added by Discord: shows the mouse location relative to the streamed DiscordAudioStream window.

  The solution would be to disable cursor capture in Discord, but this setting currently doesn't exist. You can work around this by moving the DiscordAudioStream window mostly off-screen, where the cursor won't be hovering it (first go to DiscordAudioStream settings [<img src="img/settings-light.png" align="top"></img>](#gh-light-mode-only)[<img src="img/settings-dark.png" align="top"></img>](#gh-dark-mode-only) > Debug and make sure that "Force screen redraw" is enabled).

