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

        Timer timer = new Timer(callback, null, TimeSpan.Zero, interval);

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
                timers[key].Dispose();
                timers.Remove(key);
            }
        }
    }
}