# NSFWSecurity
This mod is just a part of a system I created to help VRChat users hide their NSFW parts from everyone but the users who use the mod or followed the tutorial.

Big thanks to Jace, creator of vibes goes brrrr, for the util and the vrchook script :https://gitlab.com/jacefax/vibegoesbrrr

<b>Note that this mod/avatar setup works everywhere, but it's intended to be used only in private instances, so if you use it in a public/friend+ instance, do so at your own risk. I am in no way responsible if you get caught/banned for using it in a public/friend+ instance.</b>

Explanation:

What if VRChat offered a way to make certain objects or animations private on your avatar? A way to make those objects visible only to your friends who have authorized it. With this setup, this is dead easy.

If you follow the tutorial to set up your avatar, only the people you allowed to use the avatar pose/interaction and who installed the mod (or followed the tutorial) will be able to see NSFW parts on your avatar. You'll also have a button to bypass this security and make everyone able to see your NSFW parts.

<b>To be able to see the NSFW of your friend that have the security, you need to enable the mod and you both need to activate interaction (the yellow hand icon under the nametag) beetween each other. note that the NSFW animations updates only within 6 meter around the player because of avatar dynamics limitations.</b>

In this GitHub there's a full tutorial to setup your avatar with this security, there's also a Melonloader mod here that provides a button in game to enable/disable seeing other's NSFW bits. (This button only work with avatar that have the setup) : https://github.com/Olskor/NSFWSecurity/releases/tag/v0.0.3-alpha.

![Animation](https://user-images.githubusercontent.com/105324070/171660715-2f53686b-1111-4aeb-beca-34b5949bc04c.gif)

This works in two parts :

One part is only the "client" part, it's there to allow you to see the NSFW elements a person authorizes you to see. If you don't have this part on your avatar, you won't be able to see the animations of the other person even if they authorize you. <b>This is the part my mod automates, so if you install my mod you don't need to modify your avatar to see NSFW bits of those who authorize you.</b>

The other part is the "provider" part, it is there to hide your chosen animations and wait for an authorized client to see the part. <b>This part is not supported by my mod because it's too complex, to protect your NSFW parts from being seen unintentionally you have to modify your avatar using the tutorial provided below!</b>

Here is the tutorial that explains everything about this security and how to set it up on your avatar.

NOTE: You don't need to follow the first step if you install the mod, the mod replicates the first step automatically, so all of your friends can see your nsfw parts without configuring their avatar. 

![explanation contact](https://user-images.githubusercontent.com/105324070/171626554-f3cc1c64-8fc4-4e82-b8a4-5e7a51e91207.png)
