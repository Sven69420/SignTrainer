using UnityEditor;
using UnityEngine;
using static SignTrainer.EditorTools.EnvHelper;

namespace SignTrainer.EditorTools
{
    /// <summary>
    /// Bigger, richer greeting town-square (replaces "GreetingEnvironment") plus a
    /// walk/teleport XR rig. Run with Scenario_Greeting open.
    /// Menu: SignTrainer ▸ Environments ▸ Build Greeting (Enhanced).
    /// </summary>
    public static class GreetingSceneBuilder
    {
        const string Menu = "SignTrainer/Environments/Build Greeting (Enhanced)";
        const string MatDir = "Assets/Art/Materials/Greeting";

        [MenuItem(Menu)]
        public static void Build()
        {
            EnsureMatDir(MatDir);
            var old = GameObject.Find("GreetingEnvironment");
            if (old != null) Undo.DestroyObjectImmediate(old);
            var root = new GameObject("GreetingEnvironment");
            Undo.RegisterCreatedObjectUndo(root, "Build Enhanced Greeting");

            var pave   = Mat(MatDir, "Greet_Pavement", new Color32(196, 192, 184, 255), 0.2f);
            var curb   = Mat(MatDir, "Greet_Curb",     new Color32(150, 146, 138, 255), 0.1f);
            var facade = Mat(MatDir, "Greet_Facade",   new Color32(232, 222, 200, 255), 0.05f);
            var stone  = Mat(MatDir, "Greet_Stone",    new Color32(214, 206, 190, 255), 0.1f);
            var door   = Mat(MatDir, "Greet_Door",     new Color32( 70,  50,  34, 255), 0.3f);
            var trim   = Mat(MatDir, "Greet_Trim",     new Color32(120,  92,  60, 255), 0.2f);
            var wood   = Mat(MatDir, "Greet_Wood",     new Color32(139,  90,  43, 255), 0.2f);
            var metal  = Mat(MatDir, "Greet_Metal",    new Color32(110, 112, 118, 255), 0.7f, 0.8f);
            var plant  = Mat(MatDir, "Greet_Plant",    new Color32( 56, 140,  52, 255));
            var pot    = Mat(MatDir, "Greet_Pot",      new Color32(170,  90,  50, 255), 0.1f);
            var win    = Mat(MatDir, "Greet_Window",   new Color32(180, 214, 234, 255), 0.9f, 0f, new Color(0.5f, 0.62f, 0.74f) * 1.2f);
            var water  = Mat(MatDir, "Greet_Water",    new Color32( 90, 170, 210, 255), 0.95f, 0f, new Color(0.18f, 0.42f, 0.6f));
            var lampM  = Mat(MatDir, "Greet_Lamp",     new Color32(255, 240, 205, 255), 0.4f, 0f, new Color(1f, 0.86f, 0.55f) * 2.4f);

            Ground(root, pave, curb);
            CivicBuilding(root, facade, stone, door, trim, win);
            SideBuildings(root, facade, win, trim);
            Fountain(root, stone, water);
            Decor(root, wood, metal, pot, plant);
            Lamps(root, metal, lampM);
            Lighting(root);

            SceneXRRig.Build(new Vector3(0, 0, 1.6f), GameObject.Find("GreetingEnvironment/Ground/Plaza"),
                             "GreetingXR (Generated)");

            AssetDatabase.SaveAssets();
            Selection.activeGameObject = root;
            Debug.Log("[GreetingSceneBuilder] Enhanced plaza + XR rig built. Save the scene (Ctrl+S).");
        }

        static void Ground(GameObject root, Material pave, Material curb)
        {
            var g = Child(root, "Ground");
            Box(g, "Plaza", new Vector3(0, -0.05f, 12f), new Vector3(24f, 0.1f, 24f), pave, true);
            Box(g, "Curb_L", new Vector3(-11.8f, 0.08f, 12f), new Vector3(0.4f, 0.16f, 24f), curb, true);
            Box(g, "Curb_R", new Vector3(11.8f, 0.08f, 12f), new Vector3(0.4f, 0.16f, 24f), curb, true);
            // Steps up to the civic building.
            Box(g, "Step1", new Vector3(0, 0.1f, 20.8f), new Vector3(8f, 0.2f, 0.6f), curb, true);
            Box(g, "Step2", new Vector3(0, 0.3f, 21.3f), new Vector3(7f, 0.2f, 0.6f), curb, true);
        }

        static void CivicBuilding(GameObject root, Material facade, Material stone, Material door, Material trim, Material win)
        {
            var b = Child(root, "CivicBuilding");
            Box(b, "Facade", new Vector3(0, 4f, 23.5f), new Vector3(18f, 8f, 1f), facade, true);
            Box(b, "Cornice", new Vector3(0, 8.2f, 23.2f), new Vector3(19f, 0.6f, 1.4f), trim);
            Box(b, "Pediment", new Vector3(0, 8.9f, 23.2f), new Vector3(8f, 1.2f, 1f), stone);
            for (int i = -3; i <= 3; i++)
                Cyl(b, $"Column_{i}", new Vector3(i * 2.2f, 3.4f, 22.4f), new Vector3(0.7f, 3.4f, 0.7f), stone);
            Box(b, "Door", new Vector3(0, 2.2f, 22.95f), new Vector3(2.6f, 4.4f, 0.1f), door);
            // Window rows.
            for (int i = -3; i <= 3; i++)
            {
                if (i == 0) continue;
                Box(b, $"Win_{i}", new Vector3(i * 2.2f, 5.6f, 22.95f), new Vector3(1.2f, 1.8f, 0.1f), win);
            }
        }

