using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SignTrainer.Data;

namespace SignTrainer.Practice
{
    public class RandomQuizUIController : MonoBehaviour
    {
        [SerializeField] private ExerciseData exercise;
        [SerializeField] private TMP_Text promptLabel;
        [SerializeField] private Button[] optionButtons;
        [SerializeField] private TMP_Text[] optionLabels;
        [SerializeField] private Button confirmButton;
        [SerializeField] private FeedbackController feedback;
        [SerializeField] private GameObject demoPlayerObject;

        private SignData correctAnswer;
        private SignData selectedAnswer;

        private void Start()
        {
            StartNewQuestion();
            confirmButton?.onClick.AddListener(ConfirmAnswer);
        }

        public void StartNewQuestion()
        {
            if (exercise == null || exercise.signPool == null || exercise.signPool.Count < 4)
                return;

            selectedAnswer = null;
            correctAnswer = exercise.signPool[Random.Range(0, exercise.signPool.Count)];

            if (promptLabel != null)
                promptLabel.text = $"Which option shows “{correctAnswer.displayName}”?";

            var choices = BuildChoices(correctAnswer);

            for (int i = 0; i < optionButtons.Length && i < choices.Count; i++)
            {
                int index = i;
                SignData sign = choices[index];

                if (optionLabels != null && index < optionLabels.Length && optionLabels[index] != null)
                    optionLabels[index].text = $"Option {(char)('A' + index)}";

                optionButtons[index].onClick.RemoveAllListeners();
                optionButtons[index].onClick.AddListener(() => SelectAnswer(sign));
            }
        }

        private void SelectAnswer(SignData sign)
        {
            selectedAnswer = sign;
            demoPlayerObject?.SendMessage("Play", sign, SendMessageOptions.DontRequireReceiver);
        }

        private void ConfirmAnswer()
        {
            if (selectedAnswer == null) return;

            bool correct = selectedAnswer.signId == correctAnswer.signId;
            feedback?.Show(correct ? FeedbackState.Correct : FeedbackState.TryAgain);

            if (correct)
                Invoke(nameof(StartNewQuestion), 1.5f);
        }

        private List<SignData> BuildChoices(SignData target)
        {
            var choices = new List<SignData> { target };
            var pool = new List<SignData>(exercise.signPool);
            pool.RemoveAll(s => s == null || s.signId == target.signId);

            while (choices.Count < 4 && pool.Count > 0)
            {
                int i = Random.Range(0, pool.Count);
                choices.Add(pool[i]);
                pool.RemoveAt(i);
            }

            Shuffle(choices);
            return choices;
        }

        private static void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}