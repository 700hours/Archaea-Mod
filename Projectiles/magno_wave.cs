using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    public class magno_wave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magno Wave");
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.scale = 1f;
            projectile.aiStyle = -1;
            projectile.timeLeft = 600;
            projectile.damage = 10;
            projectile.knockBack = 7.5f;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.ownerHitCheck = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.ranged = true;
        }
    }
}
