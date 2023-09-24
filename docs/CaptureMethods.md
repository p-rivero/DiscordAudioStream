**Important:** Please note that "Capture all windows" and "Custom Area" only support `BitBlt` and ignore the selected capture method.

# Full-screen capture

## DXGI Duplication

This is the recommended full-screen method if you are using Windows 8 or Windows 10.

**Pros:**

- GPU-accelerated (fast)

**Cons:**

- If "Capture cursor" is enabled, the cursor is painted using the CPU. This can make it slightly slower than `Windows 10`, but in most cases it should not be noticeable.
- Might cause problems when using more than 1 GPU (not tested)
- Requires Windows 8 or higher.


## Windows 10

This is the recommended full-screen method if you are using Windows 11 (or Windows 10 if you don't mind the yellow border).

**Pros:**

- GPU-accelerated (fast).
- The cursor is rendered using the GPU (slightly faster than `DXGI Duplication`).

**Cons:**

- Only available in Windows 10 (version 1903 or higher) and Windows 11.
- In Windows 10, a yellow border may be displayed around the captured display. This border is disabled in Windows 11.


## BitBlt

Use this method if none of the previous ones work.

**Pros:**

- Available since Windows XP.

**Cons:**

- Capture is performed using the CPU (slow).

---

# Window capture

## Windows 10

This is the recommended window capture method if you are using Windows 11 or Windows 10.

**Pros:**

- GPU-accelerated (fast)
- Stable
- Can capture windows that are partially outside the screen or occluded by other windows (it cannot capture minimized windows).

**Cons:**

- Only available in Windows 10 (version 1903 or higher) and Windows 11.
- In Windows 10, a yellow border may be displayed around the captured window. This border is disabled in Windows 11.


## PrintWindow

Use this method if you are using Windows 7, Windows 8 or Windows 8.1.

> **Warning:** Some programs may flash or flicker while using this capture method. Usually it's not very noticeable, but I strongly discourage using `PrintWindow` if you are sensitive to flashes or suffer from epilepsy.

**Pros:**

- Can capture windows that are partially outside the screen or occluded by other windows (it cannot capture minimized windows).

**Cons:**

- Capture is performed using the CPU (slow).
- Requires Windows 8.1 or higher for the best results. Windows 7 and Windows 8 will work fine, but some elements may be displayed incorrectly.
- May cause flickering (see warning above).


## BitBlt

Use this method if none of the previous ones work.

**Pros:**

- Available since Windows XP.

**Cons:**

- Capture is performed using the CPU (slow).
- Cannot capture windows that are partially outside the screen or occluded. When using this method, the captured window will be brought to the front (other windows cannot be placed on top).
