using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using SignTrainer.Data;
using SignTrainer.Practice;
using SignTrainer.XR;
using static SignTrainer.EditorTools.EnvHelper;

namespace SignTrainer.EditorTools
{
    /// <summary>
    /// Generates a ready-to-run fingerspelling practice scene: studio + XR rig +
    /// hand-tracking recognizer + world-space UI, wired to a curated A–Z subset.
    /// Menu: SignTrainer ▸ Environments ▸ Build Fingerspelling Practice (New).
    /// </summary>
    public static class FingerspellSceneBuilder
    {
        const string MatDir = "Assets/Art/Materials/Practice";
        const string ScenePath = "Assets/Scenes/Practice_Fingerspell.unity";
        static readonly string[] Curated = { "A", "B", "D", "F", "H", "I", "L", "V", "W", "Y" };

        [MenuItem("SignTrainer/Environments/Build Fingerspelling Practice (New)")]
        public static void Build()
        {
            if (!EditorUtility.DisplayDialog("Build Fingerspelling Practice",
                    "Creates a new scene and replaces the current view.\nSave any unsaved work first.\n\nContinue?",
                    "Create Scene", "Cancel"))
                return;

            HandShapeLibraryGenerator.Generate();
            EnsureMatDir(MatDir);
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var root = new GameObject("FingerspellStudio");
            Studio(root);
            Lighting(root);

            var floor = GameObject.Find("FingerspellStudio/Floor");
            SceneXRRig.Build(new Vector3(0, 0, 0f), floor, "FingerspellXR (Generated)");

            var view = BuildCanvas();
            BuildManager(view);

            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScene(scene, ScenePath);
            Debug.Log($"[FingerspellSceneBuilder] Scene created at {ScenePath}. " +
                      "Enable OpenXR ▸ Hand Tracking Subsystem in XR settings, then play in-headset.");
        }

        static void Studio(GameObject root)
        {
            var floorM = Mat(MatDir, "Prac_Floor", new Color32(150, 153, 160, 255), 0.3f);
            var wallM = Mat(MatDir, "Prac_Wall", new Color32(70, 86, 104, 255), 0.05f);
            var ceilM = Mat(MatDir, "Prac_Ceiling", new Color32(54, 64, 78, 255), 0f);
            const float W = 8f, L = 9f, H = 3.5f, T = 0.2f;
            Box(root, "Floor", new Vector3(0, -T / 2, L / 2 - 1f), new Vector3(W, T, L), floorM, true);
            Box(root, "Ceiling", new Vector3(0, H, L / 2 - 1f), new Vector3(W, T, L), ceilM, false);
            Box(root, "Wall_Far", new Vector3(0, H / 2, L - 1f), new Vector3(W, H, T), wallM, true);
            Box(root, "Wall_L", new Vector3(-W / 2, H / 2, L / 2 - 1f), new Vector3(T, H, L), wallM, true);
            Box(root, "Wall_R", new Vector3(W / 2, H / 2, L / 2 - 1f), new Vector3(T, H, L), wallM, true);
            Box(root, "Wall_Near", new Vector3(0, H / 2, -1f), new Vector3(W, H, T), wallM, true);
        }

        static void Lighting(GameObject root)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.5f, 0.52f, 0.56f);
            RenderSettings.fog = false;
            var sun = Child(root, "Sun").AddComponent<Light>();
            sun.transform.eulerAngles = new Vector3(50f, -20f, 0f);
            sun.type = LightType.Directional;
            sun.intensity = 1.0f;
            sun.color = new Color(1f, 0.97f, 0.92f);
        }

        static FingerspellPracticeView BuildCanvas()
        {
            var go = new GameObject("PracticeCanvas");
            var canvas = go.AddComponent<Canvas>(); // adds RectTransform
            canvas.renderMode = RenderMode.WorldSpace;
            go.AddComponent<CanvasScaler>();
            go.AddComponent<GraphicRaycaster>();
            go.AddComponent<AudioSource>();
            var view = go.AddComponent<FingerspellPracticeView>();
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(800, 500);
            go.transform.position = new Vector3(0, 1.6f, 3f);
            go.transform.localScale = Vector3.one * 0.0035f;

            MakeText(rt, "Prompt", "A", 240, new Vector2(0, 70), Color.white);
            MakeText(rt, "Hint", "Sign the letter", 48, new Vector2(0, -150), new Color(0.85f, 0.9f, 1f));
            MakeText(rt, "Checkmark", "✓", 200, new Vector2(0, 70), new Color(0.3f, 0.9f, 0.4f));

            var bar = new GameObject("ScoreBar", typeof(RectTransform), typeof(Image));
            bar.transform.SetParent(rt, false);
            ((RectTransform)bar.transform).anchoredPosition = new Vector2(0, -210);
            ((RectTransform)bar.transform).sizeDelta = new Vector2(600, 36);
            bar.GetComponent<Image>().color = new Color(0.15f, 0.18f, 0.22f);
            var fillGo = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fillGo.transform.SetParent(bar.transform, false);
            ((RectTransform)fillGo.transform).sizeDelta = new Vector2(600, 36);
            var fill = fillGo.GetComponent<Image>();
            fill.color = new Color(0.3f, 0.8f, 0.45f);
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillOrigin = 0;
            fill.fillAmount = 0f;

            return view;
        }

        static void BuildManager(FingerspellPracticeView view)
        {
            var mgr = new GameObject("FingerspellManager");
            mgr.AddComponent<HandTracker>();
            mgr.AddComponent<HandShapeRecognizer>();
            var ctrl = mgr.AddComponent<FingerspellPracticeController>();

            var list = new List<HandShapeData>();
            foreach (var l in Curated)
            {
                var a = AssetDatabase.LoadAssetAtPath<HandShapeData>($"Assets/Resources/HandShapes/HS_{l}.asset");
                if (a != null) list.Add(a);
            }

            var so = new SerializedObject(ctrl);
            var arr = so.FindProperty("shapes");
            arr.arraySize = list.Count;
            for (int i = 0; i < list.Count; i++)
                arr.GetArrayElementAtIndex(i).objectReferenceValue = list[i];
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static TextMeshProUGUI MakeText(RectTransform parent, string name, string text, float size, Vector2 pos, Color col)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var t = go.AddComponent<TextMeshProUGUI>();
            t.text = text;
            t.fontSize = size;
            t.alignment = TextAlignmentOptions.Center;
            t.color = col;
            var rt = (RectTransform)go.transform;
            rt.anchoredPosition = pos;
            rt.sizeDelta = new Vector2(760, size * 1.6f);
            return t;
        }
    }
}
