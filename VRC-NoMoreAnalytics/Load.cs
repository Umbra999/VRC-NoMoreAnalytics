using MelonLoader;
using System.Collections;
using UnityEngine;

namespace VRC_NoMoreAnalytics
{
    public class Load : MelonMod
    {
        public override void OnInitializeMelon()
        {
            Patches.ApplyPatches();
            MelonCoroutines.Start(DestroyAnalytics());
        }

        private static IEnumerator DestroyAnalytics() // Kill the Analytics Instance as soonest the GameObject gets created in case they add more checks
        {
            while (VRCApplication.prop_VRCApplication_0 == null) yield return null;

            foreach (Analytics analytics in Resources.FindObjectsOfTypeAll<Analytics>())
            {
                Object.DestroyImmediate(analytics);
            }

            foreach (SessionManager analytics in Resources.FindObjectsOfTypeAll<SessionManager>())
            {
                Object.DestroyImmediate(analytics);
            }
        }
    }
}
