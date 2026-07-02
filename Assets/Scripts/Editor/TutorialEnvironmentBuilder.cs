using UnityEditor;
using UnityEngine;
using static SignTrainer.EditorTools.EnvHelper;

namespace SignTrainer.EditorTools
{
    public static class TutorialEnvironmentBuilder
    {
        const string Menu   = "SignTrainer/Environments/Build Tutorial";
        const string MatDir = "Assets/Art/Materials/Tutorial";

        const float W = 8f, L = 8f, H = 3f, T = 0.2f;

        [MenuItem(Menu)]
        public static void Build()
        {
            EnsureMatDir(MatDir);
            var old = GameObject.Find("TutorialEnvironment");
            if (old != null) Undo.DestroyObjectImmediate(old);
            var root = new GameObject("TutorialEnvironment");
            Undo.RegisterCreatedObjectUndo(root, "Build Tutorial Environment");

            var mFloor  = Mat(MatDir, "Tut_Floor",   new Color32(215, 220, 230, 255));
            var mWall   = Mat(MatDir, "Tut_Wall",    new Color32(248, 248, 250, 255));
            var mCeil   = Mat(MatDir, "Tut_Ceiling", new Color32(255, 255, 255, 255));
            var mPed    = Mat(MatDir, "Tut_Pedestal",new Color32(230, 232, 238, 255));
            var mAccent = Mat(MatDir, "Tut_Accent",  new Color32( 80, 120, 200, 255));
            var mMat    = Mat(MatDir, "Tut_Mat",     new Color32(100, 140, 200, 255));

            Room(root, mFloor, mWall, mCeil, mMat, mAccent);
            Pedestal(root, mPed, mAccent);

            AssetDatabase.SaveAssets();
            Selection.activeGameObject = root;
            Debug.Log("[TutorialEnvironmentBuilder] Tutorial environment built.");
        }

        static void Room(GameObject root, Material floor, Material wall, Material ceil,
                         Material mat, Material accent)
        {
            var r = Child(root, "Room");
            Box(r, "Floor",      new Vector3(0, -T/2,    L/2), new Vector3(W, T, L),          floor,  true);
            Box(r, "Ceiling",    new Vector3(0,  H+T/2,  L/2), new Vector3(W, T, L),          ceil,   true);
            Box(r, "Wall_L",     new Vector3(-W/2-T/2, H/2, L/2), new Vector3(T, H+T*2, L+T*2), wall, true);
            Box(r, "Wall_R",     new Vector3( W/2+T/2, H/2, L/2), new Vector3(T, H+T*2, L+T*2), wall, true);
            Box(r, "Wall_Far",   new Vector3(0, H/2,  L+T/2),   new Vector3(W+T*2, H, T),     wall,   true);
            Box(r, "Wall_Near",  new Vector3(0, H/2,   -T/2),   new Vector3(W+T*2, H, T),     wall,   true);
            // Blue practice mat on the floor
            Box(r, "PracticeMat",  new Vector3(0, 0.01f, 2.5f), new Vector3(2f, 0.02f, 2f),   mat);
            // Accent stripe at base of far wall
            Box(r, "AccentStripe", new Vector3(0, 0.06f, L-T), new Vector3(W, 0.12f, T*2),    accent);
        }

        static void Pedestal(GameObject root, Material body, Material top)
        {
            var go = Child(root, "DemoPedestal");
            go.transform.position = new Vector3(0, 0, 5.5f);
            Box(go, "Base",   new Vector3(0, 0.06f, 0),  new Vector3(0.85f, 0.12f, 0.85f), body);
            Box(go, "Column", new Vector3(0, 0.5f,  0),  new Vector3(0.7f,  1.0f,  0.7f),  body);
            Box(go, "Top",    new Vector3(0, 1.05f, 0),  new Vector3(0.9f,  0.1f,  0.9f),  top);
        }
    }
}
