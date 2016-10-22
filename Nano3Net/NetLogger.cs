/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 09.06.2016 23:55:32
*/

using System;

namespace Nano3.Net
{
    public static class NetLogger
    {
        //private static readonly string stringUID = "FD0CBE370DC76201";

        private static ILogger _logger;
        public static ILogger Logger { get { return _logger; } }

        public static EventHandlerArg<string> LogMessageEvent;
        public static EventHandlerArg<object> LogObjectEvent;

        public static void Initialize(ILogger logger)
        {
            if (_logger != null) return;
            if (logger == null) { throw new ArgumentNullException("Logger is null"); }
            _logger = logger;
        }

        public static void Log(string message)
        {
            if (_logger == null || message == null) return;
            _logger.Log(message, LogType.Log);

            LogMessageEvent?.Invoke(_logger, message);
        }
        public static void Log(string message, LogType logType)
        {
            if (_logger == null || message == null) return;
            _logger.Log(message, logType);

            LogMessageEvent?.Invoke(_logger, message);
        }
        public static void Log(object message)
        {
            if (_logger == null || message == null) return;
            _logger.Log(message, LogType.Log);

            LogObjectEvent?.Invoke(_logger, message);
        }
        public static void Log(object message, LogType logType)
        {
            if (_logger == null || message == null) return;
            _logger.Log(message, logType);

            LogObjectEvent?.Invoke(_logger, message);
        }
    }
}
