/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 25/05/2016 15:01
*/

using System;

namespace Nano3
{
    public class AppLogger
    {
        //private static readonly string stringID = "DCCE443A964CB502";

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
            if (_logger == null) return;
            _logger.Log(message, LogType.Log);

            LogMessageEvent?.Invoke(_logger, message);
        }

        public static void Log(object message)
        {
            if (_logger == null) return;
            _logger.Log(message, LogType.Log);

            LogObjectEvent?.Invoke(_logger, message);
        }

        public static void Log(string message, LogType type)
        {
            if (_logger == null) return;
            _logger.Log(message, type);

            LogMessageEvent?.Invoke(_logger, message);
        }

        public static void Log(object message, LogType type)
        {
            if (_logger == null) return;
            _logger.Log(message, type);

            LogObjectEvent?.Invoke(_logger, message);
        }
    }
}

