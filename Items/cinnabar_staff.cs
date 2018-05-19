using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items
{
    public class cinnabar_staff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cinnabar Staff");
            Tooltip.SetDefault("Conjures lethal orbs");
        }
        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 44;
            item.scale = 1f;
            item.useTime = 24;
            item.useAnimation = 24;
            item.damage = 20;
            item.knockBack = 10f;
            item.mana = 12;
            item.useStyle = 5;
            item.value = 15000;
            item.rare = 3;
            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            //  or vanilla sound?
            //  item.UseSound = SoundID.Item1;
            item.noMelee = true;
            item.autoReuse = false;
            item.magic = true;
        }
        public override void AddRecipes()
        {
            //  mod recipe
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<magno_bar>(), 10);
            recipe.AddIngredient(mod.ItemType<magno_core>(), 1);
            recipe.AddIngredient(mod.ItemType<cinnabar_crystal>(), 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        int height;
        int Proj;
        float radius = 96f;
        float degrees = 45f;
        float ProjX, ProjY;
        const float radians = 0.017f;
        Vector2 playerCenter;
        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("cinnabar_orb")] <= 3)
            {
                return true;
            }
            else return false;
        }
        public override bool UseItem(Player player)
        {
            height = player.height / 2;
            playerCenter = new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2);
            
            float start = radians * 45f;
            float end = radians * 360f;
            float add = radians * degrees;
            for (float k = start; k < end; k += add)
            {
                int Proj = Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("cinnabar_orb"), (int)(item.damage / player.magicDamage), item.knockBack, player.whoAmI, k, 0f);
            }

            Main.PlaySound(2, player.position, 20);
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, height * (-1) - 16);
        }
        public Texture2D texture
        {
            get { return mod.GetTexture("Gores/cinnabar_staffglow"); }
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            spriteBatch.Draw(texture,
                    item.Center - Main.screenPosition, null,
                    Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
        }
    }
}
