using UnityEditor;
using UnityEngine;

namespace SignTrainer.EditorTools
{
    public static class LibraryEnvironmentBuilder
    {
        const string Menu   = "SignTrainer/Environments/Build Library";
        const string MatDir = "Assets/Art/Materials/Library";

        // Room dimensions (metres)
        const float W = 10f, L = 12f, H = 3f, T = 0.2f;
        // Bookshelf unit dimensions
        const float BsW = 1.6f, BsD = 0.35f, BsH = 2.0f, BsT = 0.04f;

        [MenuItem(Menu)]
        public static void Build()
        {
            EnsureMatDir();

            var old = GameObject.Find("LibraryEnvironment");
            if (old != null) Undo.DestroyObjectImmediate(old);

            var root = new GameObject("LibraryEnvironment");
            Undo.RegisterCreatedObjectUndo(root, "Build Library Environment");

            var mFloor = Mat("Lib_Floor",    new Color32(143, 113,  75, 255));
            var mWall  = Mat("Lib_Wall",     new Color32(230, 222, 204, 255));
            var mCeil  = Mat("Lib_Ceiling",  new Color32(242, 240, 232, 255));
            var mShelf = Mat("Lib_Shelf",    new Color32( 76,  46,  20, 255));
            var mDesk  = Mat("Lib_Desk",     new Color32(127,  81,  36, 255));
            var mBooks = new[] {
                Mat("Lib_Book_0", new Color32(178,  38,  38, 255)),
                Mat("Lib_Book_1", new Color32( 38,  90, 165, 255)),
                Mat("Lib_Book_2", new Color32( 51, 140,  63, 255)),
                Mat("Lib_Book_3", new Color32(191, 153,  26, 255)),
                Mat("Lib_Book_4", new Color32(140,  38, 140, 255)),
                Mat("Lib_Book_5", new Color32(217, 102,  26, 255)),
            };

            Room(root, mFloor, mWall, mCeil);
            Shelves(root, mShelf, mBooks);
            Furniture(root, mDesk, mShelf);

            AssetDatabase.SaveAssets();
            Selection.activeGameObject = root;
            Debug.Log("[LibraryEnvironmentBuilder] Library environment built.");
        }

        // ── Room / Shelves / Furniture ────────────────────────────────────────

        static void Room(GameObject root, Material floor, Material wall, Material ceil)
        {
            // Room spans x[-5,5] y[0,3] z[0,12]. Player enters from z=0.
            var r = Child(root, "Room");
            Box(r, "Floor",     new Vector3(0, -T/2,      L/2), new Vector3(W, T, L),            floor, true);
            Box(r, "Ceiling",   new Vector3(0, H+T/2,     L/2), new Vector3(W, T, L),            ceil,  true);
            Box(r, "Wall_L",    new Vector3(-W/2-T/2, H/2, L/2), new Vector3(T, H+T*2, L+T*2),  wall,  true);
            Box(r, "Wall_R",    new Vector3( W/2+T/2, H/2, L/2), new Vector3(T, H+T*2, L+T*2),  wall,  true);
            Box(r, "Wall_Far",  new Vector3(0, H/2, L+T/2),    new Vector3(W+T*2, H, T),        wall,  true);
            Box(r, "Wall_Near", new Vector3(0, H/2,  -T/2),    new Vector3(W+T*2, H, T),        wall,  true);
        }

        static void Shelves(GameObject root, Material shelf, Material[] books)
        {
            var parent = Child(root, "Bookshelves");
            float xL = -W/2 + BsD/2, xR = W/2 - BsD/2;
            float[] zz = { 1.5f, 4.0f, 6.5f };
            for (int i = 0; i < zz.Length; i++)
            {
                Bookshelf(parent, $"Shelf_L{i+1}", new Vector3(xL, 0, zz[i]),  90f, shelf, books, i);
                Bookshelf(parent, $"Shelf_R{i+1}", new Vector3(xR, 0, zz[i]), -90f, shelf, books, i+3);
            }
        }

        static void Bookshelf(GameObject parent, string n, Vector3 pos, float yRot,
                               Material shelf, Material[] books, int seed)
        {
            var go = Child(parent, n);
            go.transform.position    = pos;
            go.transform.eulerAngles = new Vector3(0, yRot, 0);

            Box(go, "Left",   new Vector3(-BsW / 2, BsH / 2, 0),             new Vector3(BsT, BsH, BsD),       shelf);
            Box(go, "Right",  new Vector3( BsW / 2, BsH / 2, 0),             new Vector3(BsT, BsH, BsD),       shelf);
            Box(go, "Top",    new Vector3(0, BsH, 0),                         new Vector3(BsW + BsT, BsT, BsD), shelf);
            Box(go, "Bottom", new Vector3(0, BsT / 2, 0),                     new Vector3(BsW + BsT, BsT, BsD), shelf);
            Box(go, "Back",   new Vector3(0, BsH / 2, -BsD / 2 + BsT / 2),   new Vector3(BsW, BsH, BsT),       shelf);

            float[] shelfY = { 0.55f, 1.1f, 1.65f };
            for (int i = 0; i < shelfY.Length; i++)
            {
                Box(go, $"Shelf{i}", new Vector3(0, shelfY[i], 0), new Vector3(BsW, BsT, BsD), shelf);
                BooksRow(go, $"Books{i}", new Vector3(0, shelfY[i] + BsT + 0.105f, 0), BsW, BsD, books, seed * 3 + i);
            }
        }

