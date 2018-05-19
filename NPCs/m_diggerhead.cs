using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace ArchaeaMod.NPCs
{
    public class m_diggerhead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Hatchling");
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
            npc.damage = 35;
            npc.defense = 6;
            npc.lifeMax = 120;
            npc.value = 2500;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
        }

        int Previous3 = 0, despawn = 180;
        int digger3;
        int[] digger = new int[8];
        bool TailSpawned = false;
        public override void AI()
        {
            #region dig AI
            npc.ai[0]++;
            if (npc.ai[0] <= 1 || npc.ai[0] >= 400)
            {
            //  npc.damage = 35;
            //  Main.npc[digger[num36]].damage = 35;
                if (!TailSpawned)
                {
                    Previous3 = npc.whoAmI;
                    for (int i = 0; i < digger.Length; i++)
                    {
                        if (i != 7)
                        {
                            digger3 = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("m_diggerbody"), npc.whoAmI);
                        }
                        else
                        {
                            digger3 = NPC.NewNPC((int)npc.position.X + (npc.width / 2), (int)npc.position.Y + (npc.height / 2), mod.NPCType("m_diggertail"), npc.whoAmI);
                        }
                        NPC nme = Main.npc[digger3];
                        nme.realLife = npc.whoAmI;
                        nme.ai[2] = (float)npc.whoAmI;
                        nme.ai[1] = (float)Previous3;
                        Main.npc[Previous3].ai[0] = digger3;
                        if (Main.netMode == 2)
                        {
                            NetMessage.SendData(23, -1, -1, null, digger3, 0f, 0f, 0f, 0, 0, 0);
                        }
                        Previous3 = digger3;
                    }
                    TailSpawned = true;
                }
            }
            else if (npc.ai[0] >= 2)
            {
                npc.damage = 30;
            }
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }
            if (!Main.player[npc.target].active || Main.player[npc.target].dead)
                despawn--;
            if (despawn <= 0)
                npc.active = false;
            #endregion
            Player player = Main.player[npc.target];
            if(Vector2.Distance(player.position - npc.position, Vector2.Zero) > 1546f)
            {
                npc.active = false;
            }
        }

        public override void NPCLoot()
        {
            int choice = Main.rand.Next(5);
            int item = default(int);
            switch (choice)
            {
                case 0:
                    item = mod.ItemType("magno_core");
                    break;
            }
            if (item != default(int))
            {
                Item.NewItem(npc.position, item, 1, false, -1, true, false);
            }
        }
    }
}
