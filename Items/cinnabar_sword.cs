using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items
{
    public class cinnabar_sword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Sword");
        //  Tooltip.SetDefault("25% chance to throw");
        }
        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 44;
            item.scale = 1f;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 1;
            item.damage = 20;
            item.knockBack = 5.5f;
            item.value = 1500;
            item.rare = 2;
            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            //  or vanilla sound?
            item.UseSound = SoundID.Item1;
            //  yes or no?
            item.autoReuse = false;
            item.melee = true;
            //  vanilla shooting method
            //  alternative sword usage
            //  item.shoot = mod.ProjectileType("cinnabar_dagger");
            //  item.useAmmo = mod.ItemType<cinnabar_dagger>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<magno_bar>(), 8);
            recipe.AddIngredient(mod.ItemType<cinnabar_crystal>(), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        /*
        const float radians = 0.017f;
        public override bool UseItem(Player player)
        {
            float Angle = (float)Math.Atan2(player.position.Y - Main.MouseWorld.Y, player.position.X - Main.MouseWorld.X);
            float offset = MathHelper.ToRadians(30f);
            float correction = MathHelper.ToRadians(180f);
            Vector2 center = new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2);

            for (float i = Angle - offset; i < Angle + offset; i += radians * 10)
            {
                int hurtDust = Projectile.NewProjectile(center, AngularVel(8f, i + correction), mod.ProjectileType("sword_diffusion"), item.damage, item.knockBack, 0, player.direction, i + correction);
            }

            if (Main.MouseWorld.X < player.position.X)
                player.direction = -1;
            else player.direction = 1;

            return true;
        }
        */
        public Vector2 AngularVel(float radius, float angle)
        {
            float velX = (float)(radius * Math.Cos(angle));
            float velY = (float)(radius * Math.Sin(angle));

            return new Vector2(velX, velY);
        }
    }
}
