using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArchaeaMod
{
    public class ArchaeaPlayer : ModPlayer
    {
        public bool magnoMinion;
        public bool magnoShield;
        public bool magnoRanged;
        public bool MagnoZone;
        public bool magnoInvuln;
        bool flag;
        public int[] MPID = new int[255];
        int num;
        int ticks = 1;
        int debuffTime = 300;
        const int hellLayer = 192;
        const int tileSize = 16;
        public override void PostUpdate()
        {
            ArchaeaWorld modWorld = mod.GetModWorld<ArchaeaWorld>();

            #region item checks
            for (int k = 0; k < player.armor.Length; k++)
            {
                if (player.armor[k].type == mod.ItemType<Items.magno_shieldacc>())
                {
                    magnoShield = true;
                    break;
                }
                else magnoShield = false;
            }
            for (int k = 0; k < player.armor.Length; k++)
            {
                if (player.armor[k].type == mod.ItemType<Items.Armors.magnoheadgear>())
                {
                    magnoRanged = true;
                    break;
                }
                else
                {
                    magnoRanged = false;
                }

            }
            #endregion

            if (MagnoZone && !modWorld.MagnoDefeated)
            {
                if(!player.HasBuff(mod.BuffType<Buffs.magno_cursed>()))
                {
                    player.AddBuff(BuffID.WaterCandle, debuffTime, true);
                }
            }

            if (!modWorld.MagnoDefeated)
            {
                if (player.position.Y > (Main.maxTilesY * tileSize) - hellLayer * tileSize) 
                {
                    player.AddBuff(BuffID.OnFire, debuffTime, false);
                    if (!flag)
                    {
                        Main.NewText("Energy from the red crystals gathers here", 210, 110, 110);
                        flag = true;
                    }
                }
            }

        //  if (Main.netMode == 0)
        //  {
        /*      player.statLife = player.statLifeMax;
                if (Main.mouseMiddle)
                {   
                        Main.dayTime = true;
                        Main.time = 12400;
                    //  Main.NewText("Location: i, " + Math.Round(player.position.X / 16, 0) + " j, " + Math.Round(player.position.Y / 16, 0), Color.White);
                    //  Main.dayTime = false;
                    //  Main.time = 12400;
                }   */
        //  }
            if (ticks == 0)
            {
                //int chest = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.Chest, 10, true, -1, true, false);
                //int item = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.EoCShield, 1, true, -1, true, false);
                //  int item = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.EoCShield, 1, true, -1, true, false);
                //int ammoArrows = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.FrostburnArrow, 999, true, -1, true, false);
                int item2 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.RegenerationPotion, 10, true, -1, true, false);
                int item3 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.IronskinPotion, 10, true, -1, true, false);
                int item4 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.SwiftnessPotion, 10, true, -1, true, false);
                //int item5 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.HeartLantern, 1, true, -1, true, false);
                //int item6 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.HealingPotion, 30, true, -1, true, false);
                //int item7 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.Campfire, 1, true, -1, true, false);
                //int item8 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.TeleportationPotion, 30, true, -1, true, false);
                //int item9 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.WormholePotion, 30, true, -1, true, false);
                //int throwing = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), mod.ItemType<Items.cinnabar_dagger>(), 999, true, -1, true, false);
                //int throwing2 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), mod.ItemType<Items.magno_javelin>(), 999, true, -1, true, false);
                //int throwing3 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.Beenade, 250, true, -1, true, false);
                //int throwing4 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.MolotovCocktail, 99, true, -1, true, false);
                //int weapon = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.MagicMissile, 1, true, -1, true, false);
                //int weapon2 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.ThornChakram, 1, true, -1, true, false);
                //int weapon3 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.WaterBolt, 1, true, -1, true, false);
                //int weapon4 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), mod.ItemType<Items.magno_yoyo>(), 1, true, -1, true, false);
                //int weapon5 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.AquaScepter, 1, true, -1, true, false);
                //int weapon6 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), mod.ItemType<Items.magno_book>(), 1, true, -1, true, false);
                //  int item8 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), mod.ItemType<Items.Armors.magnoheadgear>(), 1, true, -1, true, false);
                //  int item9 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), mod.ItemType<Items.Armors.magnoplate>(), 1, true, -1, true, false);
                //  int item10 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), mod.ItemType<Items.Armors.magnogreaves>(), 1, true, -1, true, false);
                //  int item11 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), mod.ItemType<Items.magno_summonstaff>(), 1, true, -1, true, false);
                //int head = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.FossilHelm, 1, true, -1, true, false);
                //int body = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.FossilShirt, 1, true, -1, true, false);
                //int legs = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.FossilPants, 1, true, -1, true, false);
                //int head2 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.JungleHat, 1, true, -1, true, false);
                //int body2 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.JungleShirt, 1, true, -1, true, false);
                //int legs2 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.JunglePants, 1, true, -1, true, false);
                //int head3 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.NecroHelmet, 1, true, -1, true, false);
                //int body3 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.NecroBreastplate, 1, true, -1, true, false);
                //int legs3 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.NecroGreaves, 1, true, -1, true, false);
            /*  int head4 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.SilverHelmet, 1, true, -1, true, false);
                int body4 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.SilverChainmail, 1, true, -1, true, false);
                int legs4 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.SilverGreaves, 1, true, -1, true, false);
                int weapon = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.PlatinumBow, 1, true, -1, true, false);
                int pickaxe = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.SilverPickaxe, 1, true, -1, true, false);
                int axe = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.SilverAxe, 1, true, -1, true, false);
                int hammer = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.SilverHammer, 1, true, -1, true, false);  */
                //  int ammoArrows = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.JestersArrow, 999, true, -1, true, false);
                //  int ammoBullets = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.MusketBall, 999, true, -1, true, false);
                //int acc = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.CloudinaBottle, 1, true, -1, true, false);
                //  int acc2 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.ShoeSpikes, 1, true, -1, true, false);
                //int acc3 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.MagicMirror, 1, true, -1, true, false);
                //  int acc4 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.CobaltShield, 1, true, -1, true, false);
                //  int dummy = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.TargetDummy, 1, true, -1, true, false);
                //int dpsMeter = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.DPSMeter, 1, true, -1, true, false); 
                /*  int ammoArrows2 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.WoodenArrow, 1998, true, -1, true, false);
                    int ammoArrows3 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.JestersArrow, 999, true, -1, true, false);
                    int voodooDolls = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.GuideVoodooDoll, 3, true, -1, true, false);
                    int acc5 = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.LavaCharm, 1, true, -1, true, false);
                    int bridgeBlocks = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.IceBlock, 2997, true, -1, true, false);     */
                //int lifeCrystals = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.LifeCrystal, 10, true, -1, true, false);    
                //int manaCrystals = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.ManaCrystal, 4, true, -1, true, false);    
                //int mount = Item.NewItem(new Rectangle((int)player.position.X, (int)player.position.Y, 0, 0), ItemID.FuzzyCarrot, 1, true, -1, true, false);    
                ticks = 1;
            }
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)player.whoAmI);
            packet.Send(toWho, fromWho);
        }

        public override void UpdateBiomes()
        {
            MagnoZone = (ArchaeaWorld.magnoTiles >= 100);
        }
        public override bool CustomBiomesMatch(Player other)
        {
            ArchaeaPlayer modOther = other.GetModPlayer<ArchaeaPlayer>(mod);
            return MagnoZone == modOther.MagnoZone;
        }
        public override void CopyCustomBiomesTo(Player other)
        {
            ArchaeaPlayer modOther = other.GetModPlayer<ArchaeaPlayer>(mod);
            modOther.MagnoZone = MagnoZone;
        }
        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = MagnoZone;
            writer.Write(flags);
        }
        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            MagnoZone = flags[0];
        }
        public override Texture2D GetMapBackgroundImage()
        {
            if (MagnoZone)
            {
                return mod.GetTexture("Backgrounds/MapBGMagno");
            }
            return null;
        }
    }
}