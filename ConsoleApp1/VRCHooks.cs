using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HarmonyLib;
using UnhollowerRuntimeLib.XrefScans;
using static MelonLoader.MelonLogger;

namespace NSFWSecurity
{
  static class VRCHooks
  {
    public static event EventHandler<VRCPlayer> AvatarIsReady;
    // public static event EventHandler<string> PuppetChannelAssigned;
    // public static event EventHandler<string> PuppetChannelCleared;
    // public static string CurrentPuppeteerParameter = null;

    private static MethodInfo mAssignPuppetChannel;
    private static MethodInfo mClearPuppetChannel;

    public static void OnApplicationStart(HarmonyLib.Harmony harmony)
    {
      // Hook OnVRCPlayerAwake
      harmony.Patch(typeof(VRCPlayer).GetMethod(nameof(VRCPlayer.Awake), BindingFlags.Public | BindingFlags.Instance), 
        postfix: new HarmonyMethod(typeof(VRCHooks).GetMethod(nameof(OnVRCPlayerAwake), BindingFlags.NonPublic | BindingFlags.Static)));

      // Hook action menu puppeteering
      // TODO: Xref scan?
      // try {
      //   harmony.Patch(typeof(ActionMenu).GetMethod(nameof(ActionMenu.Method_Public_Void_Parameter_1), BindingFlags.Public | BindingFlags.Instance),
      //     postfix: new HarmonyMethod(typeof(VRCHooks).GetMethod(nameof(OnAssignPuppetChannel), BindingFlags.NonPublic | BindingFlags.Static)));
      //   harmony.Patch(typeof(ActionMenu).GetMethod(nameof(ActionMenu.Method_Public_Void_Parameter_0), BindingFlags.Public | BindingFlags.Instance), 
      //     postfix: new HarmonyMethod(typeof(VRCHooks).GetMethod(nameof(OnClearPuppetChannel), BindingFlags.NonPublic | BindingFlags.Static)));
      // } catch (Exception e) {
      //   Error("Failed to hook ActionMenu puppeteering");
      //   Error(e);
      // }

      try {
        mAssignPuppetChannel = typeof(AvatarPlayableController).GetMethods().First(mb => Regex.IsMatch(mb.Name, @"Method_Public_Void_Int32_\d") && CheckString(mb, "Ran out of free puppet channels!"));
        mClearPuppetChannel = typeof(AvatarPlayableController).GetMethods().First(mb => Regex.IsMatch(mb.Name, @"Method_Public_Void_Int32_\d") && CheckString(mb, "Tried to clear an unassigned puppet channel!"));
      } catch (Exception e) {
        Error("XRef scan for AssignPuppetChannel/ClearPuppetChannel failed");
        Error(e);
      }
    }

    public static void AssignPuppetChannel(this AvatarPlayableController instance, int index)
    {
      mAssignPuppetChannel?.Invoke(instance, new object[] { index });
    }

    public static void ClearPuppetChannel(this AvatarPlayableController instance, int index)
    {
      mClearPuppetChannel?.Invoke(instance, new object[] { index });
    }

    private static void OnVRCPlayerAwake(VRCPlayer __instance)
    {
      // OnAvatarIsReady
      __instance.Method_Public_add_Void_OnAvatarIsReady_0(new Action(() => {
        if (__instance.prop_Player_0?.prop_ApiAvatar_0 != null) {
          AvatarIsReady?.Invoke(null, __instance);
        }
      }));
    }

    // [HarmonyArgument(0, "param")]
    // private static void OnAssignPuppetChannel(ActionMenu __instance, VRC.SDK3.Avatars.ScriptableObjects.VRCExpressionsMenu.Control.Parameter param)
    // {
    //   Util.DebugLog($"Puppeteering start {param.name} {param.hash}");
    //   CurrentPuppeteerParameter = param.name;
    //   PuppetChannelAssigned?.Invoke(null, param.name);
    // }

    // [HarmonyArgument(0, "param")]
    // private static void OnClearPuppetChannel(ActionMenu __instance, VRC.SDK3.Avatars.ScriptableObjects.VRCExpressionsMenu.Control.Parameter param)
    // {
    //   Util.DebugLog($"Puppeteering end {param.name} ({param.hash})");
    //   CurrentPuppeteerParameter = null;
    //   PuppetChannelCleared?.Invoke(null, param.name);
    // }

    private static bool CheckString(MethodBase methodBase, string match)
    {
      try { 
        return XrefScanner.XrefScan(methodBase).Where(instance => instance.Type == XrefType.Global && instance.ReadAsObject().ToString() == match).Any();
      } catch { }
      return false;
    }

    private static bool CheckUsed(MethodBase methodBase, string methodName)
    {
      try {
        return XrefScanner.UsedBy(methodBase).Where(instance => instance.TryResolve() != null && instance.TryResolve().Name.Contains(methodName)).Any();
      } catch { }
      return false;
    }
  }
}