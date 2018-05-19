using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod;
using ArchaeaMod.NPCs;

namespace ArchaeaMod.NPCs
{
    public class m_slime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac Slime");
            Main.npcFrameCount[npc.type] = 3;
            //  to circumvent custom spawning net code
            //  unnecessary for final version
            //  Main.npcCatchable[npc.type] = true;
        }
        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 28;
            npc.friendly = false;
            npc.aiStyle = 1;
            npc.damage = 10;
            npc.defense = 4;
            npc.lifeMax = 80;
            npc.value = 500;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 1f;
        }

        public void Initialize()
        {
            npc.TargetClosest(true);
        }
        bool init;
        int ticks;
        int active = 300;
        public override void AI()
        {
            if (!init)
            {
                Initialize();
                init = true;
            }
            if (npc.life < npc.lifeMax && (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active))
            {
                npc.TargetClosest(true);
            }
            ticks++;
        }
        bool frameAdd = false;
        int num, frame;
        int FrameY;
        public override void FindFrame(int frameHeight)
        {
            if (!Main.dedServ)
                num = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];

            if (npc.velocity.Y == 0)
            {
                if (frameAdd && ticks % 10 == 0 && frame < 2)
                    frame++;
                else if (ticks % 10 == 0 && frame > 0)
                    frame--;
                if (frame == 0) frameAdd = true;
                if (frame == 2) frameAdd = false;
            }
            else frame = 0;
            
            npc.frame.Y = num * frame;
            FrameY = npc.frame.Y;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.Center, ItemID.Gel, 1 + Main.rand.Next(1, 4), true, 0, false, false);

            int choice = Main.rand.Next(6);
            int[] item = new int[2];
            switch (choice)
            {
                case 0:
                    item[0] = mod.ItemType("magno_core");
                    break;
            }
            int vanillaDrop = Main.rand.Next(5000);
            switch (vanillaDrop)
            {
                case 0:
                    item[1] = ItemID.SlimeStaff;
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
        
    /*  public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            ArchaeaPlayer modPlayer = spawnInfo.player.GetModPlayer<ArchaeaPlayer>(mod);

            return !spawnInfo.playerSafe && modPlayer.MagnoZone ? 0.25f : 0f;
        }   */

        float degrees;
        const float radians = 0.017f;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //  if there was going to be an fluctuating glow
            /*  degrees += radians * 4.5f;
                float radius = 8f;
                float Point = (float)(radius * Math.Cos(degrees));
                float intensity = (radius - Point) / radius;
                Color alternateColor = new Color(Vector3.Lerp(new Vector3(1f, 1f, 1f), Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0.8f, 0.361f, 0.361f), intensity), intensity)); */

            spriteBatch.Draw(mod.GetTexture("NPCs/m_slime"),
                new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2 + 4) - Main.screenPosition,
                new Rectangle(0, FrameY, npc.width, npc.height),
                Lighting.GetColor((int)npc.position.X / 16, (int)npc.position.Y / 16), 0f, new Vector2(npc.width / 2, npc.height / 2), 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(mod.GetTexture("Gores/magno_slimeglow"),
                new Vector2(npc.position.X + npc.width / 2, npc.position.Y + npc.height / 2 + 4) - Main.screenPosition, 
                new Rectangle(0, FrameY, npc.width, npc.height),
                new Color(1f, 1f, 1f, 0.8f) * 0.3f, 0f, new Vector2(npc.width / 2, npc.height / 2), 1f, SpriteEffects.None, 0f);
            
            return false;
        }
    }
}
