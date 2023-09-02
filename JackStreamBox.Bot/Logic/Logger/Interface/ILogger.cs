using Serilog.Core;
using Serilog.Events;

public class CustomLogSink : ILogEventSink
{
    private readonly Action<LogEvent> _logAction;

    public CustomLogSink(Action<LogEvent> logAction)
    {
        _logAction = logAction;
    }

    public void Emit(LogEvent logEvent)
    {
        // Call your custom function with the log event
        _logAction(logEvent);
    }
}
