using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class _boss : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 128;
            npc.height = 128;
            npc.friendly = false;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.boss = true;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 4180;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.netUpdate = true;
        }
        bool sweep = false, spinAttack = false;
        bool flames= false;
        bool pupsSpawned = false;
        bool magnoClone = false;
        bool init = false;
        int dust;
        int pups, clone, timeLeft = 600;
        int flamesID;
        int ticks = 0;
        int choose = 0;
        int direction;
        float TargetAngle, PlayerAngle;
        float degrees = 0, radius = 64;
        float Depreciate = 80, Point;
        const float Time = 80;
        const float radians = 0.017f;
        Rectangle target;
        Vector2 oldPosition, newPosition;
        Vector2 npcCenter, playerCenter, center;
        Vector2 Start, End;
        public void Initialize()
        {
            npc.TargetClosest(true);
        }
        public override void AI()
        {
            if(!init)
            {
                Initialize();
                init = true;
            }
            npc.ai[0]++;

            Player player = Main.player[npc.target];
            if (npc.target < 0 || npc.target == 255 || player.dead || !player.active)
            {
                npc.TargetClosest(true);
            }

            npcCenter = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2);
            playerCenter = new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2);
            PlayerAngle = (float)Math.Atan2(player.position.Y - npc.position.Y,
                                            player.position.X - npc.position.X);
            #region default pattern
            if (npc.ai[0] < 720)
            {
                if (npc.ai[0] == 1)
                {
                    oldPosition = new Vector2(player.position.X + Random(), player.position.Y + Main.rand.Next(-400, 64));
                    target = new Rectangle((int)oldPosition.X - 16, (int)oldPosition.Y - 16, 32, 32);
                }
                if (npc.Hitbox.Intersects(target))
                {
                    oldPosition = new Vector2(player.position.X + Random(), player.position.Y + Main.rand.Next(-400, 64));
                    target = new Rectangle((int)oldPosition.X - 16, (int)oldPosition.Y - 16, 32, 32);
                }
                if (Vector2.Distance(oldPosition - npc.position, Vector2.Zero) > 280f)
                {
                    TargetAngle = (float)Math.Atan2(oldPosition.Y - npc.position.Y,
                                                    oldPosition.X - npc.position.X);
                    npc.velocity.X = TargetSD(null, TargetAngle, 4f).X;
                    npc.velocity.Y = TargetSD(null, TargetAngle, 4f).Y;

                    npc.rotation = TargetAngle;
                }
                else if (npc.ai[0] == 1 || npc.ai[0]%200 == 0)
                {
                    npc.velocity.X = TargetSD(null, PlayerAngle, 4f).X;
                    npc.velocity.Y = TargetSD(null, PlayerAngle, 4f).Y;

                    npc.rotation = PlayerAngle;
                }
                /*
                if (npc.position.Y > player.position.Y)
                    npc.velocity.Y -= 0.4f; */
            }
            #endregion
            if(npc.ai[0] >= 720)
            {
                if(npc.ai[0] == 740)
                    npc.TargetClosest(true);

                #region sweep
                if (npc.ai[0] >= 780 && npc.ai[0] < 780 + Time)
                {
                    Start.X = player.position.X - 256;
                    Start.Y = player.position.Y - 448;

                    End.X = player.position.X + 256;
                    End.Y = Start.Y;

                    sweep = true;
                }
                if (Depreciate > 0 && sweep)
                {
                    Depreciate--;
                    Point = (Time - Depreciate) / Time;
                    npc.position = Vector2.Lerp(Start, End, Point);

                    npc.rotation = radians * 90;
                    if (Depreciate % 10 == 0)
                    {
                        int attack = Projectile.NewProjectile(npc.position + new Vector2(npc.width / 2, npc.height / 2 + 32), Vector2.Zero, 134, 12 + Main.rand.Next(-2, 8), 2f, player.whoAmI, 0f, 0f);
                        Projectile proj = Main.projectile[attack];
                        proj.velocity.X += TargetSD(null, npc.rotation, 4f).X;
                        proj.velocity.Y += TargetSD(null, npc.rotation, 4f).Y;
                        proj.friendly = false;
                        proj.hostile = true;
                    }
                }
                else if (Depreciate == 0)
                {
                    sweep = false;
                    Depreciate = Time;
                }
                int buffer = 1;
                if (npc.ai[0] >= 780 + Time + buffer && npc.ai[0] < 1000)
                {
                    npc.position = new Vector2(player.position.X - npc.width / 2, player.position.Y - 256f);
                    npc.alpha = 200;
                    npc.immortal = true;

                    player.velocity = Vector2.Zero;
                }
                #endregion
                #region spin
                if (npc.ai[0] == 1000)
                {
                    npc.TargetClosest(true);
                    npc.alpha = 0;
                    npc.immortal = false;

                    Start.X = npc.position.X;
                    Start.Y = npc.position.Y;

                    End.X = Start.X;
                    End.Y = player.position.Y - 384;
                }
                if (npc.ai[0] > 1080)
                {
                    if (Depreciate > 0)
                    {
                        Depreciate--;
                        Point = (Time - Depreciate) / Time;
                        npc.position = Vector2.Lerp(oldPosition, End, Point);

                        TargetAngle = (float)Math.Atan2(End.Y - npc.position.Y,
                                                        End.X - npc.position.X);
                        npc.rotation = TargetAngle;
                    }
                    if (Depreciate == 0) 
                    {
                        Depreciate = Time;
                        spinAttack = true;
                        direction = Main.rand.Next(0, 1);
                        choose = 0;
                    }
                }
                if (spinAttack)
                {
                    npc.position = End;

                    if (direction == 0)
                        degrees += radians * 8;
                    else if (direction == 1)
                        degrees -= radians * 8;

                    npc.rotation = degrees;
                    if (degrees >= radians * 360)
                        degrees = radians;
                    ticks++;
                    if (ticks % 3 == 0 && degrees >= radians * 45f && degrees <= radians * 146.25f)
                    {
                        int attack = Projectile.NewProjectile(npc.position + new Vector2(npc.width / 2, npc.height / 2 + 32), Vector2.Zero, 134, 20, 2f, player.whoAmI, 0f, 0f);
                        Projectile proj = Main.projectile[attack];
                        proj.velocity.X += TargetSD(null, npc.rotation, 4f).X;
                        proj.velocity.Y += TargetSD(null, npc.rotation, 4f).Y;
                        proj.friendly = false;
                        proj.hostile = true;
                    }
                    if (ticks > 180)
                    {
                        spinAttack = false;
                        degrees = radians;
                        npc.rotation = PlayerAngle;
                        Depreciate = Time;
                        ticks = 0;
                        npc.ai[0] = 0;

                        npc.position = new Vector2(player.position.X + Random(), player.position.Y + Random());
                    }
                }
                #endregion
            }
            #region spirit flames
            if (!flames && Main.rand.Next(0, 4800) == 0)
            {
                for (int k = 0; k < 4; k++)
                {
                    degrees = 90f;
                    radius = 128f;
                    center = player.position;
                    float nX = center.X + (float)(radius * Math.Cos(degrees * k));
                    float nY = center.Y + (float)(radius * Math.Sin(degrees * k));

                    flamesID = NPC.NewNPC((int)nX, (int)nY, mod.NPCType("m_flame"));
                    if(Main.netMode != 0)
                        NetMessage.SendData(23, -1, -1, null, flamesID, 0f, 0f, 0f, 0, 0, 0);

                    Main.npc[flamesID].ai[1] = degrees * k;
                    flames = true;
                }
            }
            if (flames)
            {
                radius -= 0.5f;
                NPC n = Main.npc[flamesID];
                if (n.active = false || radius <= 1f)
                    flames = false;
            }
            #endregion

            #region magno clone sequence
            /*  if (!pupsSpawned && Main.rand.Next(0, 6000) == 0)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        pups = NPC.NewNPC((int)npcCenter.X + Main.rand.Next(-npc.width, npc.width), (int)npcCenter.Y, mod.NPCType("m_diggerhead"));
                        NetMessage.SendData(23, -1, -1, null, pups, 0f, 0f, 0f, 0, 0, 0);
                        pupsSpawned = true;
                    }
                }
                if (pupsSpawned)
                {
                    Main.npc[pups].realLife = Main.npc[pups].whoAmI;
                    if (!Main.npc[pups].active)
                    {
                        pupsSpawned = false;
                        magnoClone = true;
                    }
                }
                if (magnoClone)
                {
                    clone = NPC.NewNPC((int)npcCenter.X, (int)npcCenter.Y + 128, mod.NPCType("m_mimic"));
                    Main.npc[clone].color = Color.Gold;
                    Main.npc[clone].scale = 0.6f;
                    timeLeft = 600;
                    magnoClone = false;
                }
                if(timeLeft > 0)
                    timeLeft--;
                if (timeLeft == 0)
                {
                    Main.npc[clone].active = false;
                    timeLeft = 600;
                }   */
            #endregion
        }
        public void SpawnDust(Vector2 vector, int width, int height, int dustType, Color color, float scale)
        {
            dust = Dust.NewDust(vector, width, height, dustType, 0f, 0f, 255, color, scale);
            Main.dust[dust].noGravity = true;
        }
        public float Random()
        {
            return Main.rand.Next(-400, 400);
        }
        public Vector2 TargetSD(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}
