using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using SignTrainer.Data;

namespace SignTrainer.EditorTools
{
    public static class SignDataAnimationLinker
    {
        const string AnimDir  = "Assets/TheMightyCat/AmericanSignLanguageAnimationPack/Animations";
        const string SignsDir = "Assets/ScriptableObjects/Signs";

        static readonly Dictionary<string, string> SignToFbx = new()
        {
            { "SGN-001", "AS_Raw_ASL_Hello.FBX"      },
            { "SGN-002", "AS_Raw_ASL_ThankYou.FBX"   },
            { "SGN-003", "AS_Raw_ASL_Please.FBX"     },
            { "SGN-004", "AS_Raw_ASL_Yes.FBX"        },
            { "SGN-005", "AS_Raw_ASL_No.FBX"         },
            { "SGN-006", "AS_Raw_ASL_Sorry.FBX"      },
            { "SGN-007", "AS_Raw_ASL_Help.FBX"       },
            { "SGN-019", "AS_Raw_ASL_Good.FBX"       },
            { "SGN-020", "AS_Raw_ASL_GoodBye.FBX"    },
            { "SGN-021", "AS_Raw_ASL_Friend.FBX"     },
            { "SGN-022", "AS_Raw_ASL_Stop.FBX"       },
            { "SGN-023", "AS_Raw_ASL_Wait.FBX"       },
        };

        [MenuItem("SignTrainer/Setup/Link Sign Animations")]
        public static void LinkAnimations()
        {
            var guids = AssetDatabase.FindAssets("t:SignData", new[] { SignsDir });
            int linked = 0, skipped = 0;

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var sign = AssetDatabase.LoadAssetAtPath<SignData>(path);
                if (sign == null) continue;

                var key = sign.signId?.Length >= 7 ? sign.signId.Substring(0, 7) : null;
                if (key == null || !SignToFbx.TryGetValue(key, out var fbxName))
                {
                    Debug.LogWarning($"[SignDataLinker] No FBX mapping for {sign.signId} ({path})");
                    skipped++;
                    continue;
                }

                var fbxPath = $"{AnimDir}/{fbxName}";
                var clip = AssetDatabase.LoadAllAssetsAtPath(fbxPath)
                    .OfType<AnimationClip>()
                    .FirstOrDefault(c => !c.name.StartsWith("__preview__"));

                if (clip == null)
                {
                    Debug.LogWarning($"[SignDataLinker] No clip found in {fbxPath}");
                    skipped++;
                    continue;
                }

                sign.demoAnimation = clip;
                EditorUtility.SetDirty(sign);
                Debug.Log($"[SignDataLinker] {sign.signId} → {clip.name}");
                linked++;
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"[SignDataLinker] Done — {linked} linked, {skipped} skipped.");
        }
    }
}
