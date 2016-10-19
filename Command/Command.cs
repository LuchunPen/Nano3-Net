/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 14.06.2016 18:19:37
*/

using System;

namespace Nano3.Net
{
    public abstract class Command<TArg> : ICommand
        where TArg : ICommandArg, new()
    {
        public abstract long UniqueID { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }

        protected Action<IConnector, TArg> _executor;

        public Command(Action<IConnector, TArg> executor)
        {
            if (executor != null) { _executor = executor; }
        }

        public void Execute(IConnector sender, ICommandArg comArg)
        {
            if (sender == null) return;
            if (comArg != null && comArg is TArg)
            {
               _executor?.Invoke(sender, (TArg)comArg);
            }
        }
        public void Execute(IConnector sender, byte[] comArgData)
        {
            if (sender == null) return;

            TArg arg = new TArg();
            if (arg.UnPack(comArgData))
            {
                _executor?.Invoke(sender, arg);
            }
        }

        public void AddExecuteAction<T>(Action<IConnector, T> executeAction)
        {
            if (executeAction != null && executeAction is Action<IConnector, TArg>)
            {
                _executor += executeAction as Action<IConnector, TArg>;
            }
            else { AppLogger.Log("Wrong execute action for " + Name +  " command", LogType.Error); }
        }

        public void RemoveExecuteAction<T>(Action<IConnector, T> executeAction)
        {
            if (executeAction != null && executeAction is Action<IConnector, TArg>)
            {
                _executor -= executeAction as Action<IConnector, TArg>;
            }
            else { AppLogger.Log("Wrong execute action for " + Name + " command", LogType.Error); }
        }

        public override string ToString()
        {
            return Name + ", id: " + UniqueID + " [" + Description + "]";
        }
        public override int GetHashCode()
        {
            return UniqueID.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            Command<TArg> com = obj as Command<TArg>;
            if (com != null) { return com.UniqueID == UniqueID; }
            else { return false; }
        }
    }
}
