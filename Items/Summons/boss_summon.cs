using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.NPCs;

namespace ArchaeaMod.Items.Summons
{
    public class boss_summon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac Fossil");
            Tooltip.SetDefault("Seems of red and"
                +   "\ngray glowing origins");
        }
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.useTime = 45;
            item.useAnimation = 45;
            item.useStyle = 4;
            item.maxStack = 20;
            item.value = 100;
            item.rare = 2;
            item.autoReuse = false;
            item.consumable = true;
            item.noMelee = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bone, 15);
            recipe.AddIngredient(mod.ItemType<magno_core>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        bool flag = false;
        bool isActive;
        int spawn;
        public override bool CanUseItem(Player player)
        {
            ArchaeaPlayer modPlayer = player.GetModPlayer<ArchaeaPlayer>(mod);

            foreach (NPC n in Main.npc)
            {
                if (n.active && n.type == mod.NPCType("boss_magnohead"))
                {
                    isActive = true;
                    break;
                }
                else isActive = false;
            }

            return modPlayer.MagnoZone && !isActive;
        }
        public override bool UseItem(Player player)
        {   
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType<boss_magnohead>());
            return true;
        }
    }
}
