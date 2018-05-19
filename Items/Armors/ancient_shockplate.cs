using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items.Armors
{
    [AutoloadEquip(EquipType.Body)]
    public class ancient_shockplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ancient Shock Breastplate");
        }
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.maxStack = 1;
            item.value = 100;
            item.rare = 3;
            item.defense = 5;
        }

        int Proj1;
        int ticks = 0;
        int d = 0;
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("ancient_shockhelmet") && legs.type == mod.ItemType("ancient_shockgreaves");
        }
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Shock nearby enemies";
            ticks++;
            foreach (NPC n in Main.npc)
            {
                if (n.active && !n.friendly && !n.dontTakeDamage && !n.immortal && n.target == player.whoAmI && n.lifeMax >= 50)
                {
                    if (Collision.CanHitLine(player.position, player.width, player.height, n.position, n.width, n.height) && Vector2.Distance(player.position - n.position + new Vector2(n.width / 2, n.height / 2), Vector2.Zero) < 192f)
                    {
                        if (ticks % 60 == 0 && Main.rand.NextFloat() >= .67f)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/PowerBeamChargeShot"), player.Center);
                            Proj1 = Projectile.NewProjectile(n.position + new Vector2(n.position.X + n.width / 2, n.position.Y + n.height / 2), n.velocity, mod.ProjectileType<shock_spark>(), 10 + Main.rand.Next(-2, 5), 0f, player.whoAmI, n.position.X + n.width / 2, n.position.Y + n.height / 2);
                            Projectile projectile = Main.projectile[Proj1];
                            projectile.position = n.position + new Vector2(n.width / 2, n.height / 2);
                            projectile.timeLeft = 5;
                            for (int i = 0; i < 8; i++)
                            {
                                d = Dust.NewDust(player.position, player.width, player.height, 64, 0f, 0f, 0, Color.White, 2f);
                                Main.dust[d].noGravity = true;
                                Main.dust[d].velocity.X = Main.rand.NextFloat(-4f, 4f);
                                Main.dust[d].velocity.Y = Main.rand.NextFloat(-4f, 4f);
                            }
                        }
                    }
                }
            }
        }
    }
}
