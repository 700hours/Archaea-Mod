using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class m_fanatic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac Fanatic");
            Main.npcFrameCount[npc.type] = 6;
            //  to circumvent custom spawning net code
            //  unnecessary for final version
            //  Main.npcCatchable[npc.type] = true;
        }
        public override void SetDefaults()
        {
            npc.width = 76;
            npc.height = 76;
            npc.friendly = false;
            npc.aiStyle = -1;
            npc.damage = 15;
            npc.defense = 10;
            npc.lifeMax = 120;
            npc.value = 500;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }

        public void Initialize()
        {
            oldPos = npc.position;
            placement = npc.Hitbox;
            samePos = 300;
            projTimer = 90;

            oldLife = npc.life;

            SelectTargets();
            player = Main.player[npc.target];
            
            for (int i = 0; i < 128; i++)
            {
                if (FoundLocation(player, npc.height))
                {
                    SyncNPC(teleportTo.X, teleportTo.Y, npc.height);
                    break;
                }
            }
        }
        
    /*  public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(teleportTo);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            npc.position = reader.ReadVector2();
        }   */

        public int samePos
        {
            get { return (int)npc.localAI[0]; }
            set { npc.localAI[0] = value; }
        }
        public int projTimer
        {
            get { return (int)npc.localAI[1]; }
            set { npc.localAI[1] = value; }
        }

        bool init;
        bool varSet;
        bool active;
        bool ground;
        bool netPos;
        int proj = 15, proj2 = 20, explode;
        int oldTarget, target;
        int ID;
        int num, frame;
        int oldLife;
        int[] targets = new int[255];
        const int TileSize = 16;
        float randX, randY;
        Vector2 oldPos;
        Vector2 teleportTo;
        Vector2 projEnd;
        Vector2 lead;
        Rectangle placement;
        Rectangle endPoint;
        Player player = Main.player[Main.myPlayer];
        public override void AI()
        {
            if(!init)
            {
                Initialize();
                init = true;
            }

            player = Main.player[npc.target];
            
            if(player.dead || !player.active || player.statLife <= 0)
            {
                SelectTargets();
            }

            if (oldLife > npc.life)
            {
                samePos += 60;
                oldLife = npc.life;
            }

            if (placement.Intersects(npc.Hitbox))
            {
                samePos--;
                if (samePos < 0)
                {
                    if (!TargetInRange(player, 128f))
                    {
                        if (FoundLocation(player, npc.height))
                        {
                            SyncNPC(teleportTo.X, teleportTo.Y, npc.height);
                            Reset(player);
                        }
                    }
                }
            }
            else
            {
                placement = npc.Hitbox;
            }

            if (Main.netMode < 2)
            {
                if (TargetInRange(player, 512f))
                {
                    projTimer--;
                    projEnd = TargetLead(player);
                }
                if (projTimer <= 24 && projTimer > -1)
                {
                    int leftHand = Dust.NewDust(npc.Center + new Vector2(32f, 0), 8, 8, mod.DustType("magno_dust"), 0f, -4f, 0, Color.White, 1.2f);
                    int rightHand = Dust.NewDust(npc.Center - new Vector2(32f, 0), 8, 8, mod.DustType("magno_dust"), 0f, -4f, 0, Color.White, 1.2f);
                }
                if (projTimer < 0)
                {
                    Main.PlaySound(2, npc.Center, 8);
                    float projAngle = (float)Math.Atan2(npc.Center.Y - projEnd.Y, npc.Center.X - projEnd.X);
                //  int damage = Main.expertMode ? 12 : 4;
                //  bool crit = Main.rand.Next(20) == 0;
                    proj = Projectile.NewProjectile(npc.Center + new Vector2(32f, 0f), AngularVel(8f, projAngle + MathHelper.ToRadians(180f)), mod.ProjectileType("magno_flame"), 8, 8f, 255, 0f, 0f);
                    proj2 = Projectile.NewProjectile(npc.Center - new Vector2(32f, 0f), AngularVel(8f, projAngle + MathHelper.ToRadians(180f)), mod.ProjectileType("magno_flame"), 8, 8f, 255, 0f, 0f);
                    Projectile flame = Main.projectile[proj];
                    flame.hostile = true;
                    flame.timeLeft = 300;
                    flame.tileCollide = false;
                    Projectile flame2 = Main.projectile[proj2];
                    flame2.hostile = true;
                    flame2.timeLeft = 300;
                    flame2.tileCollide = false;

                    projEnd = Vector2.Zero;
                    lead = Vector2.Zero;
                    projTimer = 90;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (!Main.dedServ)
                num = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];

            if (projTimer <= 24 && projTimer > -1)
            {
                if (projTimer % 6 == 0 && frame < 5)
                {
                    frame++;
                }
            }
            else frame = 0;

            npc.frame.Y = num * frame;
        }

        public override void NPCLoot()
        {
            int[] item = new int[2];
            int choice = Main.rand.Next(3);
            switch (choice)
            {
                case 0:
                    item[0] = mod.ItemType("magno_core");
                    break;
            }
            int rareDrop = Main.rand.Next(20);
            switch (rareDrop)
            {
                case 0:
                    item[1] = mod.ItemType("magno_summonstaff");
                    break;
            }
            foreach (int i in item)
            {
                if (i != default(int))
                {
                    Item.NewItem(npc.position, i, 1, false, -1, true, false);
                }
            }
        }

        public void Reset(Player npcTarget)
        {
            oldPos = npc.position;
            placement = npc.Hitbox;
            samePos = 300;

            if (npcTarget.position.X < npc.position.X)
            {
                npc.spriteDirection = -1;
            }
            else npc.spriteDirection = 1;

            if (!TargetInRange(player, 512f))
            {
                SelectTargets();
            }
        }

        private void SyncNPC(float x, float y, int height)
        {
            if (Main.netMode == 2)
            {
                npc.position = new Vector2(x, y - height);
                NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI, npc.position.X, npc.position.Y);
                npc.netUpdate = true;
            }
            if (Main.netMode == 0)
            {
                npc.position = new Vector2(x, y - height);
            }
        }

        public bool FoundLocation(Player npcTarget, int height)
        {
            randX = Main.rand.Next((int)npcTarget.position.X - 512, (int)npcTarget.position.X + 512);
            randY = Main.rand.Next((int)npcTarget.position.Y - 512, (int)npcTarget.position.Y + 512);
            active = IsTile((int)randX, (int)randY);
            ground = SolidGround((int)randX, (int)randY, height);
            if(!active && ground)
            {
                teleportTo = new Vector2(randX, randY - height);
            }
            return !active && ground;
        }

        public bool TargetInRange(Player npcTarget, float range)
        {
            if (Vector2.Distance(npc.position - npcTarget.position, Vector2.Zero) < range)
            {
                return true;
            }
            else return false;
        }

        public Vector2 TargetLead(Player npcTarget)
        {
            lead += npcTarget.velocity / 2;
            if (npcTarget.velocity.X == 0f) lead.X = 0f;
            if (npcTarget.velocity.Y == 0f) lead.Y = 0f;
            return npcTarget.position + lead;
        }

        public void SelectTargets()
        {
            if (Main.netMode != 0)
            {
                foreach (Player p in Main.player)
                {
                    if (p.active && !p.dead && p != null)
                    {
                        if (Vector2.Distance(npc.position - p.position, Vector2.Zero) < 1536f)
                        {
                            oldTarget = npc.target;
                            npc.target = p.whoAmI;

                            if (ID < targets.Length) ID++;
                            targets[ID] = oldTarget;

                            if (oldTarget == p.whoAmI)
                                break;
                        }
                    }
                }
            /*  foreach(int i in targets)
                { 
                    Player npcTarget = Main.player[targets[i]];
                    if (npcTarget.active && !npcTarget.dead && npcTarget != null)
                    {
                        if (Vector2.Distance(npc.position - npcTarget.position, Vector2.Zero) < 1024f)
                        {
                            oldTarget = npc.target;
                            npc.target = npcTarget.whoAmI;
                            break;
                        }
                        else npc.target = Main.myPlayer;
                    }
                }   */
            }
            else npc.TargetClosest();
        }

        public bool IsTile(int x, int y)
        {
            int i = x / 16;
            int j = y / 16;
            Tile tile = Main.tile[i, j];

            bool Active = tile.active();
            bool Solid = Main.tileSolid[tile.type];

            if (Active && Solid) return true;
            else return false;
        }

        public bool SolidGround(int x, int y, int height)
        {
            int i = x / 16;
            int j = y / 16;
            int offset = height * 2 / 16;
            Tile tile = Main.tile[i, j + offset];

            bool Active = tile.active();
            bool Solid = Main.tileSolid[tile.type];

            if (Active && Solid) return true;
            else return false;
        }

        public Vector2 AngularVel(float radius, float angle)
        {
            float velX = (float)(radius * Math.Cos(angle));
            float velY = (float)(radius * Math.Sin(angle));

            return new Vector2(velX, velY);
        }
    }
}
