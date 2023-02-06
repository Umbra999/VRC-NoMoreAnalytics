using AmplitudeSDKWrapper;
using System;
using VRC.Core;
using System.Reflection;
using HarmonyLib;
using System.Linq;

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
            CreatePatch(typeof(AnalyticsSDK).GetFromMethod(nameof(AnalyticsSDK.Initialize)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsSDK).GetFromMethod(nameof(AnalyticsSDK.AvatarUploaded)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsSDK).GetFromMethod(nameof(AnalyticsSDK.WorldUploaded)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsSDK).GetFromMethod(nameof(AnalyticsSDK.Initialize)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsSDK).GetFromMethod(nameof(AnalyticsSDK.CheckInit)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(Analytics).GetFromMethod(nameof(Analytics.Update)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsInterface).GetFromMethod(nameof(AnalyticsInterface.Initialize)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsInterface).GetFromMethod(nameof(AnalyticsInterface.CheckInstance)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsInterface).GetFromMethod(nameof(AnalyticsInterface.SetUserProperties)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsInterface).GetFromMethod(nameof(AnalyticsInterface.SetBuildVersion)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsInterface).GetFromMethod(nameof(AnalyticsInterface.SetLogger)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(AnalyticsInterface).GetFromMethod(nameof(AnalyticsInterface.SetUserId)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(SessionManager).GetFromMethod(nameof(SessionManager.Awake)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(DatabaseHelper).GetFromMethod(nameof(DatabaseHelper.LoadFromCache)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(DatabaseHelper).GetFromMethod(nameof(DatabaseHelper.SaveToCache)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(DatabaseHelper).GetFromMethod(nameof(DatabaseHelper.GetCacheFilePath)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(DatabaseHelper).GetFromMethod(nameof(DatabaseHelper.GetEvents)), GetPatch(nameof(ReturnFalsePrefix)));
            CreatePatch(typeof(DatabaseHelper).GetFromMethod(nameof(DatabaseHelper.AddEvent)), GetPatch(nameof(ReturnFalsePrefix)));
            foreach (MethodInfo Method in typeof(AmplitudeWrapper).GetMethods().Where(x => x.Name.Contains("Init") || x.Name.Contains("UpdateServer") || x.Name.Contains("PostEvents") || x.Name.Contains("SaveEvent") || x.Name.Contains("SaveAndUpload") || x.Name.Contains("SetUser") || x.Name.Contains("Session")))
            {
                CreatePatch(Method, GetPatch(nameof(ReturnFalsePrefix)));
            }
            foreach (MethodInfo method in typeof(AnalyticsInterface).GetMethods().Where(x => x.Name == "Send"))
            {
                CreatePatch(method, GetPatch(nameof(ReturnFalsePrefix)));
            }
            foreach (MethodInfo method in typeof(AnalyticsSDK).GetMethods().Where(x => x.Name == "LoggedInUserChanged"))
            {
                CreatePatch(method, GetPatch(nameof(ReturnFalsePrefix)));
            }
            foreach (MethodInfo Method in typeof(UnityEngine.Analytics.Analytics).GetMethods().Where(x => x.Name.Contains("SendCustom") || x.Name.Contains("Queue")))
            {
                CreatePatch(Method, GetPatch(nameof(ReturnFalsePrefix)));
            }
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
