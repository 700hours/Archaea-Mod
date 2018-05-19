using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArchaeaMod.Projectiles;

namespace ArchaeaMod.Items
{
    public class magno_shieldacc : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magnoliac Projection");
            Tooltip.SetDefault("Casts orbiting"
                + "\nprotective shields");
        }
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 32;
            item.value = 15000;
            item.rare = 9;
            item.expert = true;
            item.accessory = true;
            item.defense = 2;
        }

        public bool shield;
        bool flag;
        int Proj;
        int ticks = 600, maxTime = 600;
        int shieldCount;
        float radius = 96f;
        float degreeS = 67.5f;
        float ProjX, ProjY;
        const float radians = 0.017f;
        int[] shieldID = new int[4];
        Vector2 playerCenter;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ArchaeaPlayer modPlayer = player.GetModPlayer<ArchaeaPlayer>(mod);
            if (Main.netMode != 0)
            {
                MultiPlayerEffects(player, MathHelper.ToRadians(90f));
            }
            else
            {
                SinglePlayerEffects(player, MathHelper.ToRadians(90f));
            }
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = shield;
            writer.Write(flags[0]);
            writer.Write(ticks);
            for (int i = 0; i < shieldID.Length; i++)
            {
                writer.Write(shieldID[i]);
            }
        }
        public override void NetRecieve(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            shield = flags[0];
            ticks = reader.ReadInt32();
            for (int i = 0; i < shieldID.Length; i++)
            {
                shieldID[i] = reader.ReadInt32();
            }
        }

        public void NetCheck(int netID, bool flag, byte projCount, byte ticks)
        {
            if (Main.netMode == netID)
            {
                ModPacket packet = mod.GetPacket();
                packet.Write(flag);
                packet.Write(projCount);
                packet.Write(ticks);
                packet.Send();
            }
        }

        public void MultiPlayerEffects(Player player, float degrees)
        {
            playerCenter = new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2);

            if (Main.myPlayer == player.whoAmI && Main.netMode == 1)
            {
                if (!shield)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        ProjX = playerCenter.X + (float)(radius * Math.Cos(degrees * k));
                        ProjY = playerCenter.Y + (float)(radius * Math.Sin(degrees * k));
                        shieldID[k] = Projectile.NewProjectile(new Vector2(ProjX, ProjY), Vector2.Zero, mod.ProjectileType<m_shield>(), 0, 0f, player.whoAmI, degrees * k, 0f);
                        //  NetCheck(2, shield, (byte)shieldID[k], (byte)ticks);
                    }
                    //  play sound
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/conjure"), player.Center);
                    shield = true;
                }

                if (player.ownedProjectileCounts[mod.ProjectileType<m_shield>()] == 4)
                {
                    for (int l = 1; l < 4; l++)
                    {
                        int shield1 = (int)Math.Round(MathHelper.ToDegrees(Main.projectile[shieldID[l - 1]].rotation), 0);
                        int shield2 = (int)Math.Round(MathHelper.ToDegrees(Main.projectile[shieldID[l]].rotation), 0);
                        if (shield2 >= 260 && shield2 <= 280 || shield2 <= -80 && shield2 >= -100)
                        {
                            if (shield1 >= 260 && shield1 <= 280 || shield1 <= -80 && shield1 >= -100 || shield1 >= 160 && shield1 <= 190)
                            {
                            }
                            else
                            {
                                //  troubleshooting message
                                //  Main.NewText("Rotation 1: " + shield1 + " Rotation 2: " + shield2, 200, 150, 100);
                                shield = false;
                            }
                        }
                    }
                }

                if (player.ownedProjectileCounts[mod.ProjectileType<m_shield>()] < 4)
                {
                    ticks--;
                    if (ticks == 0)
                    {
                        foreach (Projectile p in Main.projectile)
                        {
                            if (p.active && player.whoAmI == p.owner && p.type == mod.ProjectileType<m_shield>())
                            {
                                p.active = false;
                            }
                        }
                        shield = false;
                        ticks = maxTime;
                    }
                }
                else if (player.ownedProjectileCounts[mod.ProjectileType<m_shield>()] == 0)
                {
                    shield = false;
                }
                if (player.ownedProjectileCounts[mod.ProjectileType<m_shield>()] > 4)
                {
                    foreach (Projectile p in Main.projectile)
                    {
                        if (p.active && player.whoAmI == p.owner && p.type == mod.ProjectileType<m_shield>())
                        {
                            p.active = false;
                        }
                    }
                    shield = false;
                    ticks = maxTime;
                }
            }
            if (Main.myPlayer != player.whoAmI && Main.netMode == 1 && !flag)
            {
                if (player.ownedProjectileCounts[mod.ProjectileType<m_shield>()] > 4)
                {
                    foreach (Projectile p in Main.projectile)
                    {
                        if (p.active && player.whoAmI == p.owner && p.type == mod.ProjectileType<m_shield>())
                        {
                            p.active = false;
                        }
                    }
                    shield = false;
                    ticks = maxTime;
                }
                flag = true;
            }
        }

        public void SinglePlayerEffects(Player player, float degrees)
        {
            playerCenter = new Vector2(player.position.X + player.width / 2, player.position.Y + player.height / 2);

            if (!shield)
            {
                for (int k = 0; k < 4; k++)
                {
                    ProjX = playerCenter.X + (float)(radius * Math.Cos(degrees * k));
                    ProjY = playerCenter.Y + (float)(radius * Math.Sin(degrees * k));
                    shieldID[k] = Projectile.NewProjectile(new Vector2(ProjX, ProjY), Vector2.Zero, mod.ProjectileType<m_shield>(), 0, 0f, player.whoAmI, degrees * k, 0f);
                }
                //  play sound
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/conjure"), player.Center);
                shield = true;
            }

            if (player.ownedProjectileCounts[mod.ProjectileType<m_shield>()] == 4)
            {
                for (int l = 1; l < 4; l++)
                {
                    int shield1 = (int)Math.Round(MathHelper.ToDegrees(Main.projectile[shieldID[l - 1]].rotation), 0);
                    int shield2 = (int)Math.Round(MathHelper.ToDegrees(Main.projectile[shieldID[l]].rotation), 0);
                    if (shield2 >= 260 && shield2 <= 280 || shield2 <= -80 && shield2 >= -100)
                    {
                        if (shield1 >= 260 && shield1 <= 280 || shield1 <= -80 && shield1 >= -100 || shield1 >= 160 && shield1 <= 190)
                        {
                        }
                        else
                        {
                            //  troubleshooting message
                            //  Main.NewText("Rotation 1: " + shield1 + " Rotation 2: " + shield2, 200, 150, 100);
                            shield = false;
                        }
                    }
                }
            }

            if (player.whoAmI == Main.myPlayer)
            {
                if (player.ownedProjectileCounts[mod.ProjectileType<m_shield>()] < 4)
                {
                    ticks--;
                    if (ticks == 0)
                    {
                        foreach (Projectile p in Main.projectile)
                        {
                            if (p.active && player.whoAmI == p.owner && p.type == mod.ProjectileType<m_shield>())
                            {
                                p.active = false;
                            }
                        }
                        shield = false;
                        ticks = maxTime;
                    }
                }
                else if (player.ownedProjectileCounts[mod.ProjectileType<m_shield>()] == 0)
                {
                    shield = false;
                }
                if (player.ownedProjectileCounts[mod.ProjectileType<m_shield>()] > 4)
                {
                    foreach (Projectile p in Main.projectile)
                    {
                        if (p.active && player.whoAmI == p.owner && p.type == mod.ProjectileType<m_shield>())
                        {
                            p.active = false;
                        }
                    }
                    shield = false;
                    ticks = maxTime;
                }
            }
        }
    }
}
