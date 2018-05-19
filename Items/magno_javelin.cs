using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    class magno_javelin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Javelin");
        }
        public override void SetDefaults()
        {
            item.width = 54;
            item.height = 56;
            item.useTime = 24;
            item.useAnimation = 18;
            item.useStyle = 5;
            item.damage = 20;
            item.knockBack = 6f;
            item.shootSpeed = 10f;
            item.value = 250;
            item.rare = 1;
            item.maxStack = 999;
            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            //  or vanilla sound
            item.UseSound = SoundID.Item1;

            item.autoReuse = false;
            item.consumable = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.thrown = true;
            //  vanilla shooting method
            item.shoot = mod.ProjectileType<magno_javelinprojectile>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<magno_bar>(), 4);
            recipe.AddIngredient(mod.ItemType<magno_core>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 15);
            recipe.AddRecipe();
        }
        
/*      int Proj = 0;
        Vector2 speed;
        public override bool UseItem(Player player)
        {
            speed = new Vector2(player.direction * Distance(null, player.itemRotation, 16f).X, player.direction * Distance(null, player.itemRotation, 16f).Y);
            Proj = Projectile.NewProjectile(player.Center, speed, mod.ProjectileType<magno_javelinprojectile>(), 18, 0f, player.whoAmI, 0f, 0f);
            return true;
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }   */

        public Texture2D texture
        {
            get { return mod.GetTexture("Gores/magno_javelinglowitem"); }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            spriteBatch.Draw(texture,
                    item.Center - Main.screenPosition, null,
                    Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
        }
    }
}
