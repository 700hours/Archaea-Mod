using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    class magno_bullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno bullet");
        }
        public override void SetDefaults()
        {
            projectile.width = 8;
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
        float Angle;
        const float radians = 0.017f;
        Vector2 center;
        Vector2 Start, End;
        Vector2 position;
        Player player;
        public void Initialize()
        {
            //  Projectile proj = Main.projectile[projectile.whoAmI];
            //  Main.projectileTexture[projectile.type] = mod.GetTexture("NPCs/m_flame");

            Player player = Main.player[projectile.owner];
        /*  if (Main.rand.NextFloat() >= .80f)
            {
                if (player.direction == 1)
                {
                    position = new Vector2(projectile.position.X, projectile.position.Y);
                }
                else
                {
                    position = new Vector2(projectile.position.X - player.width, projectile.position.Y);
                }
                // play sound
                // type 134 = missile, type 207 = chlorophyte bullet
                int projType = mod.ProjectileType<bullet_homing>();
                proj1 = Projectile.NewProjectile(position, projectile.velocity, projType, 20 + Main.rand.Next(0, 10), 2f, projectile.owner, projectile.rotation, 0f);
                proj2 = Projectile.NewProjectile(position, projectile.velocity, projType, 20 + Main.rand.Next(0, 10), 2f, projectile.owner, projectile.rotation, 0f);
                if (Main.netMode == 1)
                {
                    NetMessage.SendData(27, -1, -1, null, proj1);
                    NetMessage.SendData(27, -1, -1, null, proj2);
                }
            }   */
            Angle = (float)Math.Atan2(player.Center.Y - Main.MouseWorld.Y, player.Center.X - Main.MouseWorld.X);
            projectile.rotation = Angle;
            projectile.spriteDirection = -1;
            projectile.position.Y -= 4;
            if(projectile.rotation >= radians * 315f && projectile.rotation <= radians * 360f)
            {
                projectile.position.X += 8;
            }
            projectile.velocity *= 1.6f;

            projectile.netUpdate = true;
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
            if (Proj1.type != projectile.type)
            {
                Proj1.position.X -= Sin * WaveOffset1;
                Proj1.position.Y += Cos * WaveOffset1;
            }
            if (Proj2.type != projectile.type)
            { 
                Proj2.position.X -= Sin * WaveOffset2;
                Proj2.position.Y += Cos * WaveOffset2;
            }

            if (!Main.dayTime)
            {
                Lighting.AddLight(projectile.position, new Vector3(0.804f, 0.361f, 0.361f));
            }

            return true;
        }
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = Color.White;
        }

        public void SyncProj(int netID)
        {
            if (Main.netMode == netID)
            {
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile.whoAmI, projectile.position.X, projectile.position.Y, projectile.rotation);
                projectile.netUpdate = true;
            }
        }

    /*  public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Angle);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.rotation = reader.ReadSingle();
        }   */
    }
}
