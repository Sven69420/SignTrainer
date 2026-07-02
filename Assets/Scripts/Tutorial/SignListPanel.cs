using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SignTrainer.Data;

namespace SignTrainer.Tutorial
{
    public class SignListPanel : MonoBehaviour
    {
        [SerializeField] private Transform rowParent;
        [SerializeField] private GameObject rowPrefab;
        [SerializeField] private TutorialManager tutorial;

        private readonly List<GameObject> spawnedRows = new List<GameObject>();

        private void Start() => Build();

        public void Build()
        {
            Clear();
            if (tutorial == null || rowParent == null || rowPrefab == null) return;
            foreach (var sign in tutorial.GetSignList())
            {
                var row = Instantiate(rowPrefab, rowParent);
                var label = row.GetComponentInChildren<TMP_Text>();
                if (label != null) label.text = sign.displayName;
                var button = row.GetComponent<Button>();
                if (button != null)
                {
                    var captured = sign;
                    button.onClick.AddListener(() => tutorial.SelectSign(captured));
                }
                spawnedRows.Add(row);
            }
        }

        private void Clear()
        {
            foreach (var row in spawnedRows) if (row != null) Destroy(row);
            spawnedRows.Clear();
        }
    }
}
