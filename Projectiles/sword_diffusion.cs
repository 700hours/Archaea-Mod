using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod;

namespace ArchaeaMod.Projectiles
{
    public class sword_diffusion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Diffusion");
        }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.timeLeft = 30;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 0.1f;
            projectile.alpha = 255;
            projectile.magic = true;
        }

        public int ticks
        {
            get { return (int)projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
        public override void AI()
        {
            ticks++;

        /*  if(ticks > 5)
                projectile.velocity *= 0.98f;
            
            int dustType = mod.DustType("cinnabar_dust");
            int dustType2 = mod.DustType("c_silver_dust");

            if (ticks % 10 == 0)
            {
                int Dust1 = Dust.NewDust(projectile.position, 1, 1, dustType2, AngularVel(8f, projectile.ai[1]).X, AngularVel(8f, projectile.ai[1]).Y, 0, Color.White, 2f);
                for (int i = 0; i < 2; i++)
                {
                    int Dust2 = Dust.NewDust(projectile.position, 1, 1, dustType, AngularVel(8f, projectile.ai[1]).X, AngularVel(8f, projectile.ai[1]).Y, 0, Color.White, 2f);
                    Main.dust[Dust2].noLight = true;
                }
            }   */
        }

        public Vector2 AngularVel(float radius, float angle)
        {
            float velX = (float)(radius * Math.Cos(angle));
            float velY = (float)(radius * Math.Sin(angle));

            return new Vector2(velX, velY);
        }
    }
}
