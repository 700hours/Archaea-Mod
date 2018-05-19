using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class boss_magnobody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac Entity");
        }
        public override void SetDefaults()
        {
            npc.width = 62;
            npc.height = 62;
            npc.friendly = false;
            npc.aiStyle = 6;
            //aiType = NPCID.Worm;
            npc.boss = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;
            npc.damage = 18;
            npc.defense = 4;
            npc.lifeMax = 10350;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }
        
        bool init = false;
        bool flag;
        int ticks = 0;
        int bodyCore;
        float radius = 0f;
        float degrees = 0f;
        const float radians = 0.017f;

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(bodyCore);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            bodyCore = reader.ReadInt32();
        }

        public void Initialize()
        {
        /*  if (Main.expertMode)
            {
                bodyCore = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, mod.NPCType<m_core>());
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(23, -1, -1, null, bodyCore, 0f, 0f, 0f, 0, 0, 0);
                }
            }   */
        }
        public override void AI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }
            
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }

            npc.scale = Main.npc[(int)npc.ai[3]].scale;

            Player player = Main.player[npc.target];
            ticks++;

            #region body flames
            if (Main.expertMode)
            {
                bool chance = Main.rand.Next(10) == 0;
                if (ticks % 60 == 0 && chance)
                {
                    Vector2 center = npc.position + new Vector2(npc.width / 2, npc.height / 2);
                    Vector2 randVelocity = new Vector2(Main.rand.Next(-4, 4), -6f);
                    int type = mod.ProjectileType("cinnabar_flame");
                    int attack = Projectile.NewProjectile(center, randVelocity, type, 8 + Main.rand.Next(-2, 5), 0.5f, 255, 0f, 0f);
                    Main.projectile[attack].timeLeft = 300;
                    Main.projectile[attack].hostile = true;

                    Main.PlaySound(2, npc.Center, 20);
                }
            }
            /*  if (Vector2.Distance(player.position - npc.position, Vector2.Zero) < 320f)
                {
                    radius += 1f;
                    degrees += radians * 4.5f;
                    if (npc.life <= npc.lifeMax)
                    {
                        if (ticks % 180 == 0)
                        {
                            attack = Projectile.NewProjectile(npc.position + new Vector2(npc.width / 2, npc.height / 2), Vector2.Zero, 96, 8 + Main.rand.Next(-2, 5), 0.5f, Main.myPlayer, 0f, 0f);
                            Main.projectile[attack].timeLeft = 45;
                        }
                        Projectile proj = Main.projectile[attack];
                        proj.position.X = npc.Center.X + (float)(radius * Math.Cos(degrees));
                        proj.position.Y = npc.Center.Y + (float)(radius * Math.Sin(degrees));
                        proj.penetrate = 1;
                        proj.tileCollide = false;
                        proj.friendly = false;
                        proj.hostile = true;
                    }
                }
                else
                {
                    radius = 0f;
                    degrees = 0f;
                }   */
            #endregion

            if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].life <= 0)
            {
                npc.life = 0;
                npc.HitEffect(0, 10.0);
                npc.active = false;
            }

            if(Main.npc[(int)npc.ai[1]].dontTakeDamage)
            {
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }
        /*  if (Main.expertMode)
            {
                n = Main.npc[bodyCore];
                n.rotation = npc.rotation;
                n.Center = npc.Center;
                if (n.life > 0 || n.active)
                {
                    npc.dontTakeDamage = true;
                }
                else
                {
                    npc.dontTakeDamage = false;
                }
            }   */
        }
    }
}
