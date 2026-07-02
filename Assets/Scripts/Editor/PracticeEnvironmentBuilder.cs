using UnityEditor;
using UnityEngine;
using static SignTrainer.EditorTools.EnvHelper;

namespace SignTrainer.EditorTools
{
    public static class PracticeEnvironmentBuilder
    {
        const string Menu   = "SignTrainer/Environments/Build Practice";
        const string MatDir = "Assets/Art/Materials/Practice";

        const float W = 9f, L = 10f, H = 3f, T = 0.2f;

        [MenuItem(Menu)]
        public static void Build()
        {
            EnsureMatDir(MatDir);
            var old = GameObject.Find("PracticeEnvironment");
            if (old != null) Undo.DestroyObjectImmediate(old);
            var root = new GameObject("PracticeEnvironment");
            Undo.RegisterCreatedObjectUndo(root, "Build Practice Environment");

            var mFloor = Mat(MatDir, "Prac_Floor",   new Color32(220, 210, 195, 255));
            var mWall  = Mat(MatDir, "Prac_Wall",    new Color32(248, 245, 238, 255));
            var mCeil  = Mat(MatDir, "Prac_Ceiling", new Color32(255, 253, 248, 255));
            var mZone  = Mat(MatDir, "Prac_Zone",    new Color32(180, 210, 240, 255));
            var mBoard = Mat(MatDir, "Prac_Board",   new Color32(240, 245, 255, 255));
            var mFrame = Mat(MatDir, "Prac_Frame",   new Color32( 60,  80, 120, 255));

            Room(root, mFloor, mWall, mCeil, mZone);
            ProgressBoard(root, mBoard, mFrame);

            AssetDatabase.SaveAssets();
            Selection.activeGameObject = root;
            Debug.Log("[PracticeEnvironmentBuilder] Practice environment built.");
        }

        static void Room(GameObject root, Material floor, Material wall, Material ceil, Material zone)
        {
            var r = Child(root, "Room");
            Box(r, "Floor",        new Vector3(0, -T/2,    L/2), new Vector3(W, T, L),          floor, true);
            Box(r, "Ceiling",      new Vector3(0,  H+T/2,  L/2), new Vector3(W, T, L),          ceil,  true);
            Box(r, "Wall_L",       new Vector3(-W/2-T/2, H/2, L/2), new Vector3(T, H+T*2, L+T*2), wall, true);
            Box(r, "Wall_R",       new Vector3( W/2+T/2, H/2, L/2), new Vector3(T, H+T*2, L+T*2), wall, true);
            Box(r, "Wall_Far",     new Vector3(0, H/2,  L+T/2),   new Vector3(W+T*2, H, T),     wall,  true);
            Box(r, "Wall_Near",    new Vector3(0, H/2,   -T/2),   new Vector3(W+T*2, H, T),     wall,  true);
            // Practice zone marker
            Box(r, "PracticeZone", new Vector3(0, 0.01f, 3.5f),   new Vector3(3f, 0.02f, 3f),   zone);
        }

        static void ProgressBoard(GameObject root, Material board, Material frame)
        {
            var go = Child(root, "ProgressBoard");
            go.transform.position = new Vector3(0, 1.6f, 9.85f);
            Box(go, "Board",  new Vector3(0, 0,     0),  new Vector3(3.5f, 1.2f, 0.04f), board);
            Box(go, "Border", new Vector3(0, 0, -0.03f), new Vector3(3.7f, 1.4f, 0.03f), frame);
        }
    }
}
