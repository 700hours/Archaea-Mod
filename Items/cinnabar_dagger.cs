using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items
{
    public class cinnabar_dagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Dagger");
            //  Tooltip.SetDefault("Fires Magno arrows");
        }
        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.scale = 1f;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 1;
            item.damage = 20;
            item.knockBack = 4f;
            item.shootSpeed = 11f;
            item.value = 1500;
            item.rare = 1;
            item.maxStack = 999;
            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            //  or vanilla sound?
            //  item.UseSound = SoundID.Item11;
            item.noUseGraphic = true;
            item.autoReuse = false;
            item.consumable = true;
            item.noMelee = true;
            item.thrown = true;
            //  vanilla shooting method
            item.shoot = mod.ProjectileType("cinnabar_dagger");
            //  for use with alternative cinnabar sword usage
            //  item.ammo = item.type;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<magno_bar>(), 2);
            recipe.AddIngredient(mod.ItemType<cinnabar_crystal>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
        }
    }
}
