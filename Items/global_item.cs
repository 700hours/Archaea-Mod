using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Items
{
    public class global_item : GlobalItem
    {
        public override void PickAmmo(Item item, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
        {
            ArchaeaPlayer modPlayer = player.GetModPlayer<ArchaeaPlayer>(mod);
            if (modPlayer.magnoRanged)
            {
                if (item.type == ItemID.WoodenArrow || item.type == ItemID.EndlessQuiver)
                {
                    type = mod.ProjectileType("cinnabar_arrow");
                }
            }
        }
    }
}
