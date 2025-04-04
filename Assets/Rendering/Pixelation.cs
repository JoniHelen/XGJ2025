using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class Pixelation : ScriptableRendererFeature {
    [Serializable]
    public struct Settings {
        [Min(1)] public int VerticalResolution;
    }
    
    [SerializeField] private Settings _settings;
    
    private PixelationRenderPass _pixelationRenderPass;
    
    public override void Create() {
        _pixelationRenderPass = new PixelationRenderPass {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        if (_pixelationRenderPass == null || renderingData.cameraData.cameraType != CameraType.Game) return;
        _pixelationRenderPass.UpdateSettings(_settings);
        renderer.EnqueuePass(_pixelationRenderPass);
    }
    
    private class PixelationRenderPass : ScriptableRenderPass {
        private static readonly string _miniPassName = "Pixelation: Minimize";
        private static readonly string _copyPassName = "Pixelation: Copy";
        
        private TextureDesc _descriptor;
        private Settings _settings;

        public PixelationRenderPass() {
            requiresIntermediateTexture = true;
        }
        
        public void UpdateSettings(Settings settings) => _settings = settings;
        
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {
            var resourceData = frameData.Get<UniversalResourceData>();
            var cameraData = frameData.Get<UniversalCameraData>();

            if (resourceData.isActiveTargetBackBuffer) return;
            
            var cameraTarget = resourceData.activeColorTexture;
            
            _descriptor = renderGraph.GetTextureDesc(cameraTarget);
            _descriptor.depthBufferBits = 0;
            _descriptor.clearBuffer = false;
            _descriptor.name = "_Minimized";
            _descriptor.height = _settings.VerticalResolution;
            _descriptor.width = Mathf.FloorToInt(_settings.VerticalResolution * cameraData.camera.aspect);

            var minimizedTarget = renderGraph.CreateTexture(_descriptor);

            if (!cameraTarget.IsValid() || !minimizedTarget.IsValid()) return;

            renderGraph.AddBlitPass(cameraTarget, minimizedTarget,
                Vector2.one, Vector2.zero,
                filterMode: RenderGraphUtils.BlitFilterMode.ClampNearest, passName: _miniPassName
            );
            
            renderGraph.AddBlitPass(minimizedTarget, cameraTarget,
                Vector2.one, Vector2.zero,
                filterMode: RenderGraphUtils.BlitFilterMode.ClampNearest, passName: _copyPassName
            );
        }
    }
}
