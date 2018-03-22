using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChubbyQuokka.DayZ.Managers;
using ChubbyQuokka.DayZ.Utils;
using Harmony;

using SDG.Unturned;

namespace ChubbyQuokka.DayZ.Patching
{
    [HarmonyPatch(typeof(UseableConsumeable))]
    [HarmonyPatch("simulate")]
    internal static class UseableConsumablePatch
    {
        static void Prefix(uint simulation, bool inputSteady, UseableConsumeable __instance)
        {
            if (PatchingManager.IsHarmonyActive)
            {
                bool isUsing = ReflectionUtil.GetPrivateField<bool>(__instance, "isUsing");
                bool isUseable = ReflectionUtil.GetPrivateField<bool>(__instance, "isUseable");

                Player player = ReflectionUtil.GetPrivateField<Player>(__instance, "player");
                Player enemy = ReflectionUtil.GetPrivateField<Player>(__instance, "enemy");

                ItemConsumeableAsset asset = (ItemConsumeableAsset)player.equipment.asset;
                EConsumeMode mode = ReflectionUtil.GetPrivateField<EConsumeMode>(__instance, "consumeMode");

                if (isUsing && isUseable && asset != null)
                {
                    if (mode == EConsumeMode.USE)
                    {
                        
                    }
                    else if (enemy != null)
                    {

                    }
                }
            }
        }
    }
}
