using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArchaeaMod.Projectiles
{
    public class magno_javelinglow : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 3;
        }

        public override void AI()
        {
            Projectile parent = Main.projectile[(int)projectile.ai[0]];
            projectile.position = parent.position;
            projectile.rotation = parent.rotation;
            projectile.velocity = parent.velocity;
            if(!parent.active)
            {
                projectile.active = false; 
            }
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor = Color.White;
        }
    }
}
