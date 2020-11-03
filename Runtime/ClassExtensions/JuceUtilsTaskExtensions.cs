using System;
using System.Threading.Tasks;

public static class JuceUtilsTaskExtensions
{
    public static async void ExecuteAsync(this Task task, Action onFinish = null)
    {
        await task;

        onFinish?.Invoke();
    }
}