        static void SideBuildings(GameObject root, Material facade, Material win, Material trim)
        {
            var s = Child(root, "SideBuildings");
            foreach (var side in new[] { -1f, 1f })
                for (int i = 0; i < 3; i++)
                {
                    float z = 5f + i * 6f;
                    var shop = Child(s, $"Shop_{(side < 0 ? "L" : "R")}_{i}");
                    shop.transform.position = new Vector3(side * 12.6f, 0, z);
                    Box(shop, "Body", new Vector3(0, 3f, 0), new Vector3(1.5f, 6f, 5.5f), facade, true);
                    Box(shop, "Window", new Vector3(-side * 0.8f, 1.6f, 0), new Vector3(0.1f, 2.2f, 3.5f), win);
                    Box(shop, "Awning", new Vector3(-side * 1.1f, 3.1f, 0), new Vector3(0.8f, 0.15f, 4.5f), trim);
                }
        }

        static void Fountain(GameObject root, Material stone, Material water)
        {
            var f = Child(root, "Fountain");
            f.transform.position = new Vector3(0, 0, 12f);
            Cyl(f, "Basin", new Vector3(0, 0.3f, 0), new Vector3(4.4f, 0.3f, 4.4f), stone);
            Cyl(f, "BasinInner", new Vector3(0, 0.35f, 0), new Vector3(3.8f, 0.28f, 3.8f), stone);
            Cyl(f, "Water", new Vector3(0, 0.45f, 0), new Vector3(3.6f, 0.05f, 3.6f), water);
            Cyl(f, "Pedestal", new Vector3(0, 0.9f, 0), new Vector3(0.8f, 0.6f, 0.8f), stone);
            Cyl(f, "Tier", new Vector3(0, 1.5f, 0), new Vector3(1.8f, 0.12f, 1.8f), stone);
        }

        static void Decor(GameObject root, Material wood, Material metal, Material pot, Material plant)
        {
            var d = Child(root, "Decor");
            foreach (var (x, z) in new[] { (-6f, 8f), (6f, 8f), (-6f, 16f), (6f, 16f) })
                Bench(d, $"Bench_{x:0}_{z:0}", new Vector3(x, 0, z), wood, metal);
            foreach (var (x, z) in new[] { (-9f, 4f), (9f, 4f), (-9f, 12f), (9f, 12f), (-9f, 19f), (9f, 19f) })
                Tree(d, $"Tree_{x:0}_{z:0}", new Vector3(x, 0, z), pot, plant);
        }

        static void Bench(GameObject root, string n, Vector3 pos, Material wood, Material metal)
        {
            var go = Child(root, n);
            go.transform.position = pos;
            Box(go, "Seat", new Vector3(0, 0.45f, 0), new Vector3(1.6f, 0.07f, 0.5f), wood);
            Box(go, "Back", new Vector3(0, 0.72f, -0.2f), new Vector3(1.6f, 0.55f, 0.06f), wood);
            Box(go, "Leg_L", new Vector3(-0.6f, 0.23f, 0), new Vector3(0.06f, 0.45f, 0.5f), metal);
            Box(go, "Leg_R", new Vector3(0.6f, 0.23f, 0), new Vector3(0.06f, 0.45f, 0.5f), metal);
        }

        static void Tree(GameObject root, string n, Vector3 pos, Material pot, Material plant)
        {
            var go = Child(root, n);
            go.transform.position = pos;
            Cyl(go, "Trunk", new Vector3(0, 1.1f, 0), new Vector3(0.28f, 1.1f, 0.28f), pot);
            Sphere(go, "Canopy_A", new Vector3(0, 2.6f, 0), 2.0f, plant);
            Sphere(go, "Canopy_B", new Vector3(0.6f, 2.2f, 0.4f), 1.4f, plant);
            Sphere(go, "Canopy_C", new Vector3(-0.5f, 2.3f, -0.3f), 1.5f, plant);
        }

        static void Lamps(GameObject root, Material metal, Material lampM)
        {
            var l = Child(root, "Lamps");
            foreach (var (x, z) in new[] { (-4f, 5f), (4f, 5f), (-4f, 18f), (4f, 18f) })
            {
                var p = Child(l, $"Lamp_{x:0}_{z:0}");
                p.transform.position = new Vector3(x, 0, z);
                Cyl(p, "Pole", new Vector3(0, 1.8f, 0), new Vector3(0.12f, 1.8f, 0.12f), metal);
                Box(p, "Head", new Vector3(0, 3.7f, 0), new Vector3(0.4f, 0.4f, 0.4f), lampM, false);
                Lamp(p, "Light", new Vector3(0, 3.6f, 0), new Color(1f, 0.88f, 0.6f), 1.4f, 10f);
            }
        }

        static void Lighting(GameObject root)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.55f, 0.57f, 0.62f);
            RenderSettings.fog = false;
            foreach (var existing in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
                if (existing.type == LightType.Directional && existing.isActiveAndEnabled) return;
            var sunGo = Child(root, "Sun");
            sunGo.transform.eulerAngles = new Vector3(50f, -30f, 0f);
            var sun = sunGo.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.color = new Color(1f, 0.97f, 0.9f);
            sun.intensity = 1.1f;
            sun.shadows = LightShadows.Soft;
        }
    }
}
