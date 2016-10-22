/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 16.06.2016 21:46:48
*/

using System;
using System.Collections.Generic;

namespace Nano3.Net
{
    interface ICommandStorage : IEnumerable<KeyValuePair<long, ICommand>>
    {
        ICommand this[long comID] { get; }
        ICommand this[string comName] { get; }

        bool Contains(long commandID);
        void Add(ICommand command);
        ICommand Remove(long commandID);

        bool Execute(IConnector sender, ICommandArg comArg);

        void AddExecuteAction<T>(long commandID, Action<IConnector, T> executeAction) where T : ICommandArg;
        void RemoveExecuteAction<T>(long commandID, Action<IConnector, T> executeAction) where T : ICommandArg;

        ICommand[] GetAllCommands();
        void Clear();
    }
}
