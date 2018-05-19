using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.NPCs;

namespace ArchaeaMod.Items.Summons
{
    public class custom_summon : ModItem
    {
        public override void SetStaticDefaults()
        {
        //  DisplayName.SetDefault("Test Summoner");
            Tooltip.SetDefault("Summons Magno Mimic");
        }
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.useTime = 45;
            item.useAnimation = 45;
            item.useStyle = 4;
            item.value = 100;
            item.rare = 2;
            item.autoReuse = false;
            item.consumable = true;
            item.noMelee = true;
            //  for spawning without net code
            item.makeNPC = /*448;*/ /*NPCID.GoldWorm;*/ (short)mod.NPCType<m_mimic>();
        }

        public override bool UseItem(Player player)
        {
        /*  int npcWidth = 100;
            int npcHeight = 93;
            float npcX = player.position.X - 8 + (player.width - npcWidth) * 0.5f;
            float npcY = player.position.Y - npcHeight + 40;
            Vector2 mousev = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
            int spawn = NPC.NewNPC((int)mousev.X, (int)mousev.Y, mod.NPCType("m_mimic"));
            if (Main.netMode == 1)
            {
                ModPacket packet = mod.GetPacket();
                packet.Write(spawn);
                packet.Write(Main.npc[spawn].whoAmI);
                packet.Send();
            }   */

            //	if (spawn < 0 || spawn >= 300)
            //		return;

            //  Main.npc[spawn].target = Main.myPlayer;
            //	Main.npc[spawn].RunMethod("CustomInit", null);

            return false;
        }
    }
}
