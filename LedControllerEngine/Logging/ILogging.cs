using System;

namespace LedControllerEngine
{
    public interface ILogging
    {
        void Info(string message);
        void Warning(string message);
        void Debug(string message);
        void Error(string message);
        void Error(string message, Exception ex);
        void Error(Exception ex);
        void Fatal(string message);
        void Fatal(Exception ex);
    }
}
