using ModelCalculater.DEfinitions;

namespace UI.Utils
{
    public static class Extensions
    {
        public static string GetName(this TaskType taskType)
        {
            return taskType switch
            {
                TaskType.NoSolution => "The task has no solutions",
                TaskType.Estimated => "The task is calculated",
                TaskType.Optimization => "Optimization task",
                _ => throw new NotImplementedException(),
            };
        }

        public static string GetSoundName(this TaskType taskType)
        {
            return taskType switch
            {
                TaskType.NoSolution => "no-solutions.mp3",
                TaskType.Estimated => "estimated.mp3",
                TaskType.Optimization => "optimization.mp3",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
