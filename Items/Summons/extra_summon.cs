using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items.Summons
{
    public class extra_summon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Test Summoner");
            Tooltip.SetDefault("Summons ??? boss");
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
            item.consumable = false;
            item.noMelee = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<magno_bar>(), 5);
            recipe.AddIngredient(mod.ItemType<magno_core>(), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override bool UseItem(Player player)
        {
            int npcWidth = 100;
            int npcHeight = 93;
            float npcX = player.position.X - 8 + (player.width - npcWidth) * 0.5f;
            float npcY = player.position.Y - npcHeight + 40;
            if (!NPC.AnyNPCs(mod.NPCType("_boss")))
            {
                int spawn = NPC.NewNPC((int)npcX, (int)npcY, mod.NPCType("_boss"));
                if (Main.netMode != 0)
                    NetMessage.SendData(23, -1, -1, null, spawn, 0f, 0f, 0f, 0, 0, 0);
            }
            //	if (spawn < 0 || spawn >= 300)
            //		return;

            //  Main.npc[spawn].target = Main.myPlayer;
            //	Main.npc[spawn].RunMethod("CustomInit", null);

            return true;
        }
    }
}
