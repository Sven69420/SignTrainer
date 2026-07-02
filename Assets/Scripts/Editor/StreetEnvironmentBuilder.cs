using UnityEditor;
using UnityEngine;
using static SignTrainer.EditorTools.EnvHelper;

namespace SignTrainer.EditorTools
{
    public static class StreetEnvironmentBuilder
    {
        const string Menu   = "SignTrainer/Environments/Build Street";
        const string MatDir = "Assets/Art/Materials/Street";

        [MenuItem(Menu)]
        public static void Build()
        {
            EnsureMatDir(MatDir);
            var old = GameObject.Find("StreetEnvironment");
            if (old != null) Undo.DestroyObjectImmediate(old);
            var root = new GameObject("StreetEnvironment");
            Undo.RegisterCreatedObjectUndo(root, "Build Street Environment");

            var mPave   = Mat(MatDir, "St_Sidewalk",  new Color32(195, 192, 186, 255));
            var mRoad   = Mat(MatDir, "St_Road",      new Color32( 65,  65,  68, 255));
            var mCurb   = Mat(MatDir, "St_Curb",      new Color32(180, 178, 172, 255));
            var mBldgA  = Mat(MatDir, "St_BldgA",     new Color32(190, 175, 155, 255));
            var mBldgB  = Mat(MatDir, "St_BldgB",     new Color32(155, 165, 175, 255));
            var mWin    = Mat(MatDir, "St_Window",    new Color32(160, 195, 230, 255));
            var mLamp   = Mat(MatDir, "St_LampPole",  new Color32( 70,  70,  75, 255));
            var mGlow   = Mat(MatDir, "St_LampHead",  new Color32(245, 235, 180, 255));
            var mStripe = Mat(MatDir, "St_RoadLine",  new Color32(230, 225, 200, 255));

            Ground(root, mPave, mRoad, mCurb, mStripe);
            Buildings(root, mBldgA, mBldgB, mWin);
            StreetLamp(root, "Lamp_1", new Vector3(-5.5f, 0, 5f),  mLamp, mGlow);
            StreetLamp(root, "Lamp_2", new Vector3(-5.5f, 0, 11f), mLamp, mGlow);

            AssetDatabase.SaveAssets();
            Selection.activeGameObject = root;
            Debug.Log("[StreetEnvironmentBuilder] Street environment built.");
        }

        static void Ground(GameObject root, Material pave, Material road, Material curb, Material stripe)
        {
            // Player on left sidewalk (x < -3), road centre, opposite sidewalk (x > 3)
            Box(root, "Sidewalk_L",  new Vector3(-5f,  0.01f, 7f), new Vector3(4f,   0.02f, 18f), pave, true);
            Box(root, "Road",        new Vector3( 0,   0,     7f), new Vector3(6f,   0.02f, 18f), road, true);
            Box(root, "Sidewalk_R",  new Vector3( 5f,  0.01f, 7f), new Vector3(4f,   0.02f, 18f), pave);
            Box(root, "Curb_L",      new Vector3(-3f,  0.07f, 7f), new Vector3(0.12f,0.14f, 18f), curb);
            Box(root, "Curb_R",      new Vector3( 3f,  0.07f, 7f), new Vector3(0.12f,0.14f, 18f), curb);
            Box(root, "CentreLine",  new Vector3( 0,   0.03f, 7f), new Vector3(0.15f,0.01f, 18f), stripe);
        }

        static void Buildings(GameObject root, Material a, Material b, Material win)
        {
            Box(root, "Bldg_L", new Vector3(-10f, 4f, 7f), new Vector3(8f, 8f, 20f), a);
            Box(root, "Bldg_R", new Vector3( 10f, 4f, 7f), new Vector3(8f, 8f, 20f), b);
            Windows(root, "Win_L", new Vector3(-6.1f, 4f, 7f), win);
            Windows(root, "Win_R", new Vector3( 6.1f, 4f, 7f), win);
        }

        static void Windows(GameObject root, string n, Vector3 origin, Material glass)
        {
            var go = Child(root, n);
            go.transform.position = origin;
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    Box(go, $"W{row}{col}",
                        new Vector3(0, -1.5f + row * 1.5f, -4f + col * 4f),
                        new Vector3(0.06f, 0.8f, 1.0f), glass);
        }

        static void StreetLamp(GameObject root, string n, Vector3 pos, Material pole, Material head)
        {
            var go = Child(root, n);
            go.transform.position = pos;
            Cyl(go, "Pole", new Vector3(0,    1.75f, 0), new Vector3(0.07f, 3.5f, 0.07f), pole);
            Box(go, "Arm",  new Vector3(0.4f, 3.6f,  0), new Vector3(0.8f,  0.06f, 0.06f), pole);
            Box(go, "Head", new Vector3(0.8f, 3.5f,  0), new Vector3(0.25f, 0.15f, 0.25f), head);
        }
    }
}
