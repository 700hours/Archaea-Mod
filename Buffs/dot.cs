using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Buffs
{
    public class dot : ModBuff
    {
        int ticks;
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.active || npc.life > 0)
            {
                ticks++;
                int randDmg = Main.rand.Next(8, 14);
                bool crit = Main.rand.Next(8) == 0;
                if(ticks % 150 == 0) npc.StrikeNPC(randDmg, 0f, -1, crit ? true : false, false, false);
            }
            else buffIndex--;
        }
    }
}
