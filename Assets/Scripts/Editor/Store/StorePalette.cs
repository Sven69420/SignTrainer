using UnityEngine;
using static SignTrainer.EditorTools.EnvHelper;

namespace SignTrainer.EditorTools
{
    /// <summary>Material set for the grocery store scene.</summary>
    public sealed class StorePalette
    {
        public Material Floor, Wall, Ceiling, Shelf, ShelfBack, Fridge, Glass,
                        Sign, Banner, LightPanel, Checkout, Conveyor, Bin;
        public Material[] Products, Fruits;

        public static StorePalette Build(string dir)
        {
            return new StorePalette
            {
                Floor      = Mat(dir, "Store_Floor",     new Color32(221, 222, 226, 255), 0.55f),
                Wall       = Mat(dir, "Store_Wall",      new Color32(236, 238, 240, 255), 0.1f),
                Ceiling    = Mat(dir, "Store_Ceiling",   new Color32(246, 247, 249, 255), 0f),
                Shelf      = Mat(dir, "Store_Shelf",     new Color32(150, 153, 158, 255), 0.5f, 0.6f),
                ShelfBack  = Mat(dir, "Store_ShelfBack", new Color32(198, 201, 206, 255), 0.2f),
                Fridge     = Mat(dir, "Store_Fridge",    new Color32(170, 174, 180, 255), 0.6f, 0.7f),
                Glass      = Mat(dir, "Store_Glass",     new Color32(202, 226, 236, 255), 0.92f, 0f, new Color(0.4f, 0.55f, 0.62f) * 1.2f),
                Sign       = Mat(dir, "Store_Sign",      new Color32(255, 206, 92, 255), 0.4f, 0f, new Color(1f, 0.7f, 0.18f) * 2.0f),
                Banner     = Mat(dir, "Store_Banner",    new Color32(210, 60, 64, 255), 0.3f, 0f, new Color(0.7f, 0.12f, 0.14f) * 1.6f),
                LightPanel = Mat(dir, "Store_Light",     new Color32(255, 255, 250, 255), 0.3f, 0f, new Color(1f, 0.98f, 0.92f) * 1.7f),
                Checkout   = Mat(dir, "Store_Checkout",  new Color32(56, 118, 162, 255), 0.3f),
                Conveyor   = Mat(dir, "Store_Conveyor",  new Color32(40, 42, 46, 255), 0.1f),
                Bin        = Mat(dir, "Store_Bin",       new Color32(150, 100, 55, 255), 0.15f),
                Products = new[]
                {
                    Mat(dir, "Store_Prod_0", new Color32(214, 60, 56, 255)),
                    Mat(dir, "Store_Prod_1", new Color32(48, 110, 196, 255)),
                    Mat(dir, "Store_Prod_2", new Color32(238, 196, 52, 255)),
                    Mat(dir, "Store_Prod_3", new Color32(70, 168, 92, 255)),
                    Mat(dir, "Store_Prod_4", new Color32(232, 128, 44, 255)),
                    Mat(dir, "Store_Prod_5", new Color32(150, 80, 168, 255)),
                    Mat(dir, "Store_Prod_6", new Color32(46, 168, 174, 255)),
                    Mat(dir, "Store_Prod_7", new Color32(232, 138, 178, 255)),
                },
                Fruits = new[]
                {
                    Mat(dir, "Store_Fruit_0", new Color32(206, 52, 48, 255)),
                    Mat(dir, "Store_Fruit_1", new Color32(238, 150, 40, 255)),
                    Mat(dir, "Store_Fruit_2", new Color32(232, 210, 70, 255)),
                    Mat(dir, "Store_Fruit_3", new Color32(108, 178, 64, 255)),
                },
            };
        }
    }
}
