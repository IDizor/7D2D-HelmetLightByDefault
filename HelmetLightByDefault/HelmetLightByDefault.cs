using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Use your helmet light mod by default, instead of weapon light mod.
/// </summary>
public class HelmetLightByDefault : IModApi
{
    /// <summary>
    /// Mod initialization.
    /// </summary>
    /// <param name="_modInstance"></param>
    public void InitMod(Mod _modInstance)
    {
        Debug.Log("Loading mod: " + GetType().ToString());
        var harmony = new Harmony(GetType().ToString());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// The Harmony patch for the method <see cref="EntityAlive.GetActivatableItemPool"/>.
    /// </summary>
    [HarmonyPatch(typeof(EntityAlive))]
    [HarmonyPatch("GetActivatableItemPool")]
    public class EntityAlive_GetActivatableItemPool
    {
        /// <summary>
        /// Uses helmet light mod as default when pressing F.
        /// </summary>
        public static void Postfix(ref List<ItemValue> __result)
        {
            if (__result != null && __result.Count > 1)
            {
                var lightMod = __result.FirstOrDefault(i => i.ItemClass?.Name == "modArmorHelmetLight");
                if (lightMod != null)
                {
                    __result.Remove(lightMod);
                    __result.Insert(0, lightMod);
                }
            }
        }
    }
}
