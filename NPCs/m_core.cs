using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.NPCs
{
    public class m_core : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Core");
        }
        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 30;
            npc.friendly = false;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.color = Color.White;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 285;
            //  custom sounds?
            //  npc.HitSound = SoundID.NPCHit1;
            //  npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }

        public void Initialize()
        {
            npc.Hitbox.Inflate(64, 64);
        }
        bool init;
        public override void AI()
        {
            if(!init)
            {
                Initialize();
                init = true;
            }
            foreach (NPC n in Main.npc)
            {
                if (n.type == npc.type && (!n.active || n.life < 0))
                {
                    npc.life = 0;
                    npc.active = false;
                }
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax += (npc.lifeMax / 5) * numPlayers;
            bossLifeScale = 1f;
        }
        public override bool CheckDead()
        {
            return true;
        }
        public override void DrawEffects(ref Color drawColor)
        {
            drawColor = Color.White;
        }
    }
}
