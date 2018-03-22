using System;

using Rocket.Core.Plugins;
using RocketLogger = Rocket.Core.Logging.Logger;

using ChubbyQuokka.DayZ.Managers;

namespace ChubbyQuokka.DayZ
{
    public sealed class DayZ : RocketPlugin<DayZConfiguration>
    {
        internal static DayZ Instance { get; private set; }

        protected override void Load()
        {
            Instance = this;

            ThreadManager.Initialize();
            EventManager.Initialize();

            RocketLogger.Log(string.Format("The Project South Zagoria plugin v{0} has initialized!", Assembly.GetName().Version), ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            EventManager.Destroy();
            ThreadManager.Destroy();

            Instance = null;
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
