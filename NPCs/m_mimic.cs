using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class m_mimic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Mimic");
            Main.npcFrameCount[npc.type] = 6;
            //  to circumvent custom spawning net code
            //  unnecessary for final version
            //  Main.npcCatchable[npc.type] = true;
        }
        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 44;
            npc.friendly = false;
            npc.aiStyle = -1;
            npc.damage = 10;
            npc.defense = 6;
            npc.lifeMax = 800;
            npc.value = 7500;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
        }
        bool init = false;
        bool alarmed = true, navigate = false;
        bool detected = false;
        int ticks = 0, position = 0, rotations = 0;
        float Depreciate = 60, Point, Time = 60;
        const int AItime = 600;
        const float radians = 0.017f;
        Vector2 Start, End;
        Vector2 vector;
        Vector2 Position;
        public void Initialize()
        {
            npc.TargetClosest(true);
        }
        public override void AI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }

            if (detected)
                ticks++;

            Player player = Main.player[npc.target];

            if (!detected && (Vector2.Distance(player.position - npc.position, Vector2.Zero) < 172f || npc.life < npc.lifeMax))
            {
                npc.TargetClosest(true);
                detected = true;
            }

            Position = new Vector2(player.position.X, player.position.Y);
            #region dust
            int dustType = 71;
            float scale = 1f;
            if (ticks % 6 == 0 && navigate && !alarmed)
            {
                int TL = Dust.NewDust(Position + new Vector2(-128, -128),
                                        16, 16, dustType, 0f, 0f, 255, Color.White, scale);
                int TR = Dust.NewDust(Position + new Vector2(128, -128),
                                        16, 16, dustType, 0f, 0f, 255, Color.White, scale);
                int BL = Dust.NewDust(Position + new Vector2(-128, 128),
                                        16, 16, dustType, 0f, 0f, 255, Color.White, scale);
                int BR = Dust.NewDust(Position + new Vector2(128, 128),
                                        16, 16, dustType, 0f, 0f, 255, Color.White, scale);
                Main.dust[TL].noGravity = true;
                Main.dust[TR].noGravity = true;
                Main.dust[BL].noGravity = true;
                Main.dust[BR].noGravity = true;
            }
            #endregion
            #region default behavior
            if (detected && alarmed && (Main.netMode == 2 || Main.netMode == 0))
            {
                npc.noGravity = false;
                npc.rotation = radians;

                if (ticks < AItime)
                {
                    float Angle = (float)Math.Atan2(player.position.Y - npc.position.Y,
                                                     player.position.X - npc.position.X);
                    vector = new Vector2(npc.position.X/16, npc.position.Y/16);
                    if (ticks%60 == 0 && !TileCheck((int)vector.X, (int)vector.Y))
                    {
                        npc.velocity.Y -= 6.5f + Main.rand.Next(0, 4);
                        if (npc.velocity.Y != 0)
                            npc.velocity.X += Distance(player, Angle, 3f).X;
                        
                        // npc facing player when jumping
                        if (npc.position.X < player.position.X)
                            npc.spriteDirection = 1;
                        else if (npc.position.X > player.position.X)
                            npc.spriteDirection = -1;

                        if (Main.netMode != 0)
                            npc.netUpdate = true;
                    }
                    if (npc.velocity.Y == 0f)
                        npc.velocity = Vector2.Zero;
                }
                if (ticks >= AItime)
                {
                    alarmed = false;
                    navigate = true;
                    for (int k = 0; k < 32; k++)
                    {
                        int teleport = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 32, 32, dustType, 0f, 0f, 255, Color.White, scale);
                    }
                    // set start position
                    Start.X = player.position.X - 128;
                    Start.Y = player.position.Y - 128;

                    // set end position
                    End.X = player.position.X + 128;
                    End.Y = Start.Y;
                }
            }
            #endregion
            #region square orbit
            if (detected && navigate)
            {
                npc.noGravity = true;

                // npc looking at player
                float Angle = (float)Math.Atan2(player.position.Y - npc.position.Y,
                                                player.position.X - npc.position.X);
                npc.rotation = Angle;
                npc.spriteDirection = -1;

                if (Depreciate > 0 && !alarmed && rotations < 4)
                {
                    Depreciate--;
                    Point = (Time - Depreciate) / Time;
                    npc.position = Vector2.Lerp(Start, End, Point);
                }
                #region all square directions
                if (Depreciate <= 0 && rotations < 4)
                {
                    position = Main.rand.Next(0, 5);
                    // top right to bottom right    Checked
                    if (position == 0)
                    {
                        // set start position
                        Start.X = player.position.X + 128;
                        Start.Y = player.position.Y - 128;

                        // set end position
                        End.X = Start.X;
                        End.Y = player.position.Y + 128;
                    }
                    // top left to bottom left      Checked
                    if (position == 1)
                    {
                        // set start position
                        Start.X = player.position.X - 128;
                        Start.Y = player.position.Y - 128;

                        // set end position
                        End.X = Start.X;
                        End.Y = player.position.Y + 128;
                    }
                    // bottom right to bottom left  Checked
                    if (position == 2)
                    {
                        // set start position
                        Start.X = player.position.X + 128;
                        Start.Y = player.position.Y + 128;

                        // set end position
                        End.X = player.position.X - 128;
                        End.Y = Start.Y;
                    }
                    // bottom right to top right    Checked
                    if (position == 3)
                    {
                        // set start position
                        Start.X = player.position.X + 128;
                        Start.Y = player.position.Y + 128;

                        // set end position
                        End.X = Start.X;
                        End.Y = player.position.Y - 128;
                    }
                    // top right to top left        Problem? Fixed
                    if (position == 4)
                    {
                        // set start position
                        Start.X = player.position.X + 128;
                        Start.Y = player.position.Y - 128;

                        // set end position
                        End.X = player.position.X - 128;
                        End.Y = Start.Y;
                    }
                    // top left to top right        Problem? Fixed
                    if (position == 5)
                    {
                        // set start position
                        Start.X = player.position.X - 128;
                        Start.Y = player.position.Y - 128;

                        // set end position
                        End.X = player.position.X + 128;
                        End.Y = Start.Y;
                    }
                    for (int k = 0; k < 32; k++)
                    {
                        int teleport = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 32, 32, dustType, 0f, 0f, 255, Color.White, scale);
                    }
                    if (Main.netMode != 1)
                        npc.netUpdate = true;
                    rotations++;
                    Depreciate = 60;
                }
                #endregion

                vector = new Vector2(player.position.X + Main.rand.Next(-400, 400), player.position.Y + Main.rand.Next(-400, 400));
                if (rotations >= 4 && !TileCheck((int)vector.X/16, (int)vector.Y/16))
                {
                    // pre teleport dust
                    for (int k = 0; k < 8; k++)
                    {
                        int teleport = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 32, 32, dustType, 0f, 0f, 255, Color.White, scale);
                    }
                    if (Main.netMode != 0)
                        npc.netUpdate = true;
                    npc.position = vector;
                    // post teleport dust
                    for (int k = 0; k < 8; k++)
                    {
                        int teleport = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 32, 32, dustType, 0f, 0f, 255, Color.White, scale);
                    }
                    navigate = false;
                    ticks = 0;
                    alarmed = true;
                    rotations = 0;
                }
            }
            #endregion  
        }
        
        int num = 0, frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (!Main.dedServ)
                num = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];

            if (!detected)
                npc.frame.Y = frame;
            
            if (npc.velocity.Y < 0f && frame < 5)
                frame++;
            if (npc.velocity.Y > 0f && frame > 1)
                frame--;

            npc.frame.Y = num * frame;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            bossLifeScale = 0f;
        }

        public override void NPCLoot()
        {
            int choice = Main.rand.Next(3);
            int item = 0;
            switch (choice)
            {
                case 0:
                    item = mod.ItemType("magno_core");
                    break;
                case 1:
                    item = mod.ItemType("magno_yoyo");
                    break;
                case 2:
                    item = mod.ItemType("magno_summonstaff");
                    break;
                case 3:
                    item = mod.ItemType("magno_gun");
                    break;
            }
            Item.NewItem(npc.position, item, 1, false, -1, true, false);
        }

        public bool TileCheck(int i, int j)
        {
            bool Active = Main.tile[i, j].active() == false && Main.tile[i + 1, j + 1].active() == false;
            bool Solid = Main.tileSolid[Main.tile[i, j].type] == false && Main.tileSolid[Main.tile[i+1, j+1].type] == false;

            if (Solid && Active) return true;
            else return false;  
        }

        public Vector2 Distance(Player player, float Angle, float Radius)
        {
            float VelocityX = (float)(Radius * Math.Cos(Angle));
            float VelocityY = (float)(Radius * Math.Sin(Angle));

            return new Vector2(VelocityX, VelocityY);
        }
    }
}