// WorldOrb scaling for Userspace UI Overhaul
// Adapted from ScalableWorldOrbs by Delta (https://github.com/XDelta/ScalableWorldOrbs)

using HarmonyLib;
using FrooxEngine;

namespace UserspaceOverhaul {
	[HarmonyPatch(typeof(WorldOrb), "SetupOrb")]
	public static class WorldOrb_SetupOrb_Patch {
		static void Postfix(WorldOrb __instance) {
			if (!UserspaceOverhaul.config.GetValue(UserspaceOverhaul.EnableWorldOrbScaling)) return;
			var grabbable = __instance.Slot.GetComponent<Grabbable>();
			if (grabbable != null) grabbable.Scalable.Value = true;
		}
	}
}
