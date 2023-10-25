//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//
//  The MIT License (MIT)
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Graphics.Capture;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace Composition.WindowsRuntimeHelpers;

public static class CaptureHelper
{
    // Passing null pointer to IGraphicsCaptureItemInterop::CreateForMonitor will capture the entire desktop
    // Wasn't able to find documentation for this, but it works
    public static readonly HMONITOR ALL_SCREENS;

    private static readonly Guid GraphicsCaptureItemGuid = new("79C3F95B-31F7-4EC2-A464-632EF5D30760");

    [ComImport]
    [Guid("3628E81B-3CAC-4C60-B7F4-23CE0E0C3356")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    private interface IGraphicsCaptureItemInterop
    {
        IntPtr CreateForWindow([In] IntPtr window, [In] ref Guid iid);
        IntPtr CreateForMonitor([In] IntPtr monitor, [In] ref Guid iid);
    }

    public static GraphicsCaptureItem CreateItemForWindow(HWND hwnd)
    {
        IActivationFactory factory = WindowsRuntimeMarshal.GetActivationFactory(typeof(GraphicsCaptureItem));
        IGraphicsCaptureItemInterop interop = (IGraphicsCaptureItemInterop)factory;
        nint itemPointer = interop.CreateForWindow(hwnd, GraphicsCaptureItemGuid);
        GraphicsCaptureItem? item = Marshal.GetObjectForIUnknown(itemPointer) as GraphicsCaptureItem;
        Marshal.Release(itemPointer);
        if (item == null)
        {
            throw new ExternalException("IGraphicsCaptureItemInterop.CreateItemForWindow failed");
        }
        return item;
    }

    public static GraphicsCaptureItem CreateItemForMonitor(HMONITOR hmon)
    {
        IActivationFactory factory = WindowsRuntimeMarshal.GetActivationFactory(typeof(GraphicsCaptureItem));
        IGraphicsCaptureItemInterop interop = (IGraphicsCaptureItemInterop)factory;
        nint itemPointer = interop.CreateForMonitor(hmon, GraphicsCaptureItemGuid);
        GraphicsCaptureItem? item = Marshal.GetObjectForIUnknown(itemPointer) as GraphicsCaptureItem;
        Marshal.Release(itemPointer);
        if (item == null)
        {
            throw new ExternalException("IGraphicsCaptureItemInterop.CreateForMonitor failed");
        }
        return item;
    }
}
