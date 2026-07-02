using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SignTrainer.EditorTools
{
    public static class URPPipelineSetup
    {
        const string PipelinePath    = "Assets/SignTrainerURPAsset.asset";
        const string RendererPath    = "Assets/New Custom Universal Renderer Data.asset";
        const string RendererPath2   = "Assets/New Custom Universal Renderer Data 1.asset";

        [MenuItem("SignTrainer/Setup/Fix URP Pipeline Asset")]
        public static void FixURPPipeline()
        {
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(RendererPath)
                            ?? AssetDatabase.LoadAssetAtPath<UniversalRendererData>(RendererPath2);

            if (rendererData == null)
            {
                Debug.LogError("[URPPipelineSetup] No UniversalRendererData found. " +
                               "Right-click in Project → Create → Rendering → URP Asset (with Universal Renderer) manually.");
                return;
            }

            var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelinePath);
            if (pipeline == null)
            {
                pipeline = UniversalRenderPipelineAsset.Create(rendererData);
                AssetDatabase.CreateAsset(pipeline, PipelinePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelinePath);
            }

            GraphicsSettings.renderPipelineAsset = pipeline;
            QualitySettings.renderPipeline       = pipeline;
            EditorUtility.SetDirty(pipeline);
            AssetDatabase.SaveAssets();

            Debug.Log("[URPPipelineSetup] URP pipeline asset created and assigned. Pink materials should now display correctly.");
        }
    }
}
