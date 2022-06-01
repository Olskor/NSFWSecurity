using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Dynamics.Contact;

namespace NSFWSecurity
{
    public partial class Program : MelonMod
    {

        private static MelonPreferences_Entry<bool> ourIsEnabled;

        public override void OnApplicationStart()
        {
            VRCHooks.OnApplicationStart(HarmonyInstance);
            VRCHooks.AvatarIsReady += VRCHooks_AvatarIsReady;

            var category = MelonPreferences.CreateCategory("NSFWSecurity", "NSFWSecurity");
            ourIsEnabled = category.CreateEntry("Enabled", true, "Enable NSFW");
        }

        private void VRCHooks_AvatarIsReady(object sender, VRCPlayer player)
        {
            if (!ourIsEnabled.Value) return;
            if (player == Util.LocalPlayer)
            {
                var avatar = player.GetComponentInChildren<VRCAvatarManager>().gameObject.GetComponentInChildren<VRC.SDK3.Avatars.Components.VRCAvatarDescriptor>();
                if (!avatar) return;
                var contactSender = avatar.gameObject.AddComponent<VRC.Dynamics.ContactSender>();
                contactSender.collisionTags.Add("NSFW");
                contactSender.radius = 5;
            }
        }
    }
}
