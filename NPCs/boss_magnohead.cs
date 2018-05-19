using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.NPCs
{
    public class boss_magnohead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac Entity");
        }
        public override void SetDefaults()
        {
            npc.width = 50;
            npc.height = 50;
            npc.scale = 1f;
            npc.friendly = false;
            npc.aiStyle = 6;
          //aiType = NPCID.Worm;
            npc.boss = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;
            npc.damage = 35;
            npc.defense = 18;
            npc.lifeMax = 9350;
            npc.value = 25000;
            npc.npcSlots = 15f;
            //  custom or vanilla sounds?
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            //
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/The_Undying_Flare");
            npc.knockBackResist = 0f;
            bossBag = mod.ItemType("magno_treasurebag");
        }

        #region disorganized declarations
        bool TailSpawned = false;
        bool flames = false;
        bool pupsSpawned = false;
        bool magnoClone = false;
        bool soundOnce = false;
        bool part2 = false;
        bool towerAbove = false;
        bool set1 = false, set2 = false;
        bool goLeft = false, goRight = false;
        bool ProjActive = false;
        int Previous, digger, despawn = 210;
        int timeLeft = 600;
        int flamesID;
        int extraTimer = 90;
        int Proj1 = 0, Proj2 = 0;
        float TargetAngle, PlayerAngle;
        float radius = 64;
        float Depreciate = 80, Point;
        float WaveTimer;
        const float Time = 80;
        const float radians = 0.017f;
        Rectangle target;
        Vector2 oldPosition, newPosition;
        Vector2 npcCenter, playerCenter, center;
        Vector2 Start, End;
        Vector2 lootPos;
        #endregion
        /// <summary>
        /// reorganize declarations
        /// </summary>

        bool spawned;
        bool Flag, Flag2, Flag3, Flag4, Flag5, Flag6, Flag7, Flag8;
        bool FLAG;
        bool pattern, pattern2;
        bool switchSides;
        bool npcNoDamage;
        bool tractorBeam;
        bool rush;
        int ticks;
        int attack;
        int core;
        int clone;
        int pups;
        int bufferX = 320, bufferY = 192;
        int timer, timer2 = 5, copyTimer = 900, rushTimer = 270;
        int enrage;
        int copycat = 180;
        int[] MagnoParts = new int[8];
        int[] numPets = new int[] { 100, 101, 102 };
        float degrees, degrees2;
        Vector2 zoneRight, zoneLeft;
        public override void AI()
        {
            Player player = Main.player[npc.target];
            ArchaeaPlayer modPlayer = player.GetModPlayer<ArchaeaPlayer>(mod);
            npcCenter = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2);

            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }

            if (ticks < 2000) ticks++;
            else ticks = 0;

            #region spawn parts
            Previous = npc.whoAmI;
            if (!spawned)
            {
                for (int i = 0; i < 11; i++)
                {
                    if (i != 10)
                    {
                        digger = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType<boss_magnobody>(), npc.whoAmI, 0f, 0f, 0f, npc.whoAmI);
                    }
                    else
                    {
                        digger = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType<boss_magnotail>(), npc.whoAmI, 0f, 0f, 0f, npc.whoAmI);
                    //  if (Main.expertMode) core = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType<m_core>(), npc.whoAmI, npc.whoAmI);
                    }
                    NPC nme = Main.npc[digger];
                    nme.target = npc.target;
                    nme.scale = npc.scale;
                    nme.defense = npc.defense;
                    nme.lifeMax = npc.lifeMax;
                    nme.realLife = npc.whoAmI;
                    nme.ai[2] = npc.whoAmI;
                    nme.ai[1] = Previous;
                    Main.npc[Previous].ai[0] = digger;
                    Previous = digger;

                    spawned = true;
                }
            }
        /*  if (Main.expertMode && !Flag7)
            {
                NPC HeadCore = Main.npc[core];
                HeadCore.Center = npc.Center;
                HeadCore.rotation = npc.rotation;
                if (HeadCore.life > 0 || HeadCore.active)
                    npc.dontTakeDamage = true;
                else
                {
                    npc.dontTakeDamage = false;
                    Flag7 = true;
                }
            }   */
            #endregion

            #region movement variation
            if (ticks % 900 == 0)
            {
                extraTimer = 150;
                towerAbove = true;
            }
            if (towerAbove)
            {
                extraTimer--;
                npc.velocity = Vector2.Zero;
                if (npc.position.Y < player.position.Y - 256f)
                {
                    if(!set2)
                    {
                        if (player.position.X <= npc.position.X)
                            goLeft = true;
                        if (player.position.X >= npc.position.X)
                            goRight = true;
                        set2 = true;
                    }
                    if (goLeft)
                    {
                        npc.position.X -= 8f;
                        npc.rotation = MathHelper.ToRadians(270);
                    }
                    if (goRight)
                    {
                        npc.position.X += 8f;
                        npc.rotation = MathHelper.ToRadians(90);
                    }
                    set1 = true;
                    npc.netUpdate = true;
                }
                if (!set1)
                {
                    npc.position.Y -= 8f;
                    npc.rotation = 0;
                }
                // play sound, rush
                if (extraTimer == 0)
                {
                    set1 = false;
                    set2 = false;
                    goLeft = false;
                    goRight = false;
                    towerAbove = false;
                }
            }
            #endregion

            #region mouth flames
            if (Vector2.Distance(player.position - npc.position, Vector2.Zero) < 384f)
            {
                if (ticks % 10 == 0)
                {
                    Vector2 burst = Distance(null, npc.rotation + radians * 270f, 16f);
                    attack = Projectile.NewProjectile(npc.Center, burst, mod.ProjectileType("magno_flame"), 8 + Main.rand.Next(-2, 5), 0.5f, Main.myPlayer, 20f, 1f);
                    Projectile proj = Main.projectile[attack];
                    proj.timeLeft = 30;
                    proj.penetrate = 1;
                    proj.tileCollide = false;
                    proj.scale = 1.6f;
                    proj.alpha = 50;
                    proj.friendly = false;
                    proj.hostile = true;
                }
            }
            #endregion

            // unused
            #region npc damage modifier
            /*  if (npcNoDamage)
                {
                    npc.damage = 0;
                    for (int i = 0; i < MagnoParts.Length; i++)
                    {
                        NPC nme = Main.npc[MagnoParts[i]];
                        nme.damage = 0;
                    }
                }
                else
                {
                    npc.damage = Main.expertMode ? 60 : 35;
                    for (int i = 0; i < MagnoParts.Length; i++)
                    {
                        NPC nme = Main.npc[MagnoParts[i]];
                        nme.damage = Main.expertMode ? 36 : 18;
                    }
                }   */
            #endregion
            #region tower and grab
            /*  if (timer2 <= 4)
                {
                    if (!flag2)
                    {
                        Rectangle npcPoint = new Rectangle((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height / 2, 32, 32);
                        if (!switchSides)
                        {
                            Rectangle rightPoint = new Rectangle((int)player.position.X + bufferX, (int)player.position.Y + bufferY, 32, 32);
                            Vector2 rightPosition = new Vector2(player.position.X + bufferX, player.position.Y + bufferY);
                            float angle3 = (float)Math.Atan2(rightPosition.Y - npc.position.Y, rightPosition.X - npc.position.X);
                            npc.velocity = Distance(player, angle3, 8f);
                            if (npcPoint.Intersects(rightPoint))
                            {
                                pattern = true;
                                flag2 = true;
                                timer2++;
                            }
                        }
                        else
                        {
                            Rectangle leftPoint = new Rectangle((int)player.position.X - bufferX, (int)player.position.Y + bufferY, 32, 32);
                            Vector2 leftPosition = new Vector2(player.position.X - bufferX, player.position.Y + bufferY);
                            float angle4 = (float)Math.Atan2(leftPosition.Y - npc.position.Y, leftPosition.X - npc.position.X);
                            npc.velocity = Distance(player, angle4, 8f);
                            if (npcPoint.Intersects(leftPoint))
                            {
                                pattern = true;
                                flag2 = true;
                                timer2++;
                            }
                        }
                    }
                    if (pattern)
                    {
                        degrees += radians;
                        float radius = 2f;
                        float Point = (float)(radius * Math.Cos(degrees));
                        float intensity = (radius - Point) / radius;

                        zoneRight = Vector2.Lerp(new Vector2(player.position.X + bufferX, player.position.Y + bufferY), new Vector2(player.Center.X + bufferX, player.position.Y - bufferY / 4), intensity);
                        zoneLeft = Vector2.Lerp(new Vector2(player.position.X - bufferX, player.position.Y + bufferY), new Vector2(player.Center.X - bufferX, player.position.Y - bufferY / 4), intensity);
                        if (Math.Round(intensity, 0) == 1f)
                        {
                            timer++;
                        }
                        if (timer < 128)
                        {
                            if (!switchSides)
                                npc.position = zoneRight;
                            else npc.position = zoneLeft;
                        }
                        if (timer >= 128)
                        {
                            switchSides = !switchSides;
                            flag2 = false;
                            pattern = false;
                            timer = 0;
                        }
                    }
                    if (ticks % 120 == 0)
                    {
                        bool chance = Main.rand.Next(5) == 0;
                        if (chance)
                        {
                            int mob = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType<m_diggerhead>());
                            if (Main.netMode == 2) NetMessage.SendData(23, -1, -1, null, numPets[0], 0f, 0f, 0f, 0, 0, 0);
                        }
                    }
                    degrees2 += radians;
                    float radius2 = 8f;
                    float Point2 = (float)(radius2 * Math.Cos(degrees2));
                    float intensity2 = (radius2 - Point2) / radius2;
                    float Angle2 = (float)Math.Atan2(npc.Center.Y - player.Center.Y, npc.Center.X - player.Center.X);
                    if (!flag4 && Vector2.Distance(npc.position - player.position, Vector2.Zero) < 256f)
                    {
                        player.velocity.X = Distance(player, Angle2, radius2).X;
                        player.velocity.Y = Distance(player, Angle2, radius2).Y;
                    }
                    if (!flag4 && player.Hitbox.Intersects(npc.Hitbox))
                    {
                        npcNoDamage = true;
                        player.Center = npc.Center;
                        bool chance = Main.rand.Next(60) == 0;
                        if (chance)
                        {
                            player.velocity.X = Distance(player, npc.rotation - MathHelper.ToRadians(90), 32f).X;
                            player.velocity.Y = Distance(player, npc.rotation - MathHelper.ToRadians(90), 32f).Y;
                            flag4 = true;
                            npcNoDamage = false;
                        }
                    }
                }*/
            #endregion
            //

            if(!Flag3 && npc.life < npc.lifeMax - npc.lifeMax / 4)
            {
                timer2 = 0;
                Flag3 = true;
            }

            // bugged
            #region spirit flames
            /*  //  needs cleanup
            if (!flames && Main.rand.Next(0, 6000) == 0)
            {
                for (int k = 1; k < 4; k++)
                {
                    degrees = 67.5f;
                    radius = 128f;
                    center = player.position;
                    float nX = center.X + (float)(radius * Math.Cos(degrees * k));
                    float nY = center.Y + (float)(radius * Math.Sin(degrees * k));

                    if (npc.life > npc.lifeMax / 2 || Main.npc[digger].life > Main.npc[digger].lifeMax / 2)
                    {
                        flamesID = NPC.NewNPC((int)nX, (int)nY, mod.NPCType("m_flame"));
                        Main.npc[flamesID].damage = 10;
                    }
                    else
                    {
                        flamesID = NPC.NewNPC((int)nX, (int)nY, mod.NPCType("c_flame"));
                        Main.npc[flamesID].damage = 16;
                    }
                    Main.npc[flamesID].ai[1] = degrees * k;
                    Main.npc[flamesID].scale = 1.2f;
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(23, -1, -1, null, flamesID, 0f, 0f, 0f, 0, 0, 0);
                    }
                    flames = true;
                }
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/curse"), npc.Center);
            }
            if (flames)
            {
                radius -= 0.5f;
                NPC n = Main.npc[flamesID];
                if (n.active = false || radius <= 1f)
                    flames = false;
            }
            */
            #endregion
            #region magno clone sequence
            // needs to sprite/code separate NPC for magno clone
            /*
            npcCenter = new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2);
            if (!magnoClone)
            {
                if (!pupsSpawned && ticks == 900)
                {
                    ticks++;
                    numPets[0] = NPC.NewNPC((int)npcCenter.X + Main.rand.Next(-64, 64), (int)npcCenter.Y, mod.NPCType<m_puphead>(), 0, 0f);
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(23, -1, -1, null, numPets[0], 0f, 0f, 0f, 0, 0, 0);
                    }
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/conjure"), npc.Center);
                    pupsSpawned = true;
                }
                if (pupsSpawned)
                {
                    if (!Main.npc[numPets[0]].active)
                    {
                        pupsSpawned = false;
                        magnoClone = true;
                        flag = true;
                    }
                }
            }
            if (magnoClone)
            {
                if (flag)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/conjure"), npc.Center);
                    clone = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType<clone_magnohead>(), npc.whoAmI, npc.whoAmI);
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(23, -1, -1, null, clone, 0f, 0f, 0f, 0, 0, 0);
                    }
                    Main.npc[clone].color = Color.Gold;
                    flag = false;
                }
                else
                {
                    magnoClone = false;
                }
            }
            if (Main.expertMode && Main.npc[clone].active)
            {
                npc.immortal = true;
                npc.dontTakeDamage = true;
            }
            if (!Main.npc[clone].active)
            {
                npc.immortal = false;
                npc.dontTakeDamage = false;
                flag = true;
                magnoClone = false;
            }   */
            #endregion
            //
            
            #region patterns
            float angleToPlayer = (float)Math.Atan2(player.position.Y - npc.position.Y, player.position.X - npc.position.X);

            if (ticks % 600 == 0)
            {
                rush = true;
                enrage++;
            }
            if(rush && !towerAbove && !IsDigging((int)npcCenter.X, (int)npcCenter.Y))
            {
                if (rushTimer > 0)
                {
                    if (rushTimer % 90 == 0)
                    {
                        if (enrage < 16)
                        {
                            npc.velocity = Distance(null, angleToPlayer, 8f);
                        }
                        else
                        {
                            npc.velocity = Distance(null, angleToPlayer, 8f) * 1.25f;
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/blast"), npc.Center);
                        }
                        npc.netUpdate = true;
                    }
                    rushTimer--;
                }
                else
                {
                    rushTimer = 270;
                    rush = false;
                }
            }

            if (part2)
            {
                if (ticks % 900 == 0 && !Flag5)
                {
                    Flag5 = true;
                }
                if (Flag5)
                {
                    copyTimer--;
                }
                if (copyTimer < 0 || Flag6)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType<clone_magnohead>());
                    }
                    copyTimer = 900;
                    Flag6 = false;
                }
                foreach (NPC n in Main.npc)
                {
                    if (n.type == mod.NPCType<clone_magnohead>())
                    {
                        if (n.active && n.life > 0)
                        {
                            npc.dontTakeDamage = true;
                            copyTimer = 900;
                        }
                        else
                        {
                            npc.dontTakeDamage = false;
                            Flag5 = true;
                        }
                    }
                }
            }
            #endregion

            // play NPC sound at half life
            if (npc.life < npc.lifeMax / 2 && !part2)
            {
                if (!soundOnce)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/blast"), npc.Center);
                    soundOnce = true;
                }
                timer2 = 0;
                Flag6 = true;
                part2 = true;
            }
            
            foreach(Projectile proj in Main.projectile)
            {
                if(proj.Hitbox.Intersects(npc.Hitbox) && proj.friendly && proj.active && proj.type != 96)
                {
                    lootPos = proj.position;
                }
            }
        }
        
        public override bool SpecialNPCLoot()
        {
            return false;
        }
        public override void NPCLoot()
        {
            ArchaeaWorld modWorld = mod.GetModWorld<ArchaeaWorld>();
            modWorld.MagnoDefeated = true;

            Item.NewItem(lootPos, ItemID.Heart, 6, false, 0, false, false);
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            for(int k = 0; k < 2; k++)
            {
                Item.NewItem(lootPos, mod.ItemType("magno_fragment"), 8 + Main.rand.Next(2, 6), false, 0, false, false);
            }
            int choice = Main.rand.Next(8);
            int item = 0;
            switch (choice)
            {
                case 0:
                    item = mod.ItemType("magno_trophy");
                    break;
            }
            Item.NewItem(lootPos, item, 1, false, 0, true, false);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax += (npc.lifeMax / 5) * numPlayers;
            npc.damage = (int)(npc.damage / 0.90f);
            bossLifeScale = 1.67f;
            npc.defense += 10;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = spawned;
            writer.Write(flags[0]);
            writer.Write(npc.dontTakeDamage);
            writer.Write(core);
            writer.Write(ticks);
            writer.Write(digger);
            for (int i = 0; i < numPets.Length; i++)
            {
                writer.Write(numPets[i]);
            }
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            spawned = flags[0];
            npc.dontTakeDamage = reader.ReadBoolean();
            core = reader.ReadInt32();
            ticks = reader.ReadInt32();
            digger = reader.ReadInt32();
            for (int i = 0; i < numPets.Length; i++)
            {
                numPets[i] = reader.ReadInt32();
            }
        }

        public override void BossHeadSlot(ref int index)
        {
            index = NPCHeadLoader.GetBossHeadSlot(TestMod.magnoHead);
        }

        public bool IsDigging(int x, int y)
        {
            int i = x / 16;
            int j = y / 16;
            Tile tile = Main.tile[i, j];
            bool Active = tile.active();
            bool Solid = Main.tileSolid[tile.type];

            if (Active && Solid) return true;
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
