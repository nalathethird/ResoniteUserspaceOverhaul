// MeshOrb scaling and spinner disabling for Userspace UI Overhaul
// MeshOrb patch logic by me (NalaTheThird)

using HarmonyLib;
using FrooxEngine;

namespace UserspaceOverhaul {
	[HarmonyPatch(typeof(MeshOrb), "ConstructMeshOrb")]
	public static class MeshOrb_ConstructMeshOrb_Patch {
		static void Postfix(Slot __result) {
			if (!UserspaceOverhaul.config.GetValue(UserspaceOverhaul.EnableMeshOrbScaling)) return;
			var grabbable = __result.GetComponent<Grabbable>();
			if (grabbable != null) grabbable.Scalable.Value = true;

			var meshPreview = __result.FindChild("MeshPreview");
			if (meshPreview != null) {
				var spinner = meshPreview.GetComponent<Spinner>();
				if (spinner != null) spinner.Enabled = false;
			}
		}
	}
}
