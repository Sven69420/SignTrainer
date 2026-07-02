using UnityEngine;
using static SignTrainer.EditorTools.EnvHelper;

namespace SignTrainer.EditorTools
{
    /// <summary>Aisles, fridges, produce, checkout and signage for the store scene.</summary>
    public static class StoreFixtures
    {
        const float L = StoreSceneBuilder.L;

        public static void Build(GameObject root, StorePalette p)
        {
            var aisles = Child(root, "Aisles");
            float[] xx = { -3f, 0f, 3f };
            for (int i = 0; i < xx.Length; i++)
            {
                Gondola(aisles, $"Aisle_{i}", xx[i], 6f, 18f, p, i * 5);
                Sign(aisles, $"AisleSign_{i}", new Vector3(xx[i], 3.2f, 5.2f), i + 1, p);
            }

            WallUnit(root, "Fridges", 8.1f, -1, 6f, 18f, p, true);
            WallUnit(root, "WallShelf", -8.1f, 1, 6f, 18f, p, false);
            Produce(root, p);
            Checkout(root, p);
            Banner(root, p);
        }

        static void Gondola(GameObject parent, string n, float x, float z0, float z1, StorePalette p, int seed)
        {
            var go = Child(parent, n);
            float mid = (z0 + z1) / 2f, len = z1 - z0;
            Box(go, "Base", new Vector3(x, 0.1f, mid), new Vector3(1.0f, 0.2f, len), p.Shelf, true);
            Box(go, "Spine", new Vector3(x, 0.95f, mid), new Vector3(0.12f, 1.7f, len), p.ShelfBack, true);
            foreach (var side in new[] { -1f, 1f })
            {
                float sx = x + side * 0.3f;
                Box(go, $"Cap_{side:0}", new Vector3(sx, 1.0f, mid), new Vector3(0.04f, 1.6f, len), p.Shelf);
                foreach (var y in new[] { 0.55f, 1.0f, 1.45f })
                {
                    Box(go, $"Shelf_{side:0}_{y:0.0}", new Vector3(sx, y, mid), new Vector3(0.44f, 0.04f, len), p.Shelf, false);
                    Products(go, new Vector3(sx, y + 0.04f, 0), z0, z1, p, seed++);
                }
            }
        }

        static void WallUnit(GameObject root, string n, float x, float face, float z0, float z1, StorePalette p, bool fridge)
        {
            var go = Child(root, n);
            float mid = (z0 + z1) / 2f, len = z1 - z0;
            Box(go, "Body", new Vector3(x, 1.5f, mid), new Vector3(1.4f, 3.0f, len), fridge ? p.Fridge : p.Shelf, true);
            if (fridge)
                Box(go, "Glass", new Vector3(x + face * 0.72f, 1.5f, mid), new Vector3(0.08f, 2.6f, len), p.Glass);
            int seed = fridge ? 30 : 60;
            foreach (var y in new[] { 0.6f, 1.2f, 1.8f, 2.4f })
            {
                Box(go, $"Shelf_{y:0.0}", new Vector3(x + face * 0.45f, y, mid), new Vector3(0.5f, 0.04f, len), p.ShelfBack, false);
                Products(go, new Vector3(x + face * 0.45f, y + 0.04f, 0), z0, z1, p, seed++);
            }
        }

        static void Products(GameObject parent, Vector3 basePos, float z0, float z1, StorePalette p, int seed)
        {
            var row = Child(parent, "Items");
            int count = Mathf.Max(1, Mathf.RoundToInt((z1 - z0) / 0.45f));
            for (int i = 0; i < count; i++)
            {
                if ((seed * 7 + i * 5) % 13 == 0) continue;
                float z = z0 + 0.25f + i * 0.45f;
                var mat = p.Products[(seed * count + i) % p.Products.Length];
                if ((seed + i) % 3 == 0)
                {
                    Cyl(row, $"Can_{i}", new Vector3(basePos.x, basePos.y + 0.11f, z), new Vector3(0.12f, 0.11f, 0.12f), mat);
                }
                else
                {
                    float h = 0.22f + (seed * 3 + i) % 5 * 0.02f;
                    Box(row, $"Box_{i}", new Vector3(basePos.x, basePos.y + h / 2f, z), new Vector3(0.22f, h, 0.3f), mat);
                }
            }
        }

        static void Produce(GameObject root, StorePalette p)
        {
            var prod = Child(root, "Produce");
            foreach (var x in new[] { -3f, 0f, 3f })
            {
                var bin = Child(prod, $"Bin_{x:0}");
                bin.transform.position = new Vector3(x, 0, 3f);
                Box(bin, "Box", new Vector3(0, 0.45f, 0), new Vector3(1.6f, 0.5f, 1.2f), p.Bin, true);
                for (int i = 0; i < 14; i++)
                {
                    float fx = -0.6f + i % 5 * 0.3f;
                    float fz = -0.4f + i / 5 * 0.3f;
                    Sphere(bin, $"Fruit_{i}", new Vector3(fx, 0.75f, fz), 0.18f, p.Fruits[i % p.Fruits.Length]);
                }
            }
        }

        static void Checkout(GameObject root, StorePalette p)
        {
            var co = Child(root, "Checkout");
            foreach (var x in new[] { -6.5f, -4f })
            {
                var lane = Child(co, $"Lane_{x:0}");
                lane.transform.position = new Vector3(x, 0, 3.5f);
                Box(lane, "Counter", new Vector3(0, 0.5f, 0), new Vector3(1.0f, 1.0f, 2.4f), p.Checkout, true);
                Box(lane, "Belt", new Vector3(0, 1.02f, 0.3f), new Vector3(0.6f, 0.06f, 1.6f), p.Conveyor, false);
                Box(lane, "Register", new Vector3(0, 1.2f, -0.7f), new Vector3(0.4f, 0.35f, 0.4f), p.ShelfBack, false);
                Sign(lane, "LaneSign", new Vector3(0, 2.6f, 0), 0, p);
            }
        }

        static void Sign(GameObject parent, string n, Vector3 lp, int number, StorePalette p)
        {
            var s = Child(parent, n);
            s.transform.localPosition = lp;
            Box(s, "Panel", Vector3.zero, new Vector3(1.4f, 0.7f, 0.08f), p.Sign, false);
            Cyl(s, "Hanger", new Vector3(0, 0.7f, 0), new Vector3(0.03f, 0.5f, 0.03f), p.Shelf);
        }

        static void Banner(GameObject root, StorePalette p)
        {
            var b = Child(root, "Banner");
            Box(b, "Sign", new Vector3(0, 3.6f, 0.4f), new Vector3(6f, 1.0f, 0.15f), p.Banner, false);
            Box(b, "Backboard", new Vector3(0, 3.6f, 0.55f), new Vector3(6.4f, 1.3f, 0.1f), p.ShelfBack, false);
        }
    }
}
