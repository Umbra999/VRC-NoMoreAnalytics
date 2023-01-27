using AmplitudeSDKWrapper;
using System;
using VRC.Core;
using System.Reflection;
using HarmonyLib;

namespace VRC_NoMoreAnalytics
{
    internal static class Patches
    {
        private static readonly HarmonyLib.Harmony Instance = new HarmonyLib.Harmony("Hexed");

        private static HarmonyMethod GetPatch(string name)
        {
            return new HarmonyMethod(typeof(Patches).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic));
        }

        private static MethodInfo GetFromMethod(this Type Type, string name)
        {
            return Type.GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        }

        private static void CreatePatch(MethodBase TargetMethod, HarmonyMethod Before = null, HarmonyMethod After = null)
        {
            try
            {
                Instance.Patch(TargetMethod, Before, After);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to Patch {TargetMethod.Name} \n{e}");
            }
        }

        public static unsafe void ApplyPatches()
        {
            // Analytics
            CreatePatch(typeof(Analytics).GetFromMethod(nameof(Analytics.Awake)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(Analytics).GetFromMethod(nameof(Analytics.Update)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsInterface).GetFromMethod(nameof(AnalyticsInterface.Initialize)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AmplitudeWrapper).GetFromMethod(nameof(AmplitudeWrapper.Init)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AmplitudeWrapper).GetFromMethod(nameof(AmplitudeWrapper.StartNewSession)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AmplitudeWrapper).GetFromMethod(nameof(AmplitudeWrapper.StartSession)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AmplitudeWrapper).GetFromMethod(nameof(AmplitudeWrapper.UpdateServer)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AmplitudeWrapper).GetFromMethod(nameof(AmplitudeWrapper.UpdateServerDelayed)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AmplitudeWrapper).GetFromMethod(nameof(AmplitudeWrapper._UpdateServerDelayed_b__68_0)), GetPatch(nameof(ReturnFalsePrefix)));
        }

        private static void AllowBoolPostfix(ref bool __result)
        {
            __result = true;
        }

        private static void DisallowBoolPostfix(ref bool __result)
        {
            __result = false;
        }

        private static bool ReturnFalsePrefix()
        {
            return false;
        }
    }
}
