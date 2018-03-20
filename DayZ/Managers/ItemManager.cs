using System;
using System.Collections.Generic;
using System.Linq;

using Rocket.Unturned.Player;

using UnityEngine;
using UnityRandom = UnityEngine.Random;

using ChubbyQuokka.DayZ.Structures;

namespace ChubbyQuokka.DayZ.Managers
{
    internal static class ItemManager
    {
        public static void GiveCategoryToPlayer(UnturnedPlayer player, PlayerItemCategory category) => AddItemsToPlayer(player, GrabRandomItems(category));

        static ushort[] GrabRandomItems(PlayerItemCategory category)
        {
            if (category.Items.Length != 0)
            {
                byte amt = (byte)UnityRandom.Range(category.MinAmt, category.MaxAmt);

                if (amt != 0)
                {
                    List<ushort> ids = new List<ushort>();

                    foreach (PlayerItem item in category.Items)
                    {
                        for (ulong i = 0; i < item.Weight; i++)
                        {
                            ids.Add(item.ItemID);
                        }
                    }

                    if (category.Unique)
                    {
                        ushort id = ids.OrderBy(x => UnityRandom.Range(float.MinValue, float.MaxValue)).First();

                        ids.Clear();

                        for (int i = 0; i < amt; i++)
                        {
                            ids.Add(id);
                        }

                        return ids.ToArray();
                    }
                    else
                    {
                        while (ids.Count < amt)
                        {
                            ids.AddRange(ids);
                        }

                        return ids.OrderBy(x => UnityRandom.Range(float.MinValue, float.MaxValue)).Take(amt).ToArray();
                    }
                }
            }

            return new ushort[0];
        }

        static void AddItemsToPlayer(UnturnedPlayer player, ushort[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                player.GiveItem(ids[i], 1);
            }
        }
    }
}