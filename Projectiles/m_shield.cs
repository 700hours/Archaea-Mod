using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    public class m_shield : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Projectile Shield");
        }
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 40;
            projectile.timeLeft = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1f;
            projectile.magic = true;
            projectile.netImportant = true;
        }

        public void Initialize()
        {
            projectile.netUpdate = true;
            projectile.position = center + new Vector2((float)(radius * Math.Cos(degrees)), (float)(radius * Math.Sin(degrees)));
        }
        public float degrees
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
        bool init = false;
        int damage = 0;
        int maxDamage = 2;
        int ticks;
        float radius = 96f;
        float pointX, pointY;
        const float radians = 0.017f;
        Vector2 center;
        Vector2 intensity;
        Projectile proj;
        public override void AI()
        {
            ArchaeaPlayer modPlayer = player.GetModPlayer<ArchaeaPlayer>(mod);
            center = player.Center;

            if (!init)
            {
                Initialize();
                init = true;
            }
            if (ticks < 900)
            {
                ticks++;
            }
            else ticks = 0;
            if (ticks % 180 == 0)
                projectile.netUpdate = true;

            if (modPlayer.magnoShield && !player.dead)
            {
                projectile.timeLeft = 2;
            }

            degrees += radians * 4.5f;
            projectile.position.X = (center.X - projectile.width / 2) + (float)(radius * Math.Cos(degrees));
            projectile.position.Y = (center.Y - projectile.height / 2) + (float)(radius * Math.Sin(degrees));

            float Angle = (float)Math.Atan2(player.position.Y - projectile.position.Y, player.position.X - projectile.position.X);
            projectile.rotation = Angle + MathHelper.ToRadians(180);

            foreach (Projectile p in Main.projectile)
            {
                if (p.active && !p.friendly && p.owner != player.whoAmI && p.type != mod.ProjectileType<m_shield>())
                {
                    if (p.Hitbox.Intersects(projectile.Hitbox))
                    {
                        p.Kill();
                        damage++;
                        // play sound, metal tink
                        Main.PlaySound(21, projectile.position, 0);
                    }
                }
            }
            if (damage == maxDamage)
            {
                projectile.Kill();
                damage = 0;

                // play sound, metal clang
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int k = 0; k < 4; k++)
            {
                int Dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 4, 0f, 0f, 0, default(Color), 1.2f);
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
        public Texture2D texture
        {
            get { return mod.GetTexture("Gores/m_shieldglow"); }
        }
        public Player player
        {
            get { return Main.player[projectile.owner]; }
        }
        float degrees2;
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(texture,
                projectile.Center - Main.screenPosition, null,
                Color.White, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), projectile.scale, SpriteEffects.None, 0f);
        }
    }
}