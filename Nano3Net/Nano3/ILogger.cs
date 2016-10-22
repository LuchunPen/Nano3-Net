/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 09.06.2016 23:56:41
*/

using System;

namespace Nano3
{
    public enum LogType
    {
        Log, 
        Warning,
        Exception,
        Error
    }

    public interface ILogger
    {
        void Log(string message, LogType logType);
        void Log(object message, LogType logType);
    }
}
