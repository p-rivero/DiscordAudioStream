using System.Runtime.InteropServices;

using SharpDX;
using SharpDX.Direct3D11;
using DiscordAudioStream.Shaders;

using Buffer = SharpDX.Direct3D11.Buffer;

namespace DiscordAudioStream.VideoCapture.CaptureStrategy;
public class Rescaler : IDisposable
{
    private readonly Device device;
    private readonly Buffer vertexBuffer;
    private readonly VertexShader vertexShader;
    private readonly PixelShader pixelShader;
    private readonly InputLayout inputLayout;
    private readonly SamplerState samplerState;
    private readonly RasterizerState rasterizerState;

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

    public Rescaler(Device device)
    {
        this.device = device;

        Vertex[] vertices = new[]
        {
            new Vertex(-1, 1, 0, 1, 0, 0),
            new Vertex(1, 1, 0, 1, 1, 0),
            new Vertex(-1, -1, 0, 1, 0, 1),
            new Vertex(1, -1, 0, 1, 1, 1)
        };
        vertexBuffer = Buffer.Create(device, BindFlags.VertexBuffer, vertices);

        byte[] vertexShaderBytecode = ShaderCache.GetShader("rescaleVertexShader");
        byte[] pixelShaderBytecode = ShaderCache.GetShader("rescalePixelShader");
        vertexShader = new(device, vertexShaderBytecode);
        pixelShader = new(device, pixelShaderBytecode);

        inputLayout = new(device, vertexShaderBytecode, new[]
            {
                new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32A32_Float, 0, 0),
                new InputElement("TEXCOORD", 0, SharpDX.DXGI.Format.R32G32_Float, 16, 0)
            }
        );

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
        samplerState = new(device, samplerDesc);

        RasterizerStateDescription rasterizerDesc = new()
        {
            CullMode = CullMode.None,
            FillMode = FillMode.Solid,
        };
        rasterizerState = new(device, rasterizerDesc);
    }

    public Texture2D ScaleTexture(Texture2D sourceTexture, double scale)
    {
        DeviceContext context = device.ImmediateContext;
        int newWidth = (int)(sourceTexture.Description.Width * scale);
        int newHeight = (int)(sourceTexture.Description.Height * scale);

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

        using RenderTargetView renderTargetView = new(device, scaledTexture);
        context.OutputMerger.SetRenderTargets(renderTargetView);

        context.Rasterizer.SetViewport(0, 0, newWidth, newHeight);

        context.VertexShader.Set(vertexShader);
        context.PixelShader.Set(pixelShader);

        context.InputAssembler.InputLayout = inputLayout;
        context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<Vertex>(), 0));
        context.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleStrip;

        using ShaderResourceView sourceSRV = new(device, sourceTexture);
        context.PixelShader.SetShaderResource(0, sourceSRV);
        context.PixelShader.SetSampler(0, samplerState);
        context.Rasterizer.State = rasterizerState;
        context.Draw(4, 0);

        return scaledTexture;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            vertexBuffer.Dispose();
            vertexShader.Dispose();
            pixelShader.Dispose();
            inputLayout.Dispose();
            samplerState.Dispose();
            rasterizerState.Dispose();
        }
    }
}
