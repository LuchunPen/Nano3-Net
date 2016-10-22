/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 15.06.2016 2:30:24
*/

using System;

namespace Nano3.Net
{
    public interface ICommand : IUniq, IDescriptable
    {
        void Execute(IConnector sender, ICommandArg comArg);
        void Execute(IConnector sender, byte[] comArgData);

        void AddExecuteAction<TArg>(Action<IConnector, TArg> executeAction);
        void RemoveExecuteAction<TArg>(Action<IConnector, TArg> executeAction);
    }
}
