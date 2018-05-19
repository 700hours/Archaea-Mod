using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using ReLogic.Graphics;

namespace ArchaeaMod.Tiles
{
    public class m_totem : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSpelunker[Type] = true;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 1200;
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileNoSunLight[Type] = false;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Origin = new Point16(0, 1);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.HookCheck = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.FindEmptyChest), -1, 0, true);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(new Func<int, int, int, int, int, int>(Chest.AfterPlacement_Hook), -1, 0, false);
            TileObjectData.newTile.AnchorInvalidTiles = new int[] { 127 };
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Crimson Totem");
            AddMapEntry(new Color(110, 210, 110), name);
            disableSmartCursor = true;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.604f;
            g = 0.161f;
            b = 0.161f;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            if (progress < 1800) return false;
            else return true;
        }

        bool flag;
        bool active;
        int dustCircle;
        int playerID;
        int progress;
        float degrees;
        float radius = 384f;
        const float radians = 0.017f;
        Color alternateColor = default(Color);
    //  Player player = default(Player);
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 tilev = new Vector2(i * 16, j * 16);
            foreach (Player p in Main.player)
            {
                if (p.active && !p.dead && p != null)
                {
                    if (Vector2.Distance(p.position - tilev, Vector2.Zero) < radius)
                    {
                        active = true;
                        playerID = p.whoAmI;
                        break;
                    }
                    else active = false;
                }
            }
            if (active)
            {
                if (progress < 600) progress++;
                else
                {
                    NPC.SpawnOnPlayer(Main.player[playerID].whoAmI, mod.NPCType("boss_magnohead"));
                    WorldGen.KillTile(i, j, false, false, true);
                }
                
                degrees += radians * 9f;

                int closeDust = Dust.NewDust(tilev, 32, 32, 159, 0f, 0f, 0, default(Color), 1f);
                Main.dust[closeDust].position.X = tilev.X + (float)(radius * Math.Cos(degrees));
                Main.dust[closeDust].position.Y = tilev.Y + (float)(radius * Math.Sin(degrees));

                Main.player[playerID].AddBuff(mod.BuffType("magno_cursed"), 300, false);
            }
            else progress = 0;
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 tilev = new Vector2(i * 16 - 8, j * 16 - 32);

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            if (progress > 0)
            {
                Main.spriteBatch.DrawString(Main.fontMouseText, "" + progress / 6 + "%", tilev - Main.screenPosition + zero, Color.Lerp(Color.Red, Color.Green, progress / 600f));
            }

            float Radius = 8f;
            float Point = (float)(Radius * Math.Cos(degrees));
            float intensity = (Radius - Point) / Radius;
            Color alternateColor = new Color(Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0.8f, 0.361f, 0.361f), intensity));

            Main.spriteBatch.Draw(Main.tileTexture[Type],
                new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
                new Rectangle(0, 0, 16, 16),
                alternateColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);

            return true;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.showItemIcon2 = mod.ItemType("magno_trophy");
            player.showItemIcon = true;
        }
        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
        }
    }
}
