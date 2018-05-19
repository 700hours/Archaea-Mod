using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    public class magno_book : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Inferno Missile");
            Tooltip.SetDefault("Conjures explosive orb");
        }
        public override void SetDefaults()
        {
            item.width = 62;
            item.height = 24;
            item.scale = 1f;
            item.useTime = 20;
            item.useAnimation = 20;
            item.damage = 15;
            item.mana = 8;
            item.useStyle = 1;
            item.value = 2500;
            item.rare = 2;
            //  custom sound?
            //  item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/*");
            item.autoReuse = false;
            item.consumable = false;
            item.noMelee = true;
            item.magic = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.ownedProjectileCounts[mod.ProjectileType("magno_orb")] < 1)
            {
                return true;
            }
            else return false;
        }
        int Proj1;
        public override bool UseItem(Player player)
        {
            Main.PlaySound(2, player.Center, 20);
            Proj1 = Projectile.NewProjectile(player.position + new Vector2(player.width / 2, player.height / 2), Vector2.Zero, mod.ProjectileType("magno_orb"), (int)(15 * player.magicDamage), 4f, player.whoAmI, 0f, 0f);
            Main.projectile[Proj1].netUpdate = true;
        //  duplicates projectiles
        //  if (Main.netMode == 1) NetMessage.SendData(27, -1, -1, null, Proj1);
            return true;
        }
    }
}
