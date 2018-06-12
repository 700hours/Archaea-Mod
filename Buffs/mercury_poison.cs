using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Buffs {
    class mercury_poison : ModBuff {
        public override void SetDefaults() {
            DisplayName.SetDefault("Mercury Poisoning");
            Description.SetDefault("*cool tagline here*");
            //Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            //canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex) {
            player.moveSpeed *= 0.9f;
            player.meleeDamage *= 0.9f;
            player.lifeRegen = 0;
            player.lifeRegenTime = 0;
            if (player.buffTime[buffIndex] % 60 == 0)
                player.statLife--;
        }
    }
}
