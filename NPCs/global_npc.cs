using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace ArchaeaMod.NPCs
{
    public class global_npc : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            bool Zone = spawnInfo.player.GetModPlayer<ArchaeaPlayer>(mod).MagnoZone;
            
        //  NetMessage.BroadcastChatMessage(NetworkText.FromKey("Zone check: " + Zone), Color.White);
            
            pool.Add(mod.NPCType<m_slime>(), Zone ? 0.25f : 0f);
            pool.Add(mod.NPCType<m_fanatic>(), Zone ? 0.12f : 0f);
            pool.Add(mod.NPCType<m_diggerhead>(), Zone ? 0.075f : 0f);
            pool.Add(mod.NPCType<m_mimic>(), Zone && Main.hardMode ? 0.1f : 0f);
        }
    }
}
