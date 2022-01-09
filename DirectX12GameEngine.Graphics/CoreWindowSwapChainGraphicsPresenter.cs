﻿using SharpGen.Runtime;
using Vortice.DXGI;
using Windows.UI.Core;

namespace DirectX12GameEngine.Graphics
{
    public class CoreWindowSwapChainGraphicsPresenter : SwapChainGraphicsPresenter
    {
        private readonly CoreWindow coreWindow;

        public CoreWindowSwapChainGraphicsPresenter(GraphicsDevice device, PresentationParameters presentationParameters, CoreWindow coreWindow)
            : base(device, presentationParameters, CreateSwapChain(device, presentationParameters, coreWindow))
        {
            this.coreWindow = coreWindow;
        }

        private static IDXGISwapChain3 CreateSwapChain(GraphicsDevice device, PresentationParameters presentationParameters, CoreWindow coreWindow)
        {
            SwapChainDescription1 swapChainDescription = new SwapChainDescription1
            {
                Width = presentationParameters.BackBufferWidth,
                Height = presentationParameters.BackBufferHeight,
                SampleDescription = new Vortice.DXGI.SampleDescription(1, 0),
                Stereo = presentationParameters.Stereo,
                BufferUsage = Usage.RenderTargetOutput,
                BufferCount = BufferCount,
                Scaling = Scaling.Stretch,
                SwapEffect = SwapEffect.FlipSequential,
                Format = (Format)presentationParameters.BackBufferFormat.ToNonSrgb(),
                Flags = SwapChainFlags.None,
                AlphaMode = AlphaMode.Unspecified
            };

            DXGI.CreateDXGIFactory2(false, out IDXGIFactory2? factory);
            using ComObject window = new ComObject(coreWindow);
            using IDXGISwapChain1 tempSwapChain = factory!.CreateSwapChainForCoreWindow(device.DirectCommandQueue.NativeCommandQueue, window, swapChainDescription);
            factory.Dispose();

            return tempSwapChain.QueryInterface<IDXGISwapChain3>();
        }
    }
}
