// MaterialOrb scaling for Userspace UI Overhaul
// Adapted from ScalableMaterialOrbs by AlexW-578 (https://github.com/AlexW-578/ScalableMaterialOrbs)

using HarmonyLib;
using FrooxEngine;

namespace UserspaceOverhaul {
	[HarmonyPatch(typeof(MaterialOrb), "ConstructMaterialOrb", typeof(IAssetProvider<Material>), typeof(Slot), typeof(float))]
	public static class MaterialOrb_ConstructMaterialOrb_Patch {
		static void Postfix(Slot slot) {
			if (!UserspaceOverhaul.config.GetValue(UserspaceOverhaul.EnableMaterialOrbScaling)) return;
			var grabbable = slot.GetComponent<Grabbable>();
			if (grabbable != null) grabbable.Scalable.Value = true;
		}
	}
}
