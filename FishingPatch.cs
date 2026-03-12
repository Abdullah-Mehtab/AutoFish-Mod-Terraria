using HarmonyLib;
using Terraria;

namespace AutoFisher
{
    public static class ModState
    {
        public static bool IsEnabled = true;
    }

    [HarmonyPatch(typeof(Player), nameof(Player.ItemCheck))]
    public class AutoFisherPatch
    {
        public static void Prefix(Player __instance)
        {
            if (__instance.whoAmI != Main.myPlayer || !ModState.IsEnabled)
                return;

            if (__instance.HeldItem == null || __instance.HeldItem.fishingPole <= 0)
                return;

            bool hasBobber = false;

            for (int i = 0; i < 1000; i++)
            {
                Projectile p = Main.projectile[i];

                if (p.active && p.owner == __instance.whoAmI && p.bobber)
                {
                    hasBobber = true;

                    if (p.ai[1] < 0f)
                    {
                        __instance.controlUseItem = true;
                        __instance.releaseUseItem = true;
                    }
                }
            }

            if (!hasBobber)
            {
                __instance.controlUseItem = true;
                __instance.releaseUseItem = true;
            }
        }
    }
}