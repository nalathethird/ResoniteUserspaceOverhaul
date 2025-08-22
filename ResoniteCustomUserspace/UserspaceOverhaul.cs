using System;

using Elements.Core;

using FrooxEngine;
using FrooxEngine.UIX;

using HarmonyLib;

using ResoniteModLoader;

namespace UserspaceOverhaul {
	// Orb scaling features adapted from:
	// - ScalableWorldOrbs by Delta (https://github.com/XDelta/ScalableWorldOrbs)
	// - ScalableMaterialOrbs by AlexW-578 (https://github.com/AlexW-578/ScalableMaterialOrbs)
	// - MeshOrb scaling and spinner disabling by Me (NalaTheThird)

	public class UserspaceOverhaul : ResoniteMod {
		internal const string VERSION_CONSTANT = "1.0.4a";
		public override string Name => "UserspaceOverhaul";
		public override string Author => "NalaTheThird";
		public override string Version => VERSION_CONSTANT;
		public override string Link => "https://github.com/nalathethird/ResoniteUserspaceUI-Overhaul/";

		[AutoRegisterConfigKey]
		public static readonly ModConfigurationKey<bool> Enabled = new(
			"Enabled", "Enable Userspace UI Overhaul", () => true
		);
		[AutoRegisterConfigKey]
		public static readonly ModConfigurationKey<float> uiScale = new(
			"uiScale", "UIX Label Scale (1.0 = default)", () => 1.0f
		);
		[AutoRegisterConfigKey]
		public static readonly ModConfigurationKey<float> previewScale = new(
			"previewScale", "Reference Preview Scale (1.0 = default)", () => 1.0f
		);
		[AutoRegisterConfigKey]
		public static readonly ModConfigurationKey<colorX> uiColor = new(
			"uiColor", "UI Backing Color", () => colorX.Black
		);
		[AutoRegisterConfigKey]
		public static readonly ModConfigurationKey<string> uiFont = new(
			"uiFont", "UI Font Asset URL (resdb asset URL) - can be left blank for Default Font", () => ""
		);
		[AutoRegisterConfigKey]
		public static readonly ModConfigurationKey<colorX> uiFontColor = new(
			"uiFontColor", "UI Font Color", () => colorX.White
		);
		[AutoRegisterConfigKey]
		public static readonly ModConfigurationKey<bool> EnableWorldOrbScaling = new(
			"EnableWorldOrbScaling", "Enable scaling for World Orbs", () => true
		);
		[AutoRegisterConfigKey]
		public static readonly ModConfigurationKey<bool> EnableMaterialOrbScaling = new(
			"EnableMaterialOrbScaling", "Enable scaling for Material Orbs", () => true
		);
		[AutoRegisterConfigKey]
		public static readonly ModConfigurationKey<bool> EnableMeshOrbScaling = new(
			"EnableMeshOrbScaling", "Enable scaling for Mesh Orbs", () => true
		);
//		[AutoRegisterConfigKey]
//		public static readonly ModConfigurationKey<bool> DisableGizmoSnapping = new(
//			"DisableGizmoSnapping", "Disable Gizmo snapping functionality", () => true
//		);
//		[AutoRegisterConfigKey]
//		public static readonly ModConfigurationKey<bool> EnableGizmoVisibilityToggle = new(
//			"EnableGizmoVisibilityToggle", "Enable the option to toggle visibility of your own Gizmos for others", () => true
//		);
//		[AutoRegisterConfigKey]
//		public static readonly ModConfigurationKey<string> CustomDashBackground = new(
//			"CustomDashBackground", "Custom background for Radiant Dash (resdb asset URL) - leave blank to use default", () => ""
//
//		[AutoRegisterConfigKey]
//		public static readonly ModConfigurationKey<bool> EnableCustomDashBackground = new(
//			"EnableCustomDashBackground", "Enable custom Radiant Dash background", () => false
//		);
//		[AutoRegisterConfigKey]
//		public static readonly ModConfigurationKey<bool> PreserveBackgroundAspectRatio = new(
//			"PreserveBackgroundAspectRatio", "Preserve aspect ratio of the custom background image", () => true
//		);
//		[AutoRegisterConfigKey]
//		public static readonly ModConfigurationKey<float> BackgroundTransparency = new(
//			"BackgroundTransparency", "Background transparency level (0.0 = fully transparent, 1.0 = fully opaque)", () => 1.0f
//		);

		public static ModConfiguration config;
		public override void OnEngineInit() {
			Msg($"[{Name}] Initializing v{Version} by {Author}");
			config = GetConfiguration();
			config.Save(true);
			Msg($"[{Name}] Config loaded: Enabled={config.GetValue(Enabled)}, uiColor={config.GetValue(uiColor)}, uiSize={config.GetValue(uiScale)}, uiFont={config.GetValue(uiFont)}, uiFontColor={config.GetValue(uiFontColor)}");

			var harmony = new Harmony("com.nalathethird.UserspaceUIOverhaul");
			harmony.PatchAll();

			Msg($"[{Name}] All Harmony patches applied.");
			Msg($"[{Name}] ScalableMaterialOrbs by AlexW-578 (https://github.com/AlexW-578/ScalableMaterialOrbs)");
			Msg($"[{Name}] ScalableWorldOrbs by Delta (https://github.com/XDelta/ScalableWorldOrbs)");
		}
		public static void Error(Exception ex) {
			ResoniteModLoader.ResoniteMod.Error(ex);
		}
	}
}
