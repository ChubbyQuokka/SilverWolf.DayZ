using Rocket.Core.Plugins;

using ChubbyQuokka.DayZ.Managers;

namespace ChubbyQuokka.DayZ
{
    public sealed class DayZ : RocketPlugin<DayZConfiguration>
    {
        internal static DayZ Instance { get; private set; }

        protected override void Load()
        {
            Instance = this;

            EventManager.Initialize();
        }

        protected override void Unload()
        {
            EventManager.Destroy();

            Instance = null;
        }
    }
}
