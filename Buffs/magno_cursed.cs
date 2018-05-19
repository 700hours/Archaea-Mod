using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Buffs
{
    public class magno_cursed : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Granted Inspiration");
            Description.SetDefault("Free from the curse");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
