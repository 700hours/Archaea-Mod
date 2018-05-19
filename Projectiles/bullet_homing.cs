using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    public class bullet_homing : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 4;
            projectile.scale = 1f;
            projectile.aiStyle = -1;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.ranged = true;
        }

        public void Initialize()
        {
            projectile.rotation = (float)Math.Atan2(Main.player[projectile.owner].position.Y - Main.MouseWorld.Y, Main.player[projectile.owner].chestX - Main.MouseWorld.X);
            oldVel = projectile.velocity;
        }
        bool init = false;
        int ticks = 0;
        int pDust = 0;
        int targetID;
        float newAngle;
        const float radians = 0.017f;
        Vector2 oldPos;
        Vector2 oldVel;
        public override void AI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }
            for (int i = 0; i < 6; i++)
            {
                int pDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 0, Color.Red, 1f);
                Main.dust[pDust].noGravity = true;
            }
            Player player = Main.player[projectile.owner];

            ticks++;
            
            foreach (NPC n in Main.npc)
            {
                if (n.active && !n.friendly && !n.dontTakeDamage && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, n.position, n.width, n.height))
                {
                    targetID = n.whoAmI;
                    break;
                }
            }
            NPC target = Main.npc[targetID];
            if (targetID != default(int))
            {
                newAngle = (float)Math.Atan2(projectile.position.Y - target.position.Y, projectile.position.X - target.position.X);
                projectile.velocity += Distance(null, newAngle + MathHelper.ToRadians(180f), 4f);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = Color.White;
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}
