using System.IO;
using UnityEditor;
using UnityEngine;
using SignTrainer.Data;

namespace SignTrainer.EditorTools
{
    public static class CreateMvpSignData
    {
        private const string PackAnimDir = "Assets/TheMightyCat/AmericanSignLanguageAnimationPack/Animations";
        private const string OutputDir = "Assets/ScriptableObjects/Signs";

        private struct Entry
        {
            public string id, display, meaning, fbxFile;
            public SignCategory category;
            public int module;
            public Entry(string id, string display, string meaning, SignCategory c, int module, string fbx)
            { this.id = id; this.display = display; this.meaning = meaning; category = c; this.module = module; fbxFile = fbx; }
        }

        private static readonly Entry[] Mvp = new[]
        {
            new Entry("SGN-001","Hello","Greet someone in a friendly way",SignCategory.Greeting,1,"AS_JustHands_ASL_Hello.FBX"),
            new Entry("SGN-002","Thank you","Express gratitude",SignCategory.Greeting,1,"AS_JustHands_ASL_ThankYou.FBX"),
            new Entry("SGN-003","Please","Polite request opener",SignCategory.Politeness,1,"AS_JustHands_ASL_Please.FBX"),
            new Entry("SGN-004","Yes","Affirm a statement or request",SignCategory.Affirmation,1,"AS_JustHands_ASL_Yes.FBX"),
            new Entry("SGN-005","No","Decline or negate",SignCategory.Negation,1,"AS_JustHands_ASL_No.FBX"),
            new Entry("SGN-006","Sorry","Apologise",SignCategory.Apology,1,"AS_JustHands_ASL_Sorry.FBX"),
            new Entry("SGN-007","Help","Ask for or offer help",SignCategory.Basics,1,"AS_JustHands_ASL_Help.FBX"),
            new Entry("SGN-019","Good","Affirm quality — fine, well",SignCategory.Affirmation,1,"AS_JustHands_ASL_Good.FBX"),
            new Entry("SGN-020","GoodBye","Say farewell",SignCategory.Greeting,1,"AS_JustHands_ASL_GoodBye.FBX"),
            new Entry("SGN-021","Friend","Indicate friendship",SignCategory.Social,1,"AS_JustHands_ASL_Friend.FBX"),
            new Entry("SGN-022","Stop","Signal someone to stop",SignCategory.Command,1,"AS_JustHands_ASL_Stop.FBX"),
            new Entry("SGN-023","Wait","Ask someone to wait",SignCategory.Command,1,"AS_JustHands_ASL_Wait.FBX")
        };

        [MenuItem("SignTrainer/Generate MVP SignData Assets")]
        public static void Generate()
        {
            if (!Directory.Exists(OutputDir)) Directory.CreateDirectory(OutputDir);
            AssetDatabase.Refresh();

            int created = 0, updated = 0, skipped = 0;
            foreach (var entry in Mvp)
            {
                string safeName = entry.display.Replace(" ", "");
                string assetPath = $"{OutputDir}/{entry.id}_{safeName}.asset";
                var sd = AssetDatabase.LoadAssetAtPath<SignData>(assetPath);
                bool existed = sd != null;
                if (!existed) sd = ScriptableObject.CreateInstance<SignData>();

                sd.signId = entry.id;
                sd.displayName = entry.display;
                sd.meaning = entry.meaning;
                sd.category = entry.category;
                sd.module = entry.module;
                sd.isMvp = true;

                string fbxPath = $"{PackAnimDir}/{entry.fbxFile}";
                var clip = FindAnimationClip(fbxPath);
                if (clip == null)
                {
                    Debug.LogWarning($"[CreateMvpSignData] No AnimationClip in {fbxPath} for {entry.id}. Wire manually later.");
                    skipped++;
                }
                else
                {
                    sd.demoAnimation = clip;
                }

                if (!existed) { AssetDatabase.CreateAsset(sd, assetPath); created++; }
                else { EditorUtility.SetDirty(sd); updated++; }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[CreateMvpSignData] Done. Created {created}, Updated {updated}, Missing-clip {skipped}.");
        }

        private static AnimationClip FindAnimationClip(string fbxPath)
        {
            var all = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
            if (all == null) return null;
            foreach (var a in all)
            {
                if (a is AnimationClip clip && !clip.name.StartsWith("__preview__"))
                    return clip;
            }
            return null;
        }
    }
}
