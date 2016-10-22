/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 16.06.2016 19:00:37
*/

using System;
using System.Collections;
using System.Collections.Generic;

using Nano3.Collection;

namespace Nano3.Net
{
    public class CommandStorage : ICommandStorage
    {
        private FastDictionaryM2<long, ICommand> _cStorage = new FastDictionaryM2<long, ICommand>();
        private Dictionary<string, ICommand> _cStorageByName = new Dictionary<string, ICommand>();

        public ICommand this[long comID]
        {
            get
            {
                ICommand com; _cStorage.TryGetValue(comID, out com);
                return com;
            }
        }
        public ICommand this[string comName]
        {
            get
            {
                ICommand com; _cStorageByName.TryGetValue(comName, out com);
                return com;
            }
        }

        public CommandStorage() { }

        public bool Contains(long commandID)
        {
            return _cStorage.ContainsKey(commandID);
        }

        public void Add(ICommand command)
        {
            if (command != null)
            {
                long commandID = command.UniqueID;
                string cName = command.Name;

                ICommand ecom; _cStorage.TryGetValue(commandID, out ecom);
                if (ecom != null) {
                    NetLogger.Log("CommandID " + commandID +" is already registered by name: " + ecom.Name, LogType.Error);
                    return;
                }

                _cStorageByName.TryGetValue(cName, out ecom);
                if (ecom != null) {
                    NetLogger.Log("CommandName " + cName + " is already registered by ID: " + ecom.UniqueID, LogType.Error);
                    return;
                }

                _cStorage.Add(commandID, command);
                _cStorageByName.Add(cName, command);
            }
        }

        public ICommand Remove(long commandID)
        {
            ICommand ecom; _cStorage.TryGetValue(commandID, out ecom);
            if (ecom != null)
            {
                _cStorage.Remove(commandID);
                _cStorageByName.Remove(ecom.Name);
            }
            return ecom;
        }

        public bool Execute(IConnector sender, ICommandArg comArg)
        {
            if (comArg == null || sender == null) return false;
            ICommand com; _cStorage.TryGetValue(comArg.CommandID, out com);
            if (com != null) { com.Execute(sender, comArg); return true; }
            else { return false; }
        }

        public bool Execute(IConnector sender, byte[] comArgData)
        {
            if (sender == null) return false;
            long commandID = ComArgHelper.GetCommandID(comArgData);
            if (commandID == 0) return false;

            ICommand com; _cStorage.TryGetValue(commandID, out com);
            if (com != null) { com.Execute(sender, comArgData); return true; }
            else { return false; }
        }

        public void AddExecuteAction<T>(long commandID, Action<IConnector, T> executeAction)
            where T : ICommandArg
        {
            ICommand com; _cStorage.TryGetValue(commandID, out com);
            if (com != null) { com.AddExecuteAction(executeAction); }
            else { AppLogger.Log("Command with this commandID is not registered"); }
        }

        public void RemoveExecuteAction<T>(long commandID, Action<IConnector, T> executeAction)
            where T : ICommandArg
        {
            ICommand com; _cStorage.TryGetValue(commandID, out com);
            if (com != null) { com.RemoveExecuteAction(executeAction); }
            else { AppLogger.Log("Command with this commandID is not registered"); }
        }

        public ICommand[] GetAllCommands()
        {
            return _cStorage.GetValues();
        }

        public IEnumerator<KeyValuePair<long, ICommand>> GetEnumerator()
        {
            return _cStorage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Clear()
        {
            _cStorage.Clear();
            _cStorageByName.Clear();
        }
    }
}
