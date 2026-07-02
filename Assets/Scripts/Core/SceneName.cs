namespace SignTrainer.Core
{
    public enum SceneName
    {
        Core,
        Boot,
        MainMenu,
        Tutorial,
        Practice,
        PracticeName,
        PracticeQuiz,
        PracticeLetters,
        PracticeQuizLetters,
        Scenario_Greeting,
        Scenario_Help,
        Scenario_Library,
        Scenario_Street,
        Scenario_Store
    }

    public static class SceneNameExtensions
    {
        public static string AsUnityName(this SceneName name) => name.ToString();
    }
}
