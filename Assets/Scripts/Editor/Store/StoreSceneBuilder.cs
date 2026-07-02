using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static SignTrainer.EditorTools.EnvHelper;

namespace SignTrainer.EditorTools
{
    /// <summary>
    /// Generates a brand-new grocery store scene (Assets/Scenes/Scenario_Store.unity)
    /// with a walk/teleport XR rig. Menu: SignTrainer ▸ Environments ▸ Build Store Scene (New).
    /// </summary>
    public static class StoreSceneBuilder
    {
        const string Menu = "SignTrainer/Environments/Build Store Scene (New)";
        const string MatDir = "Assets/Art/Materials/Store";
        const string ScenePath = "Assets/Scenes/Scenario_Store.unity";

        public const float W = 18f, L = 22f, H = 4.5f, T = 0.2f;

        [MenuItem(Menu)]
        public static void Build()
        {
            if (!EditorUtility.DisplayDialog("Build Store Scene",
                    "This creates a new scene and replaces the current view.\n" +
                    "Save any unsaved work first.\n\nContinue?", "Create Store", "Cancel"))
                return;

            EnsureMatDir(MatDir);
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var palette = StorePalette.Build(MatDir);

            var root = new GameObject("StoreEnvironment");
            Shell(root, palette);
            CeilingLights(root, palette);
            StoreFixtures.Build(root, palette);
            Lighting(root);

            var floor = GameObject.Find("StoreEnvironment/Floor");
            SceneXRRig.Build(new Vector3(0, 0, 1.6f), floor, "StoreXR (Generated)");

            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScene(scene, ScenePath);
            Selection.activeGameObject = root;
            Debug.Log($"[StoreSceneBuilder] Store scene created at {ScenePath}. " +
                      "Add it to Build Settings if you want it in the app flow.");
        }

        static void Shell(GameObject root, StorePalette p)
        {
            Box(root, "Floor", new Vector3(0, -T / 2, L / 2), new Vector3(W, T, L), p.Floor, true);
            Box(root, "Ceiling", new Vector3(0, H + T / 2, L / 2), new Vector3(W, T, L), p.Ceiling, false);
            Box(root, "Wall_L", new Vector3(-W / 2 - T / 2, H / 2, L / 2), new Vector3(T, H + T * 2, L + T * 2), p.Wall, true);
            Box(root, "Wall_R", new Vector3(W / 2 + T / 2, H / 2, L / 2), new Vector3(T, H + T * 2, L + T * 2), p.Wall, true);
            Box(root, "Wall_Far", new Vector3(0, H / 2, L + T / 2), new Vector3(W + T * 2, H, T), p.Wall, true);
            // Near wall with a central entrance gap (x -2..2 open).
            Box(root, "Wall_Near_L", new Vector3(-5.5f, H / 2, -T / 2), new Vector3(7f, H, T), p.Wall, true);
            Box(root, "Wall_Near_R", new Vector3(5.5f, H / 2, -T / 2), new Vector3(7f, H, T), p.Wall, true);
            Box(root, "Entrance_Lintel", new Vector3(0, H - 0.4f, -T / 2), new Vector3(4f, 0.8f, T), p.Wall, false);
        }

        static void CeilingLights(GameObject root, StorePalette p)
        {
            var lights = Child(root, "CeilingLights");
            foreach (var x in new[] { -5f, 0f, 5f })
            {
                Box(lights, $"Panel_{x:0}", new Vector3(x, H - 0.06f, L / 2), new Vector3(0.6f, 0.06f, L - 4f), p.LightPanel, false);
                foreach (var z in new[] { 5f, 11f, 17f })
                    Lamp(lights, $"Light_{x:0}_{z:0}", new Vector3(x, H - 0.4f, z), new Color(1f, 0.98f, 0.94f), 1.1f, 9f);
            }
        }

        static void Lighting(GameObject root)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.62f, 0.63f, 0.65f);
            RenderSettings.fog = false;
            var sunGo = Child(root, "Sun");
            sunGo.transform.eulerAngles = new Vector3(55f, -25f, 0f);
            var sun = sunGo.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.color = new Color(1f, 0.98f, 0.95f);
            sun.intensity = 0.7f;
            sun.shadows = LightShadows.Soft;
        }
    }
}
