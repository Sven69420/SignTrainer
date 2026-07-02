using UnityEditor;
using UnityEngine;
using static SignTrainer.EditorTools.EnvHelper;

namespace SignTrainer.EditorTools
{
    public static class HelpDeskEnvironmentBuilder
    {
        const string Menu   = "SignTrainer/Environments/Build HelpDesk";
        const string MatDir = "Assets/Art/Materials/HelpDesk";

        const float W = 8f, L = 10f, H = 2.8f, T = 0.2f;

        [MenuItem(Menu)]
        public static void Build()
        {
            EnsureMatDir(MatDir);
            var old = GameObject.Find("HelpDeskEnvironment");
            if (old != null) Undo.DestroyObjectImmediate(old);
            var root = new GameObject("HelpDeskEnvironment");
            Undo.RegisterCreatedObjectUndo(root, "Build HelpDesk Environment");

            var mFloor = Mat(MatDir, "HD_Floor",   new Color32(200, 200, 205, 255));
            var mWall  = Mat(MatDir, "HD_Wall",    new Color32(245, 245, 240, 255));
            var mCeil  = Mat(MatDir, "HD_Ceiling", new Color32(252, 252, 252, 255));
            var mDesk  = Mat(MatDir, "HD_Desk",    new Color32(110, 115, 125, 255));
            var mDeskT = Mat(MatDir, "HD_DeskTop", new Color32(220, 222, 228, 255));
            var mChair = Mat(MatDir, "HD_Chair",   new Color32( 55,  95, 175, 255));
            var mFrame = Mat(MatDir, "HD_Frame",   new Color32( 60,  60,  65, 255));
            var mSign  = Mat(MatDir, "HD_Sign",    new Color32(240, 245, 255, 255));

            Room(root, mFloor, mWall, mCeil);
            ReceptionDesk(root, mDesk, mDeskT);
            WaitingChairs(root, mChair, mFrame);
            InfoPanel(root, mSign, mFrame);

            AssetDatabase.SaveAssets();
            Selection.activeGameObject = root;
            Debug.Log("[HelpDeskEnvironmentBuilder] HelpDesk environment built.");
        }

        static void Room(GameObject root, Material floor, Material wall, Material ceil)
        {
            var r = Child(root, "Room");
            Box(r, "Floor",     new Vector3(0, -T/2,      L/2), new Vector3(W, T, L),            floor, true);
            Box(r, "Ceiling",   new Vector3(0,  H+T/2,    L/2), new Vector3(W, T, L),            ceil,  true);
            Box(r, "Wall_L",    new Vector3(-W/2-T/2, H/2, L/2), new Vector3(T, H+T*2, L+T*2),  wall,  true);
            Box(r, "Wall_R",    new Vector3( W/2+T/2, H/2, L/2), new Vector3(T, H+T*2, L+T*2),  wall,  true);
            Box(r, "Wall_Far",  new Vector3(0, H/2,  L+T/2),   new Vector3(W+T*2, H, T),        wall,  true);
            Box(r, "Wall_Near", new Vector3(0, H/2,   -T/2),   new Vector3(W+T*2, H, T),        wall,  true);
            // Skirting boards
            Box(r, "Skirt_L",   new Vector3(-W/2+T/2, 0.06f, L/2), new Vector3(0.02f, 0.12f, L), wall);
            Box(r, "Skirt_R",   new Vector3( W/2-T/2, 0.06f, L/2), new Vector3(0.02f, 0.12f, L), wall);
        }

        static void ReceptionDesk(GameObject root, Material body, Material top)
        {
            var go = Child(root, "ReceptionDesk");
            go.transform.position = new Vector3(0, 0, 7.5f);
            Box(go, "Body",   new Vector3(0,     0.55f,  0),      new Vector3(4.0f, 1.1f,  0.7f),  body);
            Box(go, "Top",    new Vector3(0,     1.13f,  0.1f),   new Vector3(4.2f, 0.06f, 0.9f),  top);
            Box(go, "Side_L", new Vector3(-2.1f, 0.55f,  0.35f),  new Vector3(0.06f, 1.1f, 0.7f),  body);
            Box(go, "Side_R", new Vector3( 2.1f, 0.55f,  0.35f),  new Vector3(0.06f, 1.1f, 0.7f),  body);
        }

        static void WaitingChairs(GameObject root, Material seat, Material frame)
        {
            for (int i = 0; i < 3; i++)
            {
                var go = Child(root, $"WaitChair_{i + 1}");
                go.transform.position = new Vector3(-3f, 0, 2.5f + i * 1.2f);
                Box(go, "Seat", new Vector3(0,      0.45f,  0),      new Vector3(0.45f, 0.05f, 0.45f), seat);
                Box(go, "Back", new Vector3(0,      0.72f, -0.2f),   new Vector3(0.45f, 0.5f,  0.05f), seat);
                Box(go, "Leg1", new Vector3(-0.18f, 0.22f,  0.18f),  new Vector3(0.04f, 0.44f, 0.04f), frame);
                Box(go, "Leg2", new Vector3( 0.18f, 0.22f,  0.18f),  new Vector3(0.04f, 0.44f, 0.04f), frame);
                Box(go, "Leg3", new Vector3(-0.18f, 0.22f, -0.18f),  new Vector3(0.04f, 0.44f, 0.04f), frame);
                Box(go, "Leg4", new Vector3( 0.18f, 0.22f, -0.18f),  new Vector3(0.04f, 0.44f, 0.04f), frame);
            }
        }

        static void InfoPanel(GameObject root, Material sign, Material frame)
        {
            var go = Child(root, "InfoPanel");
            go.transform.position = new Vector3(0, 1.8f, 9.85f);
            Box(go, "Board",  new Vector3(0, 0,     0), new Vector3(2.5f, 0.8f, 0.04f), sign);
            Box(go, "Border", new Vector3(0, 0, -0.03f), new Vector3(2.7f, 1.0f, 0.03f), frame);
        }
    }
}
