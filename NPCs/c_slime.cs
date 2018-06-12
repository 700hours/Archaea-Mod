using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Buffs;

namespace ArchaeaMod.NPCs {
    class c_slime : ModNPC {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Mercury Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[2];
        }

        public override void SetDefaults() {
            npc.width = 32;
            npc.height = 26;
            npc.damage = 40;
            npc.defense = 5;
            npc.lifeMax = 140;
            npc.defense = 18;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 100;
            npc.knockBackResist = 0.7f;
            npc.aiStyle = 1;
            animationType = NPCID.BlueSlime;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            target.AddBuff(mod.BuffType<mercury_poison>(), 299);
        }
    }
}
