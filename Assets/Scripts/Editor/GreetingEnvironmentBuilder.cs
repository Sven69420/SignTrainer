using UnityEditor;
using UnityEngine;
using static SignTrainer.EditorTools.EnvHelper;

namespace SignTrainer.EditorTools
{
    public static class GreetingEnvironmentBuilder
    {
        const string Menu   = "SignTrainer/Environments/Build Greeting";
        const string MatDir = "Assets/Art/Materials/Greeting";

        [MenuItem(Menu)]
        public static void Build()
        {
            EnsureMatDir(MatDir);
            var old = GameObject.Find("GreetingEnvironment");
            if (old != null) Undo.DestroyObjectImmediate(old);
            var root = new GameObject("GreetingEnvironment");
            Undo.RegisterCreatedObjectUndo(root, "Build Greeting Environment");

            var mPave   = Mat(MatDir, "Greet_Pavement", new Color32(200, 198, 192, 255));
            var mFacade = Mat(MatDir, "Greet_Facade",   new Color32(235, 228, 210, 255));
            var mDoor   = Mat(MatDir, "Greet_Door",     new Color32( 80,  60,  40, 255));
            var mTrim   = Mat(MatDir, "Greet_Trim",     new Color32(215, 210, 200, 255));
            var mWood   = Mat(MatDir, "Greet_Wood",     new Color32(139,  90,  43, 255));
            var mMetal  = Mat(MatDir, "Greet_Metal",    new Color32( 90,  90,  95, 255));
            var mPlant  = Mat(MatDir, "Greet_Plant",    new Color32( 60, 150,  50, 255));
            var mPot    = Mat(MatDir, "Greet_Pot",      new Color32(170,  90,  50, 255));

            Plaza(root, mPave, mTrim);
            Building(root, mFacade, mDoor, mTrim);
            Bench(root, "Bench_L", new Vector3(-2.5f, 0, 2.5f), mWood, mMetal);
            Bench(root, "Bench_R", new Vector3( 2.5f, 0, 2.5f), mWood, mMetal);
            Planter(root, "Planter_L", new Vector3(-3.5f, 0, 1f), mPot, mPlant);
            Planter(root, "Planter_R", new Vector3( 3.5f, 0, 1f), mPot, mPlant);

            AssetDatabase.SaveAssets();
            Selection.activeGameObject = root;
            Debug.Log("[GreetingEnvironmentBuilder] Greeting environment built.");
        }

        static void Plaza(GameObject root, Material pave, Material step)
        {
            // Player at z=0 facing +z toward building
            Box(root, "Pavement", new Vector3(0, -0.05f, 5f), new Vector3(14f, 0.1f, 14f), pave, true);
            Box(root, "Step1",    new Vector3(0,  0.07f, 6.3f), new Vector3(4f,   0.15f, 0.5f), step);
            Box(root, "Step2",    new Vector3(0,  0.22f, 6.6f), new Vector3(3.5f, 0.15f, 0.5f), step);
        }

        static void Building(GameObject root, Material facade, Material door, Material trim)
        {
            Box(root, "Facade",  new Vector3(0,   2.5f, 7f),    new Vector3(12f, 5f, 0.3f),  facade);
            Box(root, "Wing_L",  new Vector3(-5f, 2.5f, 4.5f),  new Vector3(2f,  5f, 5f),    facade);
            Box(root, "Wing_R",  new Vector3( 5f, 2.5f, 4.5f),  new Vector3(2f,  5f, 5f),    facade);
            Box(root, "Door",    new Vector3(0,   1.2f, 6.88f), new Vector3(1.2f, 2.4f, 0.05f), door);
            Box(root, "Lintel",  new Vector3(0,   2.6f, 6.88f), new Vector3(2f,   0.2f, 0.1f),  trim);
            Box(root, "Cornice", new Vector3(0,  5.15f, 7f),    new Vector3(12.4f, 0.3f, 0.5f), trim);
        }

        static void Bench(GameObject root, string n, Vector3 pos, Material wood, Material metal)
        {
            var go = Child(root, n);
            go.transform.position = pos;
            Box(go, "Seat",  new Vector3(0,     0.45f,  0),     new Vector3(1.5f, 0.07f, 0.45f), wood);
            Box(go, "Back",  new Vector3(0,     0.72f, -0.18f), new Vector3(1.5f, 0.55f, 0.06f), wood);
            Box(go, "Leg_L", new Vector3(-0.55f, 0.23f, 0),     new Vector3(0.06f, 0.45f, 0.45f), metal);
            Box(go, "Leg_R", new Vector3( 0.55f, 0.23f, 0),     new Vector3(0.06f, 0.45f, 0.45f), metal);
        }

        static void Planter(GameObject root, string n, Vector3 pos, Material pot, Material plant)
        {
            var go = Child(root, n);
            go.transform.position = pos;
            Cyl(go,    "Pot",    new Vector3(0, 0.3f,  0), new Vector3(0.6f, 0.6f, 0.6f), pot);
            Cyl(go,    "Trunk",  new Vector3(0, 0.85f, 0), new Vector3(0.1f, 0.5f, 0.1f), pot);
            Sphere(go, "Leaves", new Vector3(0, 1.5f,  0), 0.7f, plant);
        }
    }
}
