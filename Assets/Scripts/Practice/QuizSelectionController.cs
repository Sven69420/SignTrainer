using UnityEngine;
using SignTrainer.Data;

namespace SignTrainer.Practice
{
    public class QuizSelectionController : MonoBehaviour
    {
        [SerializeField] private PracticeManager practiceManager;
        [SerializeField] private SignData correctAnswer;

        private SignData selectedAnswer;

        public void SelectAnswer(SignData sign)
        {
            selectedAnswer = sign;
        }

        public void ConfirmAnswer()
        {
            if (selectedAnswer == null || practiceManager == null) return;
            practiceManager.RecordAttempt(selectedAnswer.signId == correctAnswer.signId);
        }
    }
}