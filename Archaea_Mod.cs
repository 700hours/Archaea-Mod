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

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.netMode < 2 && Main.LocalPlayer.active && !Main.gameMenu && Main.LocalPlayer.GetModPlayer<ArchaeaPlayer>().MagnoZone)
            {
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Magno_Biome_1");
                priority = MusicPriority.BiomeHigh;
            }
        }
    }
}