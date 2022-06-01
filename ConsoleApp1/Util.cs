using System;
using UnityEngine;
using VRC;

namespace NSFWSecurity
{
  static class Util
  {
    public static PlayerManager PlayerManager => PlayerManager.prop_PlayerManager_0;

    public static VRCPlayer LocalPlayer => VRCPlayer.field_Internal_Static_VRCPlayer_0;

    public static GameObject GetAvatar(this Player player) => player.prop_VRCPlayer_0.prop_VRCAvatarManager_0.prop_GameObject_0;

    #if DEBUG
      public static bool Debug => true;
    #else
      public static bool Debug => false;
    #endif

    public static bool DebugLogging = Debug; 
    public static void DebugLog(string message, bool force = false) 
    {
      if (DebugLogging || force) {
        MelonLoader.MelonLogger.Msg(System.ConsoleColor.Cyan, "[DEBUG] " + message);
      }
    }

    public static bool AlmostEqual(double a, double b) {
      const double delta = 0.0001;
      return Math.Abs(a - b) < delta;
    }

    static bool IsChildOf(GameObject parent, GameObject child)
    {
      if (parent == child) {
        return true;
      }

      for (int i = 0; i < parent.transform.childCount; i++) {
        if (IsChildOf(parent.transform.GetChild(i).gameObject, child)) {
          return true;
        }
      }

      return false;
    }

    // Since there is no GetComponentInParent that gets inactive components...
    // GetComponent functions not including active components and not making it a conscious
    // choice for the programmer is one of the biggest mistakes and source of bugs Unity ever caused.
    public static T GetComponentInParent<T>(Component c, bool inactive) 
    {
      var components = c.GetComponentsInParent<T>(inactive);
      return components.Length > 0 ? components[0] : default(T);
    }
  }
}
