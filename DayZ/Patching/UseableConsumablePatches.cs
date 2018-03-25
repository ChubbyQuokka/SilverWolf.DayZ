using System;
using System.Linq;
using Action = System.Action;

using ChubbyQuokka.DayZ.Managers;
using ChubbyQuokka.DayZ.Structures;
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

                    //That fucking navigation tho
                    ulong steamId = __instance.player.channel.owner.playerID.steamID.m_SteamID;

                    HumanityItem item = default(HumanityItem);
                    PlayerItemResult result = DayZConfiguration.ItemSettings.ItemUseResults.FirstOrDefault(x => x.OriginalItem == asset.id);

                    if (mode == EConsumeMode.USE)
                    {
                        item = DayZConfiguration.HumanitySettings.HumanityUse.FirstOrDefault(x => x.ItemID == asset.id);
                    }
                    else if (enemy != null)
                    {
                        item = DayZConfiguration.HumanitySettings.HumanityAid.FirstOrDefault(x => x.ItemID == asset.id);
                    }
                    
                    if (item != null)
                    {
                        HumanityManager.IncrementHumanity(steamId, item.Humanity);
                    }

                    if (result != null)
                    {
                        byte quality = __instance.player.equipment.quality;

                        Action action = () =>
                        {
                            Item temp = new Item(result.ItemID, 1, quality);
                            __instance.player.inventory.tryAddItem(temp, true);
                        };

                        ThreadManager.EnqueueMain(action);
                    }
                }
            }
        }
    }
}
