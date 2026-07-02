using UnityEditor;
using UnityEngine;

namespace SignTrainer.EditorTools
{
    internal static class EnvHelper
    {
        internal static GameObject Child(GameObject parent, string n)
        {
            var go = new GameObject(n);
            go.transform.SetParent(parent.transform, false);
            return go;
        }

        internal static void Box(GameObject parent, string n, Vector3 lp, Vector3 scale,
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

        internal static void Cyl(GameObject parent, string n, Vector3 lp, Vector3 scale, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = n;
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = lp;
            go.transform.localScale    = scale;
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
            Object.DestroyImmediate(go.GetComponent<CapsuleCollider>());
        }

        internal static void Sphere(GameObject parent, string n, Vector3 lp, float r, Material mat)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = n;
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = lp;
            go.transform.localScale    = Vector3.one * r;
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
            Object.DestroyImmediate(go.GetComponent<SphereCollider>());
        }

        internal static Material Mat(string matDir, string name, Color32 col)
        {
            var path   = $"{matDir}/{name}.mat";
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

        internal static Material Mat(string matDir, string name, Color32 col, float smoothness,
                                     float metallic = 0f, Color? emission = null)
        {
            var mat = Mat(matDir, name, col);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);
            if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", smoothness);
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
            if (emission.HasValue)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", emission.Value);
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }
            EditorUtility.SetDirty(mat);
            return mat;
        }

        /// <summary>Cheap mobile-friendly point light (vertex, no shadows).</summary>
        internal static GameObject Lamp(GameObject parent, string n, Vector3 lp,
                                        Color color, float intensity, float range)
        {
            var go = Child(parent, n);
            go.transform.localPosition = lp;
            var l = go.AddComponent<Light>();
            l.type = LightType.Point;
            l.color = color;
            l.intensity = intensity;
            l.range = range;
            l.shadows = LightShadows.None;
            l.renderMode = LightRenderMode.ForceVertex;
            return go;
        }

        internal static void EnsureMatDir(string matDir)
        {
            var parts = matDir.Split('/');
            var cur   = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                var next = $"{cur}/{parts[i]}";
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(cur, parts[i]);
                cur = next;
            }
        }
    }
}
