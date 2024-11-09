using System.Drawing;
using System.Drawing.Imaging;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Buffer = SharpDX.Direct3D11.Buffer;
using System.Runtime.InteropServices;
using DiscordAudioStream.Shaders;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;

public abstract class DirectXCapture : CaptureSource
{
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    private struct Vertex
    {
        [FieldOffset(0)]
        public float PositionX;
        [FieldOffset(4)]
        public float PositionY;
        [FieldOffset(8)]
        public float PositionZ;
        [FieldOffset(12)]
        public float PositionW;
        [FieldOffset(16)]
        public float TexCoordX;
        [FieldOffset(20)]
        public float TexCoordY;

        public Vertex(float x, float y, float z, float w, float u, float v)
        {
            PositionX = x;
            PositionY = y;
            PositionZ = z;
            PositionW = w;
            TexCoordX = u;
            TexCoordY = v;
        }
    }

    public Func<Rectangle>? CustomAreaCrop { get; set; }

    protected Bitmap TextureToBitmap(Texture2D texture, Device d3dDevice)
    {
        Rectangle copiedArea = CustomAreaCrop?.Invoke() ?? new(0, 0, texture.Description.Width, texture.Description.Height);
        int width = copiedArea.Width;
        int height = copiedArea.Height;

        using Texture2D screenTexture = CreateReadableTexture(texture.Description.Width, texture.Description.Height, d3dDevice);
        // Copy resource into memory that can be accessed by the CPU
        d3dDevice.ImmediateContext.CopyResource(texture, screenTexture);
        DataBox sourceData = d3dDevice.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);

        // Create new Bitmap
        Bitmap bmp = new(width, height, PixelFormat.Format32bppArgb);
        Rectangle boundsRect = new(0, 0, width, height);
        BitmapData destinationData = bmp.LockBits(boundsRect, ImageLockMode.WriteOnly, bmp.PixelFormat);

        nint sourcePtr =
            sourceData.DataPointer
            + copiedArea.Top * sourceData.RowPitch
            + copiedArea.Left * 4;
        nint destinationPtr = destinationData.Scan0;
        for (int y = 0; y < height; y++)
        {
            // Copy a single line
            Utilities.CopyMemory(destinationPtr, sourcePtr, width * 4);

            // Advance pointers
            sourcePtr += sourceData.RowPitch;
            destinationPtr += destinationData.Stride;
        }

        // Release source and dest locks
        bmp.UnlockBits(destinationData);
        d3dDevice.ImmediateContext.UnmapSubresource(screenTexture, 0);

        return bmp;
    }

    protected static Texture2D ScaleTexture(Texture2D sourceTexture, Device device)
    {
        DeviceContext context = device.ImmediateContext;
        int newWidth = sourceTexture.Description.Width / 2;
        int newHeight = sourceTexture.Description.Height / 2;

        ShaderResourceView sourceSRV = new(device, sourceTexture);

        Texture2DDescription scaledTextureDesc = new()
        {
            Width = newWidth,
            Height = newHeight,
            MipLevels = 1,
            ArraySize = 1,
            Format = sourceTexture.Description.Format,
            SampleDescription = new(1, 0),
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
            CpuAccessFlags = CpuAccessFlags.None,
            OptionFlags = ResourceOptionFlags.None,
        };

        Texture2D scaledTexture = new(device, scaledTextureDesc);

        RenderTargetView renderTargetView = new(device, scaledTexture);

        context.OutputMerger.SetRenderTargets(renderTargetView);

        context.Rasterizer.SetViewport(0, 0, newWidth, newHeight);
        context.Rasterizer.State = new RasterizerState(device, new RasterizerStateDescription()
        {
            FillMode = FillMode.Solid,
            CullMode = CullMode.None, // Disable culling
            IsFrontCounterClockwise = false,
            DepthBias = 0,
            DepthBiasClamp = 0,
            SlopeScaledDepthBias = 0,
            IsDepthClipEnabled = true,
            IsScissorEnabled = false,
            IsMultisampleEnabled = false,
            IsAntialiasedLineEnabled = false,
        });

        byte[] vertexShaderBytecode = ShaderCache.GetShader("rescaleVertexShader");
        VertexShader vertexShader = new(device, vertexShaderBytecode);

        byte[] pixelShaderBytecode = ShaderCache.GetShader("rescalePixelShader");
        PixelShader pixelShader = new(device, pixelShaderBytecode);

        context.VertexShader.Set(vertexShader);
        context.PixelShader.Set(pixelShader);

        InputLayout inputLayout = new(
            device,
            ShaderSignature.GetInputSignature(vertexShaderBytecode),
            new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0)
            }
        );
        context.InputAssembler.InputLayout = inputLayout;

        Vertex[] vertices = new[]
        {
            new Vertex(-1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f),
            new Vertex(1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f),
            new Vertex(-1.0f, -1.0f, 0.0f, 1.0f, 0.0f, 1.0f),
            new Vertex(1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 1.0f)
        };

        Buffer vertexBuffer = Buffer.Create(device, BindFlags.VertexBuffer, vertices);
        context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<Vertex>(), 0));

        ushort[] indices = new ushort[] { 0, 1, 2, 2, 1, 3 };
        Buffer indexBuffer = Buffer.Create(device, BindFlags.IndexBuffer, indices);
        context.InputAssembler.SetIndexBuffer(indexBuffer, Format.R16_UInt, 0);
        context.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

        RasterizerStateDescription rasterizerDesc = new()
        {
            CullMode = CullMode.None,
            FillMode = FillMode.Solid,
        };
        RasterizerState rasterizerState = new(device, rasterizerDesc);
        context.Rasterizer.State = rasterizerState;

        context.PixelShader.SetShaderResource(0, sourceSRV);

        SamplerStateDescription samplerDesc = new()
        {
            Filter = Filter.MinMagMipLinear,
            AddressU = TextureAddressMode.Clamp,
            AddressV = TextureAddressMode.Clamp,
            AddressW = TextureAddressMode.Clamp,
            ComparisonFunction = Comparison.Always,
            MinimumLod = 0,
            MaximumLod = float.MaxValue,
        };
        SamplerState samplerState = new(device, samplerDesc);
        context.PixelShader.SetSampler(0, samplerState);

        context.DrawIndexed(6, 0, 0);

        vertexShader.Dispose();
        pixelShader.Dispose();
        inputLayout.Dispose();
        vertexBuffer.Dispose();
        sourceSRV.Dispose();
        renderTargetView.Dispose();
        samplerState.Dispose();

        return scaledTexture;
    }

    private static Texture2D CreateReadableTexture(int width, int height, Device d3dDevice)
    {
        Texture2DDescription texture2DDescription = new()
        {
            CpuAccessFlags = CpuAccessFlags.Read,
            BindFlags = BindFlags.None,
            Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
            Width = width,
            Height = height,
            OptionFlags = ResourceOptionFlags.None,
            MipLevels = 1,
            ArraySize = 1,
            SampleDescription = { Count = 1, Quality = 0 },
            Usage = ResourceUsage.Staging
        };

        return new Texture2D(d3dDevice, texture2DDescription);
    }
}
