using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace AntlersHarmonypatch
{
    /// <summary>
    /// Prevents antlers from being shed while a creature and its chunk are still being loaded.
    /// </summary>
    /// <remarks>
    /// Vanilla calls OnGameTick(3.1f) directly inside OnEntityLoaded. If the calendar is inside the shed window at that moment, the antler item is spawned during chunk loading, stays visible on the client and can never be picked up. This patch only keeps the birth date setup and lets the regular game tick shed the antlers a few seconds later
    /// </remarks>
    [HarmonyPatch(typeof(EntityBehaviorAntlerGrowth), nameof(EntityBehaviorAntlerGrowth.OnEntityLoaded))]
    public static class AntlerSheddingOnLoadPatch
    {
        private static readonly Action<EntityBehaviorAntlerGrowth>? ensureBirthDateIsSet = CreateBirthDateSetter();

        private static bool hasWarnedAboutMissingBirthDateMethod;

        /// <summary>
        /// Replaces the vanilla OnEntityLoaded on the server side, so no antlers are shed during loading.
        /// </summary>
        /// <param name="__instance">The antler growth behavior of the creature that has just been loaded</param>
        /// <returns>False to skip the vanilla method, true to fall back to it on the client or if the birth date method could not be found</returns>
        public static bool Prefix(EntityBehaviorAntlerGrowth __instance)
        {
            Entity creature = __instance.entity;

            if (creature.World.Side != EnumAppSide.Server)
            {
                return true;
            }

            if (ensureBirthDateIsSet == null)
            {
                WarnOnceAboutMissingBirthDateMethod(creature);
                return true;
            }

            ensureBirthDateIsSet(__instance);
            return false;
        }

        /// <summary>
        /// Looks up the private vanilla method that gives a creature a birth date and turns it into a callable delegate.
        /// </summary>
        /// <returns>A delegate that calls ensureHasBirthDate, or null if a game update renamed or removed the method</returns>
        private static Action<EntityBehaviorAntlerGrowth>? CreateBirthDateSetter()
        {
            MethodInfo? birthDateMethod = AccessTools.Method(typeof(EntityBehaviorAntlerGrowth), "ensureHasBirthDate");

            if (birthDateMethod == null)
            {
                return null;
            }

            return AccessTools.MethodDelegate<Action<EntityBehaviorAntlerGrowth>>(birthDateMethod);
        }

        /// <summary>
        /// Writes a single warning to the server log if the patch has to fall back to the vanilla behavior.
        /// </summary>
        /// <param name="creature">The creature whose loading triggered the fallback, used to access the logger</param>
        private static void WarnOnceAboutMissingBirthDateMethod(Entity creature)
        {
            if (hasWarnedAboutMissingBirthDateMethod)
            {
                return;
            }

            hasWarnedAboutMissingBirthDateMethod = true;
            creature.Api.Logger.Warning("[Antler Harmonypatch] Could not find EntityBehaviorAntlerGrowth.ensureHasBirthDate. The vanilla loading behavior is used instead; the antler pickup bug may still occur.");
        }

    }

}