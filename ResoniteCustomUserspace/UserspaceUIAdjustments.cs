// UI scaling and preview adjustments for Userspace UI Overhaul

using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;
using UIXImage = FrooxEngine.UIX.Image;
using HarmonyLib;
using ResoniteModLoader;
using System;

namespace UserspaceOverhaul {
	public static class UserspaceUIAdjustments {
		private static ModConfiguration config => UserspaceOverhaul.config;

		private static float3 ToUIScale(float userScale) {
			float clamped = MathX.Clamp(userScale, 0.2f, 2.0f);
			float baseScale = 0.0028f;
			float actual = baseScale * clamped;
			return new float3(actual, actual, actual);
		}

		private static float MapQuadScale(float userScale) {
			float clamped = MathX.Clamp(userScale, 0.0f, 2.0f);
			if (clamped < 1.0f)
				return MathX.Lerp(0.2f, 1.0f, clamped);
			else
				return MathX.Lerp(1.0f, 3.0f, clamped - 1.0f);
		}

		private static float ClampSphereRadius(float userScale) {
			float baseRadius = 0.04f;
			float clamped = MathX.Clamp(userScale, 0.25f, 3.75f);
			float radius = baseRadius * clamped;
			return MathX.Clamp(radius, 0.01f, 0.15f);
		}

		[HarmonyPatch(typeof(InspectorHelper), nameof(InspectorHelper.SetupProxyVisual))]
		private static class ReferenceProxyPatch {
			[HarmonyPostfix]
			public static void ModifyFinal(Slot slot) {
				try {
					if (!config.GetValue(UserspaceOverhaul.Enabled)) return;

					slot.ReferenceID.ExtractIDs(out var position, out var user);
					var world = slot.World;
					if (world == null) return;
					User userByAllocationID = world.GetUserByAllocationID(user);
					if (userByAllocationID != world.LocalUser) return;

					Slot labelSlot = null;
					Slot previewSlot = null;

					foreach (var child in slot.Children) {
						if (child.Name == "Label" && child.GetComponent<Canvas>() != null) {
							labelSlot = child;
						} else if (previewSlot == null && child.Name != "Label" && child.GetComponent<Canvas>() == null) {
							previewSlot = child;
						}
					}

					if (previewSlot != null) {
						float userScale = config.GetValue(UserspaceOverhaul.previewScale);
						if (previewSlot.Name == "Quad") {
							float quadScale = MapQuadScale(userScale);
							previewSlot.LocalScale = new float3(quadScale, quadScale, quadScale);
						} else if (previewSlot.Name != "Mesh") {
							float3 previewScaleVec = ToUIScale(userScale);
							previewSlot.LocalScale = previewScaleVec;
						}
					}

					var sphereMesh = slot.GetComponent<SphereMesh>();
					if (sphereMesh != null) {
						float radius = ClampSphereRadius(config.GetValue(UserspaceOverhaul.previewScale));
						sphereMesh.Radius.Value = radius;
					}

					if (labelSlot != null) {
						float3 labelScale = ToUIScale(config.GetValue(UserspaceOverhaul.uiScale));
						labelSlot.LocalScale = labelScale;

						var img = labelSlot.GetComponentInChildren<UIXImage>();
						if (img != null) img.Tint.Value = config.GetValue(UserspaceOverhaul.uiColor);

						var text = labelSlot.GetComponentInChildren<Text>();
						if (text != null) {
							text.Color.Value = config.GetValue(UserspaceOverhaul.uiFontColor);
							var fontChain = slot.GetComponent<FontChain>() ?? slot.AttachComponent<FontChain>();
							string fontUrl = config.GetValue(UserspaceOverhaul.uiFont);
							if (!string.IsNullOrWhiteSpace(fontUrl)) {
								var staticFont = slot.GetComponent<StaticFont>() ?? slot.AttachComponent<StaticFont>();
								staticFont.URL.Value = new Uri(fontUrl, UriKind.RelativeOrAbsolute);
								fontChain.MainFont.Target = staticFont;
								text.Font.Target = fontChain;
							} else {
								Slot rootSlot = slot;
								while (rootSlot.Parent != null) rootSlot = rootSlot.Parent;
								var rootFontChain = rootSlot.GetComponentInChildren<FontChain>();
								text.Font.Target = rootFontChain ?? null;
							}
						}
					}
				} catch (Exception ex) {
					UserspaceOverhaul.Error(ex);
				}
			}
		}
	}
}
