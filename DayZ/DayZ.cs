using System;
using System.Collections;

using Rocket.Core.Plugins;
using RocketLogger = Rocket.Core.Logging.Logger;

using ChubbyQuokka.DayZ.Managers;
using UnityEngine;

namespace ChubbyQuokka.DayZ
{
    public sealed class DayZ : RocketPlugin<DayZConfiguration>
    {
        internal static DayZ Instance { get; private set; }

        protected override void Load()
        {
            Instance = this;

            ThreadManager.Initialize();
            MySQLManager.Initialize();
            EventManager.Initialize();
            PatchingManager.Initialize();
            HumanityManager.Initialize();
            
            InvokeRepeating("Cache", 10f, 10f);

            RocketLogger.Log(string.Format("The Project South Zagoria plugin v{0} has initialized!", Assembly.GetName().Version), ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            CancelInvoke("Cache");

            HumanityManager.Destroy();
            PatchingManager.Destroy();
            EventManager.Destroy();
            MySQLManager.Destroy();
            ThreadManager.Destroy();

            Instance = null;
        }

        private void Update()
        {
            ThreadManager.Update();
        }

        void Cache()
        {
            ThreadManager.ExecuteWorker(() =>
            {
                HumanityManager.Refresh();
            });
        }

        internal static void Log(string message, ConsoleColor color = ConsoleColor.Yellow)
        {
            Action action = () =>
            {
                RocketLogger.Log(message, color);
            };

            if (ThreadManager.IsWorkerThread)
            {
                ThreadManager.ExecuteMain(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
