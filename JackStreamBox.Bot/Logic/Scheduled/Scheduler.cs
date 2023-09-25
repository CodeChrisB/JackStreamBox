using System;
using System.Collections.Generic;
using System.Threading;

public class Scheduler
{
    private static Dictionary<string, Timer> timers = new Dictionary<string, Timer>();

    public static void RegisterScheduler(string key, Action action, TimeSpan interval)
    {
        TimerCallback callback = _ =>
        {
            action.Invoke();
        };

        Timer timer = new Timer(callback, null, interval, interval);

        lock (timers)
        {
            timers[key] = timer;
        }
    }

    public static void UnregisterScheduler(string key)
    {
        lock (timers)
        {
            if (timers.ContainsKey(key))
            {
				Timer timer = timers[key];
				timer.Dispose(); // This stops the timer and releases associated resources
				timers.Remove(key); // Remove the timer from the dictionary
			}
        }
    }
}