        static void BooksRow(GameObject parent, string n, Vector3 localPos,
                             float rowW, float rowD, Material[] books, int seed)
        {
            var go = Child(parent, n);
            go.transform.localPosition = localPos;
            const int count = 10;
            float bw = (rowW - 0.05f) / count;
            for (int i = 0; i < count; i++)
            {
                // Deterministic height variation: 0.16 – 0.21 m
                float h  = 0.16f + (float)((seed * count + i * 3) % 6) * 0.01f;
                float x  = -rowW / 2 + bw * i + bw / 2;
                var   mat = books[(seed * count + i) % books.Length];
                Box(go, $"B{i}", new Vector3(x, h / 2, 0), new Vector3(bw - 0.005f, h, rowD * 0.85f), mat);
            }
        }

        static void Furniture(GameObject root, Material desk, Material legs)
        {
            var p = Child(root, "Furniture");
            LibDesk(p, "LibrarianDesk", new Vector3(0, 0, 10f), desk);
            // Reading table at room centre
            Box(p, "Table_Top", new Vector3(0,     0.73f, 5.0f), new Vector3(2.4f, 0.07f, 1.2f),  desk);
            Box(p, "Table_L1",  new Vector3(-1.1f, 0.36f, 4.5f), new Vector3(0.07f, 0.73f, 0.07f), legs);
            Box(p, "Table_R1",  new Vector3( 1.1f, 0.36f, 4.5f), new Vector3(0.07f, 0.73f, 0.07f), legs);
            Box(p, "Table_L2",  new Vector3(-1.1f, 0.36f, 5.5f), new Vector3(0.07f, 0.73f, 0.07f), legs);
            Box(p, "Table_R2",  new Vector3( 1.1f, 0.36f, 5.5f), new Vector3(0.07f, 0.73f, 0.07f), legs);
            Chair(p, "Chair_1", new Vector3(-0.6f, 0, 3.7f),   0f, desk, legs);
            Chair(p, "Chair_2", new Vector3( 0.6f, 0, 3.7f),   0f, desk, legs);
            Chair(p, "Chair_3", new Vector3(-0.6f, 0, 6.3f), 180f, desk, legs);
            Chair(p, "Chair_4", new Vector3( 0.6f, 0, 6.3f), 180f, desk, legs);
        }

        static void LibDesk(GameObject parent, string n, Vector3 pos, Material mat)
        {
            var go = Child(parent, n);
            go.transform.position = pos;
            Box(go, "Top",   new Vector3(0,      0.85f,  0),      new Vector3(3f, 0.06f, 0.8f),   mat);
            Box(go, "Front", new Vector3(0,      0.43f, -0.37f),  new Vector3(3f, 0.85f, 0.06f),  mat);
            Box(go, "Back",  new Vector3(0,      0.43f,  0.37f),  new Vector3(3f, 0.85f, 0.06f),  mat);
            Box(go, "SideL", new Vector3(-1.47f, 0.43f,  0),      new Vector3(0.06f, 0.85f, 0.8f), mat);
            Box(go, "SideR", new Vector3( 1.47f, 0.43f,  0),      new Vector3(0.06f, 0.85f, 0.8f), mat);
        }

        static void Chair(GameObject parent, string n, Vector3 pos, float yRot,
                          Material seat, Material legs)
        {
            var go = Child(parent, n);
            go.transform.position    = pos;
            go.transform.eulerAngles = new Vector3(0, yRot, 0);
            Box(go, "Seat",   new Vector3(0,      0.45f,  0),      new Vector3(0.42f, 0.04f, 0.42f), seat);
            Box(go, "Back",   new Vector3(0,      0.73f, -0.19f),  new Vector3(0.42f, 0.56f, 0.04f), seat);
            Box(go, "Leg_FL", new Vector3(-0.18f, 0.23f,  0.18f),  new Vector3(0.04f, 0.45f, 0.04f), legs);
            Box(go, "Leg_FR", new Vector3( 0.18f, 0.23f,  0.18f),  new Vector3(0.04f, 0.45f, 0.04f), legs);
            Box(go, "Leg_BL", new Vector3(-0.18f, 0.23f, -0.18f),  new Vector3(0.04f, 0.45f, 0.04f), legs);
            Box(go, "Leg_BR", new Vector3( 0.18f, 0.23f, -0.18f),  new Vector3(0.04f, 0.45f, 0.04f), legs);
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        static GameObject Child(GameObject parent, string n)
        {
            var go = new GameObject(n);
            go.transform.SetParent(parent.transform, false);
            return go;
        }

        static void Box(GameObject parent, string n, Vector3 lp, Vector3 scale,
                        Material mat, bool keepCollider = false)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = n;
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = lp;
            go.transform.localScale    = scale;
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
            if (!keepCollider) Object.DestroyImmediate(go.GetComponent<BoxCollider>());
        }

        static Material Mat(string name, Color32 col)
        {
            var path   = $"{MatDir}/{name}.mat";
            var mat    = AssetDatabase.LoadAssetAtPath<Material>(path);
            var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard")!;
            if (mat == null)
            {
                mat = new Material(shader);
                AssetDatabase.CreateAsset(mat, path);
            }
            else
            {
                mat.shader = shader;
            }
            mat.color = col;
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", col);
            EditorUtility.SetDirty(mat);
            return mat;
        }

        static void EnsureMatDir()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Art"))
                AssetDatabase.CreateFolder("Assets", "Art");
            if (!AssetDatabase.IsValidFolder("Assets/Art/Materials"))
                AssetDatabase.CreateFolder("Assets/Art", "Materials");
            if (!AssetDatabase.IsValidFolder(MatDir))
                AssetDatabase.CreateFolder("Assets/Art/Materials", "Library");
        }
    }
}
