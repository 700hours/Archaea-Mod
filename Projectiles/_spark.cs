using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    class _spark : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 4;
            projectile.scale = 1f;
            projectile.aiStyle = -1;
            projectile.timeLeft = 300;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.ranged = true;
        }

        int degrees;
        const float radians = 0.017f;
        public override void AI()
        {
            projectile.rotation += radians * 11.125f;
            projectile.timeLeft = (int)projectile.ai[0];
        }
    }
}
