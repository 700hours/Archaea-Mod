using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    public class magno_flame : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.scale = 1f;
            projectile.aiStyle = -1;
            projectile.timeLeft = 300;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.ranged = true;
        }

        public void Initialize()
        {
            projectile.netUpdate = true;
        }
        public int projTimer
        {
            get { return (int)projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
        public int projBounce
        {
            get { return (int)projectile.ai[1]; }
            set { projectile.ai[1] = value; }
        }
        bool init;
        int ticks;
        int dustType;
        public override void AI()
        {
            if(!init)
            {
                Initialize();
                init = true;
            }

            ticks++;

            dustType = mod.DustType("magno_dust");

            projectile.rotation = MathHelper.ToRadians(ticks * 9);

            if (ticks % 3 == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    int Dust1 = Dust.NewDust(projectile.position, 16, 16, dustType, 0f, 0f, 0, Color.White, 1.2f);
                }
            }

            projTimer--;
            if (projTimer < 0 && projBounce == 1)
            {
                if (TileCollideLeft((int)projectile.position.X, (int)projectile.position.Y) || TileCollideRight((int)projectile.position.X, (int)projectile.position.Y))
                {
                    projectile.velocity.X *= -1;
                }
                if (TileCollideUp((int)projectile.position.X, (int)projectile.position.Y) || TileCollideDown((int)projectile.position.X, (int)projectile.position.Y))
                {
                    projectile.velocity.Y *= -1;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            for(int i = 0; i < 3; i++)
            {
                int Dust1 = Dust.NewDust(projectile.position, 16, 16, dustType, projectile.velocity.X, projectile.velocity.Y, 0, Color.White, 1.2f);
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public void SyncProj(int netID)
        {
            if (Main.netMode == netID)
            {
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI, projectile.position.X, projectile.position.Y);
                projectile.netUpdate = true;
            }
        }

        public bool TileCollideLeft(int x, int y)
        {
            int i = x / 16;
            int j = y / 16;

            Tile tile = Main.tile[i - 1, j];
            bool active = tile.active();
            bool solid = Main.tileSolid[tile.type] && tile.type != TileID.Platforms;

            return active && solid;
        }
        public bool TileCollideRight(int x, int y)
        {
            int i = x / 16;
            int j = y / 16;

            Tile tile = Main.tile[i + 1, j];
            bool active = tile.active();
            bool solid = Main.tileSolid[tile.type] && tile.type != TileID.Platforms;

            return active && solid;
        }
        public bool TileCollideUp(int x, int y)
        {
            int i = x / 16;
            int j = y / 16;

            Tile tile = Main.tile[i, j - 1];
            bool active = tile.active();
            bool solid = Main.tileSolid[tile.type] && tile.type != TileID.Platforms;

            return active && solid;
        }
        public bool TileCollideDown(int x, int y)
        {
            int i = x / 16;
            int j = y / 16;

            Tile tile = Main.tile[i, j + 1];
            bool active = tile.active();
            bool solid = Main.tileSolid[tile.type] && tile.type != TileID.Platforms;

            return active && solid;
        }
    }
}
