using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod
{
    public class TestMod : Mod
    {
        public const string magnoHead = "ArchaeaMod/Gores/magno_icon";
        public void SetModInfo(out string name, ref ModProperties properties)
        {
            name = "The ArchaeaMod Mod";
            properties.Autoload = true;
            properties.AutoloadGores = true;
            properties.AutoloadSounds = true;
        }
        public override void Load()
        {
            AddBossHeadTexture(magnoHead);
        }

        int oldID, ID;
        int[] plrID = new int[255];
        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            foreach (Player p in Main.player)
            {
                if (p.active && !p.dead && p != null)
                {
                    oldID = plrID[ID];
                    plrID[ID] = p.whoAmI;
                    if (oldID == p.whoAmI)
                        break;
                    if (ID < plrID.Length)
                        ID++;
                    else ID = 0;
                }
            }
            foreach (int i in plrID)
            {
                if (Main.player[plrID[i]].active && !Main.player[plrID[i]].dead && Main.player[plrID[i]] != null)
                {
                    if (Main.myPlayer == Main.player[plrID[i]].whoAmI && Main.player[plrID[i]].GetModPlayer<ArchaeaPlayer>().MagnoZone)
                    {
                        music = GetSoundSlot(SoundType.Music, "Sounds/Music/Magno_Biome_1");
                        priority = MusicPriority.BiomeHigh;
                        break;
                    }
                }
            }
        }
    }
}