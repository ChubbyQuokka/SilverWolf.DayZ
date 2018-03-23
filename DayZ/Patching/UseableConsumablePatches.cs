using System;

using ChubbyQuokka.DayZ.Managers;
using ChubbyQuokka.DayZ.Utils;

using Harmony;

using SDG.Unturned;

namespace ChubbyQuokka.DayZ.Patching
{
    [HarmonyPatch(typeof(UseableConsumeable))]
    [HarmonyPatch("simulate")]
    [HarmonyPatch(new Type[] { typeof(uint), typeof(bool) })]
    internal static class UseableConsumable_simulate
    {
        static void Prefix(uint simulation, bool inputSteady, UseableConsumeable __instance)
        {
            if (PatchingManager.IsHarmonyActive)
            {
                bool isUsing = ReflectionUtil.GetPrivateField<bool>(__instance, "isUsing");
                bool isUseable = ReflectionUtil.GetPrivateProperty<bool>(__instance, "isUseable");

                ItemConsumeableAsset asset = (ItemConsumeableAsset)__instance.player.equipment.asset;

                if (isUsing && isUseable && asset != null)
                {
                    Player enemy = ReflectionUtil.GetPrivateField<Player>(__instance, "enemy");
                    EConsumeMode mode = ReflectionUtil.GetPrivateField<EConsumeMode>(__instance, "consumeMode");

                    if (mode == EConsumeMode.USE)
                    {
                        DayZ.Log($"USE: {asset.itemName}");
                    }
                    else if (enemy != null)
                    {
                        DayZ.Log($"AID: {asset.itemName}");
                    }
                }
            }
        }
    }
}
