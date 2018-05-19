using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod;

namespace ArchaeaMod.Projectiles
{
    public class global_projectile : GlobalProjectile
    {
        public override void Kill(Projectile projectile, int timeLeft)
        {
            Player player = Main.LocalPlayer;
            ArchaeaPlayer modPlayer = player.GetModPlayer<ArchaeaPlayer>(mod);

            if (modPlayer.magnoRanged && Main.rand.NextFloat() > 0.85f && projectile.friendly && projectile.arrow)
            {
                for (float k = 0; k < MathHelper.ToRadians(360); k += 0.017f * 9)
                {
                    int Proj1 = Projectile.NewProjectile(projectile.position + new Vector2(projectile.width / 2, projectile.height / 2), Distance(null, k, 16f), mod.ProjectileType("dust_diffusion"), projectile.damage / 2, projectile.knockBack, projectile.owner, Distance(null, k, 16f).X, Distance(null, k, 16f).Y);
                    if (Main.netMode == 1) NetMessage.SendData(27, -1, -1, null, Proj1);
                }
                //custom sound
                //Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/IceBeamChargeShot"), projectile.position);
                //vanilla sound
                Main.PlaySound(2, projectile.position, 14);
            }
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}
