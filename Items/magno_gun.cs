using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    public class magno_gun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Gun");
            Tooltip.SetDefault("20% chance to"
                +   "\nfire a homing bullet"
                +   "\n20% chance not"
                +   "\nto consume ammo");
        }
        public override void SetDefaults()
        {
            item.width = 46;
            item.height = 20;
            item.scale = 1f;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = 5;
            item.damage = 19;
            item.knockBack = 2.5f;
            item.shootSpeed = 10f;
            item.value = 2500;
            item.rare = 1;
            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.consumable = false;
            item.noMelee = true;
            item.ranged = true;
            //  vanilla shooting method
            item.shoot = mod.ProjectileType<magno_bullet>();
            item.useAmmo = AmmoID.Bullet;
        }
        public override void AddRecipes()
        {
            //  mod recipe, balanced?
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType<magno_bar>(), 10);
            recipe.AddIngredient(mod.ItemType<magno_core>(), 4);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        Vector2 playerCenter;
        public override void UseStyle(Player player)
        {
            playerCenter = new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2);
            float HalfPi = (float)Math.PI / 2f;
            if (Main.MouseWorld.X - player.position.X > 0) player.direction = 1;
            else player.direction = -1;
            player.itemRotation = (float)Math.Atan2((Main.MouseWorld.Y - playerCenter.Y) * player.direction, (Main.MouseWorld.X - playerCenter.X) * player.direction);
            player.itemLocation.X -= player.direction * (player.width / 8) * (1f - (float)Math.Abs(player.itemRotation) / HalfPi);
        }

        int Proj;
        int ticks = 0;
        float nX, nY;
        float Offset;
        Vector2 position;
        Vector2 speed;
        //  manual firing method
        public override bool UseItem(Player player)
        {
            /*
            ticks++;
            if (player.direction == 1)
                position = new Vector2(player.itemLocation.X + (player.direction * player.width), player.itemLocation.Y);
            else position = new Vector2(player.itemLocation.X - (player.direction * player.width), player.itemLocation.Y);

            speed = new Vector2(player.direction * Distance(null, player.itemRotation, item.shootSpeed).X, player.direction * Distance(null, player.itemRotation, item.shootSpeed).Y);

            float radius = player.width * 3f;
            if (player.direction == 1)
            {
                nX = position.X + (float)(radius * Math.Cos(player.itemRotation));
                nY = position.Y + (float)(radius * Math.Sin(player.itemRotation));
            }
            else
            {
                nX = position.X + (float)((radius * -1 / 4) * Math.Cos(player.itemRotation));
                nY = position.Y + (float)((radius * -1 / 4) * Math.Sin(player.itemRotation));
            }
            if (player.itemRotation >= MathHelper.ToRadians(315) && player.itemRotation <= MathHelper.ToRadians(360) ||
                player.itemRotation >= MathHelper.ToRadians(135) && player.itemRotation <= MathHelper.ToRadians(180) ||
                player.itemRotation >= MathHelper.ToRadians(0) && player.itemRotation <= MathHelper.ToRadians(45))
            {
                Offset = 20f;
            }
            else Offset = 8f;
            Proj = Projectile.NewProjectile(new Vector2(nX + Offset, nY + Offset / 2), speed, mod.ProjectileType<magno_bullet>(), item.damage, item.knockBack, player.whoAmI, player.itemRotation, item.damage);
            */
            //  unecessary?
            //  if (Main.netMode == 2) NetMessage.SendData(27, -1, -1, null, Proj);
            return true;
        }
        
        //  unecessary if manually shooting
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if(Main.rand.NextFloat() < 0.80f)
                type = mod.ProjectileType<magno_bullet>();
            else type = 207;
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .20f;
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}