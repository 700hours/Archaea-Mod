using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod;

namespace ArchaeaMod.Projectiles
{
    public class shock_spark : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shock Spark");
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.scale = 1f;
            projectile.aiStyle = -1;
            projectile.timeLeft = 30;
            projectile.friendly = true;
            projectile.penetrate = 500;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.magic = true;
        }

        bool init = false;
        Texture2D texture;
        public void Initialize()
        {
            if (Main.netMode != 2)
                texture = mod.GetTexture("Gores/electricity");
        }
        public override void AI()
        {
            if(!init)
            {
                Initialize();
                init = true;
                for (int i = 0; i < 8; i++)
                {
                    int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 64, 0f, 0f, 0, Color.White, 2f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity.X = Main.rand.NextFloat(-4f, 4f);
                    Main.dust[d].velocity.Y = Main.rand.NextFloat(-4f, 4f);
                }
                projectile.netUpdate = true;
            }
        //  projectile.position = new Vector2(projectile.ai[0], projectile.ai[1]);
        }
        public override bool PreDraw(SpriteBatch s, Color lightColor)
        {
            Player owner = Main.player[projectile.owner];
            Vector2 OwnerPos = new Vector2(owner.position.X + owner.width / 2, owner.position.Y + owner.height / 2);
            Vector2 ProjPos = new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 2);
            float targetrotation = (float)Math.Atan2((ProjPos.Y - OwnerPos.Y), (ProjPos.X - OwnerPos.X));
            ArchaeaWorld.DrawChain(OwnerPos, ProjPos, texture, s);

            return true;
        }
    }
}
