using System.Reflection;

using Harmony;

namespace ChubbyQuokka.DayZ.Managers
{
    internal static class PatchingManager
    {
        static HarmonyInstance Instance;

        public static bool IsHarmonyActive { get; private set; } = false;
             
        public static void Initialize()
        {
            IsHarmonyActive = true;

            if (Instance == null)
            {
                Instance = HarmonyInstance.Create("net.chubbyquokka.SilverWolf.DayZ");

                Instance.PatchAll(Assembly.GetExecutingAssembly());
            }
        }

        public static void Destroy()
        {
            IsHarmonyActive = false;
        }
    }
}
