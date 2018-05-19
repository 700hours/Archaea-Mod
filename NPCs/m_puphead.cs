using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class m_puphead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac Pet");
        }
        public override void SetDefaults()
        {
            npc.width = 30;
            npc.height = 34;
            npc.friendly = false;
            npc.aiStyle = 6;
            //  aiType = NPCID.Worm;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.behindTiles = true;
            npc.damage = 30;
            npc.defense = 10;
            npc.lifeMax = 180;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }

        public void Initialize()
        {
            degrees = MathHelper.ToRadians(npc.ai[0]);
        }
        bool init;
        bool spawned;
        int Previous;
        float degrees, radius = 256f;
        const float radians = 0.017f;
        Vector2 center;
        int[] parts = new int[5];
        public override void AI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }

            #region spawn parts
            Previous = npc.whoAmI;
            if (!spawned)
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i != 4)
                    {
                        parts[i] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType<m_pupbody>(), npc.whoAmI);
                    }
                    else
                    {
                        parts[i] = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType<m_puptail>(), npc.whoAmI);
                    }
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(23, -1, -1, null, parts[i], 0f, 0f, 0f, 0, 0, 0);
                    }
                    NPC nme = Main.npc[parts[i]];
                    nme.target = npc.target;
                    nme.scale = npc.scale;
                    nme.lifeMax = npc.lifeMax;
                    nme.realLife = npc.whoAmI;
                    nme.ai[2] = npc.whoAmI;
                    nme.ai[1] = Previous;
                    Main.npc[Previous].ai[0] = parts[i];
                    Previous = parts[i];

                    spawned = true;
                }
            }
            #endregion

            npc.color = Color.White;

            npc.TargetClosest(true);

            Player player = Main.player[npc.target];

            center = new Vector2(player.Center.X - npc.width / 2, player.Center.Y - npc.height / 2);
            degrees += radians * 2.25f;
            if (radius > 8)
                radius = 0.5f;
            else npc.active = false;

            center = player.position;
            npc.position.X = center.X + (float)(radius * Math.Cos(degrees));
            npc.position.Y = center.Y + (float)(radius * Math.Sin(degrees));

            float faceAngle = (float)Math.Atan2(player.position.Y - npc.position.Y, player.position.X - npc.position.X);
            npc.rotation = faceAngle;
        }
    }
}
