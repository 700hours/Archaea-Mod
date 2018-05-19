using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    class _bullet : ModProjectile
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
        bool init = false;
        int proj1, proj2;
        float degrees = 0.017f;
        float radius;
        float WaveTimer;
        float MaxTime, Speed;
        float CurrentPoint;
        const float radians = 0.017f;
        Vector2 center;
        Vector2 Start, End;
        Vector2 position;
        Player player;
        public void Initialize()
        {
            Player player = Main.player[projectile.owner];
            if (Main.rand.NextFloat() >= .80f)
            {
                if (player.direction == 1)
                {
                    position = new Vector2(projectile.position.X + player.width * 2, projectile.position.Y);
                }
                else
                {
                    position = projectile.position;
                }
                // play sound
                proj1 = Projectile.NewProjectile(position, projectile.velocity, 134, 20 + Main.rand.Next(0, 10), 2f, projectile.owner, 0f, 0f);
                proj2 = Projectile.NewProjectile(position, projectile.velocity, 134, 20 + Main.rand.Next(0, 10), 2f, projectile.owner, 0f, 0f);
            }
        }
        public override bool PreAI()
        {
            if(!init)
            {
                Initialize();
                init = true;
            }
            Projectile Proj1 = Main.projectile[proj1];
            Projectile Proj2 = Main.projectile[proj2];
            
            #region wave
            // the amplitude of the wave
            float Offset1 = 7.5f;
            float Offset2 = -7.5f;

            // 360 degrees in radians
            float Revolution = 6.28308f;

            // how many waves are completed per second
            float WavesPerSecond = 2.0f;

            // get time between updates
            float Time = 1.0f / Main.frameRate;

            // increase wave timer
            WaveTimer += Time * Revolution * WavesPerSecond;

            float Angle = projectile.ai[0];

            float Cos = (float)Math.Cos(Angle);
            float Sin = (float)Math.Sin(Angle);

            float WaveOffset1 = (float)Math.Sin(WaveTimer) * Offset1;
            float WaveOffset2 = (float)Math.Sin(WaveTimer) * Offset2;
            #endregion
            Proj1.position.X -= Sin * WaveOffset1;
            Proj1.position.Y += Cos * WaveOffset1;
            
            Proj2.position.X -= Sin * WaveOffset2;
            Proj2.position.Y += Cos * WaveOffset2;
            
            return true;
        }
    }
}
