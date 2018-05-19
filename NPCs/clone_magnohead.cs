using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class clone_magnohead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac Copycat");
        }
        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 24;
            npc.friendly = false;
            npc.aiStyle = 6;
            npc.scale = 1f;
            //  aiType = NPCID.Worm;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;
            npc.damage = 20;
            npc.defense = 10;
            npc.lifeMax = 380;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }
        
        public void Initialize()
        {
            patternTimer = 270;
        }

        public int patternTimer
        {
            get { return (int)npc.localAI[0]; }
            set { npc.localAI[0] = value; }
        }

        bool tailSpawned;
        bool invincible;
        bool rush;
        bool active = true;
        bool init;
        int Previous2;
        int digger2;
        int ticks = 60;
        int timeLeft = 1800;
        float degrees;
        const float radians = 0.017f;
        Vector2 center;
        int[] parts = new int[5];
        Mod mod = ModLoader.GetMod("ArchaeaMod");
        public override void AI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }

            #region dig AI
            if (!tailSpawned)
            {
                Previous2 = npc.whoAmI;
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i != 4)
                    {
                        digger2 = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("clone_magnobody"), npc.whoAmI);
                    }
                    else
                    {
                        digger2 = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("clone_magnotail"), npc.whoAmI);
                    }
                    NPC nme = Main.npc[digger2];
                    nme.realLife = npc.whoAmI;
                    nme.ai[2] = (float)npc.whoAmI;
                    nme.ai[1] = (float)Previous2;
                    Main.npc[Previous2].ai[0] = digger2;

                    Previous2 = digger2;
                }
                tailSpawned = true;
            }
            #endregion

            Player player = Main.player[npc.target];
            float angleToPlayer = (float)Math.Atan2(player.position.Y - npc.position.Y, player.position.X - npc.position.X);
            
            ticks++;
            timeLeft--;
            if(ticks % 600 == 0)
            {
                rush = true;
            }
            if(rush)
            {
                if (patternTimer > 0)
                {
                    if (patternTimer % 90 == 0)
                    {
                        npc.velocity = Distance(null, angleToPlayer, 8f);
                        npc.netUpdate = true;
                    }
                    patternTimer--;
                }
                else
                {
                    patternTimer = 270;
                    rush = false;
                }
            }

            #region invincibility
        /*  foreach (Player p in Main.player)
            {
                if (p.active && p != null && !p.dead)
                {
                    if (p.HasBuff(mod.BuffType("magno_uncursed")))
                    {
                        invincible = false;
                        break;
                    }
                    else
                    {
                        invincible = true;
                    }
                }
            }   
            foreach (NPC n in Main.npc)
            {
                if (n.active && (n.type == mod.NPCType<boss_magnobody>() || n.type == mod.NPCType<boss_magnohead>())) 
                {
                    if (invincible)
                    {
                        n.defense = 24;
                    }
                    else
                    {
                        n.defense = 6;
                    }
                }
            }   */
            #endregion
            foreach(NPC n in Main.npc)
            {
                if (!n.active || n.life < 0)
                {
                    if (n.type == mod.NPCType<boss_magnohead>())
                    {
                        active = false;
                        break;
                    }
                }
            }

            if (!Main.npc[(int)npc.ai[0]].active || !active || timeLeft < 0)
            {
                npc.active = false;
            }
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = tailSpawned;
            writer.Write(flags[0]);
            writer.Write(digger2);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            tailSpawned = flags[0];
            digger2 = reader.ReadInt32();
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}
