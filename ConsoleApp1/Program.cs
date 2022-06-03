using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Dynamics.Contact;
using VRChatUtilityKit.Utilities;

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
            ourIsEnabled.OnValueChanged += (_, v) =>
            {
                try
                {
                    VRCUtils.ReloadAllAvatars();
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error("Error while reloading avatar:\n" + ex.ToString());
                } // Ignore
            };
        }

        private void VRCHooks_AvatarIsReady(object sender, VRCPlayer player)
        {
            if (player == Util.LocalPlayer)
            {
                var avatar = player.GetComponentInChildren<VRC.SDK3.Avatars.Components.VRCAvatarDescriptor>();
                if (!avatar) return;

                VRC.Dynamics.ContactSender contactSender;
                VRC.Dynamics.ContactSender[] contacts;
                contacts = avatar.gameObject.GetComponentsInChildren<VRC.Dynamics.ContactSender>();

                VRC.Dynamics.ContactSender contact = contacts.FirstOrDefault(c => c.collisionTags.Contains("NSFW"));
                if (contact != null)
                {
                    contactSender = contact;
                    if (!ourIsEnabled.Value)
                    {
                        Component.Destroy(contactSender);
                        return;
                    }
                } else
                {
                    contactSender = avatar.gameObject.AddComponent<VRC.Dynamics.ContactSender>();

                    contactSender.collisionTags.Add("NSFW");
                    contactSender.radius = 5;
                    if (!ourIsEnabled.Value)
                    {
                        Component.Destroy(contactSender);
                        return;
                    }
                }
            }
        }
    }
}
