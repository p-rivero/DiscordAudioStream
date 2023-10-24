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

using SharpDX.Direct3D;
using SharpDX.Direct3D11;

using Windows.Graphics.DirectX.Direct3D11;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace Composition.WindowsRuntimeHelpers;

public static class Direct3D11Helper
{
    private static Guid ID3D11Device = new("db6f6ddb-ac77-4e88-8253-819df9bbf140");
    private static Guid ID3D11Texture2D = new("6f15aaf2-d208-4e89-9ab4-489535d34f9c");

    [ComImport]
    [Guid("A9B3D012-3DF2-4EE3-B8D1-8695F457D3C1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComVisible(true)]
    private interface IDirect3DDxgiInterfaceAccess
    {
        IntPtr GetInterface([In] ref Guid iid);
    };

    [DllImport(
        "d3d11.dll",
        EntryPoint = "CreateDirect3D11DeviceFromDXGIDevice",
        SetLastError = true,
        CharSet = CharSet.Unicode,
        ExactSpelling = true,
        CallingConvention = CallingConvention.StdCall
    )]
    private static extern HRESULT CreateDirect3D11DeviceFromDXGIDevice(nint dxgiDevice, out nint graphicsDevice);

    public static IDirect3DDevice CreateDevice(bool useWARP = false)
    {
        DriverType driverType = useWARP ? DriverType.Software : DriverType.Hardware;
        Device d3dDevice = new(driverType, DeviceCreationFlags.BgraSupport);
        return CreateDirect3DDeviceFromSharpDXDevice(d3dDevice);
    }

    public static IDirect3DDevice CreateDirect3DDeviceFromSharpDXDevice(Device d3dDevice)
    {
        using SharpDX.DXGI.Device3 dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device3>();
        // Wrap the native device using a WinRT interop object.
        CreateDirect3D11DeviceFromDXGIDevice(dxgiDevice.NativePointer, out IntPtr pUnknown).AssertSuccess("Failed to create Direct3D11 device");
        IDirect3DDevice? device = Marshal.GetObjectForIUnknown(pUnknown) as IDirect3DDevice;
        Marshal.Release(pUnknown);

        if (device == null)
        {
            throw new ExternalException("CreateDirect3D11DeviceFromDXGIDevice returned null");
        }
        return device;
    }

    public static Device CreateSharpDXDevice(IDirect3DDevice device)
    {
        IDirect3DDxgiInterfaceAccess access = (IDirect3DDxgiInterfaceAccess)device;
        IntPtr d3dPointer = access.GetInterface(ID3D11Device);
        return new Device(d3dPointer);
    }

    public static Texture2D CreateSharpDXTexture2D(IDirect3DSurface surface)
    {
        IDirect3DDxgiInterfaceAccess access = (IDirect3DDxgiInterfaceAccess)surface;
        IntPtr d3dPointer = access.GetInterface(ID3D11Texture2D);
        return new Texture2D(d3dPointer);
    }
}
