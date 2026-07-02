using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SignTrainer.Data;

namespace SignTrainer.EditorTools
{
    /// <summary>
    /// Generates approximate A–Z fingerspelling hand-shape assets into a Resources
    /// folder. Patterns are curl-only (thumb,index,middle,ring,pinky) using
    /// E=extended, C=curled, A=any. Some letters are intentionally loose where curl
    /// alone can't distinguish them (e.g. M/N/S/T) — guided practice, not recognition.
    /// Menu: SignTrainer ▸ Setup ▸ Generate Hand Shapes (A–Z).
    /// </summary>
    public static class HandShapeLibraryGenerator
    {
        const string Dir = "Assets/Resources/HandShapes";

        static readonly Dictionary<char, string> Patterns = new()
        {
            { 'A', "ACCCC" }, { 'B', "CEEEE" }, { 'C', "AAAAA" }, { 'D', "AECCC" },
            { 'E', "CCCCC" }, { 'F', "CCEEE" }, { 'G', "AECCC" }, { 'H', "AEECC" },
            { 'I', "CCCCE" }, { 'J', "CCCCE" }, { 'K', "AEECC" }, { 'L', "EECCC" },
            { 'M', "ACCCC" }, { 'N', "ACCCC" }, { 'O', "AAAAA" }, { 'P', "AEECC" },
            { 'Q', "AECCC" }, { 'R', "AEECC" }, { 'S', "ACCCC" }, { 'T', "ACCCC" },
            { 'U', "CEECC" }, { 'V', "CEECC" }, { 'W', "CEEEC" }, { 'X', "ACCCC" },
            { 'Y', "ECCCE" }, { 'Z', "AECCC" },
        };

        [MenuItem("SignTrainer/Setup/Generate Hand Shapes (A-Z)")]
        public static void Generate()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            if (!AssetDatabase.IsValidFolder(Dir))
                AssetDatabase.CreateFolder("Assets/Resources", "HandShapes");

            foreach (var kv in Patterns)
            {
                string path = $"{Dir}/HS_{kv.Key}.asset";
                var shape = AssetDatabase.LoadAssetAtPath<HandShapeData>(path);
                if (shape == null)
                {
                    shape = ScriptableObject.CreateInstance<HandShapeData>();
                    AssetDatabase.CreateAsset(shape, path);
                }
                shape.label = kv.Key.ToString();
                shape.thumb = Parse(kv.Value[0]);
                shape.index = Parse(kv.Value[1]);
                shape.middle = Parse(kv.Value[2]);
                shape.ring = Parse(kv.Value[3]);
                shape.pinky = Parse(kv.Value[4]);
                EditorUtility.SetDirty(shape);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[HandShapeLibraryGenerator] Generated {Patterns.Count} hand shapes in {Dir}.");
        }

        static FingerTarget Parse(char c) => c switch
        {
            'E' => FingerTarget.Extended,
            'C' => FingerTarget.Curled,
            _ => FingerTarget.Any,
        };
    }
}
