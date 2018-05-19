using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace ArchaeaMod.Tiles
{
    public class global_tile : GlobalTile
    {
        public override void RandomUpdate(int i, int j, int type)
        {
        //  maybe the Magno stone takes care of crystal spreading instead
            Tile tile = Main.tile[i, j - 1];
            if(type == mod.TileType<m_stone>() || type == mod.TileType<m_dirt>() || type == mod.TileType<m_ore>())
            {
                int chance = Main.rand.Next(4);
                switch (chance)
                {
                    case 0:
                        if(!Main.tileSolid[tile.type] && !tile.active())
                            WorldGen.PlaceTile(i, j - 1, mod.TileType<c_crystal2x2>(), true, false);
                        break;
                    case 1:
                        if(!Main.tileSolid[tile.type] && !tile.active())
                            WorldGen.PlaceTile(i, j - 1, mod.TileType<c_crystal2x1>(), true, false);
                        break;
                    case 2:
                        if (!Main.tileSolid[Main.tile[i - 1, j].type] && !Main.tile[i - 1, j].active())
                            WorldGen.PlaceTile(i - 1, j, mod.TileType<c_crystalwall>(), true, false);
                        break;
                    case 3:
                        if (!Main.tileSolid[Main.tile[i + 1, j].type] && !Main.tile[i + 1, j].active())
                            WorldGen.PlaceTile(i + 1, j, mod.TileType<c_crystalwall>(), true, false);
                        break;
                }
                //  troubleshooting message
            /*  if (Main.tile[i, j - 1].type == mod.TileType<c_crystal2x2>() || Main.tile[i, j - 1].type == mod.TileType<c_crystal2x1>() ||
                Main.tile[i - 1, j].type == mod.TileType<c_crystalwall>() || Main.tile[i + 1, j].type == mod.TileType<c_crystalwall>())
                    Main.NewText("Crystal spawned? i, " + i + " j, " + j, Color.White); */
            }
        }
        public override bool Slope(int i, int j, int type)
        {
            if (Main.tile[i, j - 1].type == mod.TileType<c_crystalsmall>() ||
                Main.tile[i, j - 1].type == mod.TileType<c_crystal2x1>() ||
                Main.tile[i, j - 1].type == mod.TileType<c_crystal2x2>() ||
                Main.tile[i + 1, j].type == mod.TileType<c_crystalwall>() ||
                Main.tile[i - 1, j].type == mod.TileType<c_crystalwall>() ||
                Main.tile[i, j - 1].type == mod.TileType<m_totem>())
            {
                return false;
            }
            else return true;
        }
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            ArchaeaWorld modWorld = mod.GetModWorld<ArchaeaWorld>();
            Tile tile = Main.tile[i, j];
            if ((type == mod.TileType<m_stone>() || type == mod.TileType<m_ore>() || type == mod.TileType<m_dirt>()) && 
                (Main.tile[i, j - 1].type == mod.TileType<c_crystalsmall>() ||
                Main.tile[i, j - 1].type == mod.TileType<c_crystal2x1>() ||
                Main.tile[i, j - 1].type == mod.TileType<c_crystal2x2>() ||
                Main.tile[i + 1, j].type == mod.TileType<c_crystalwall>() ||
                Main.tile[i - 1, j].type == mod.TileType<c_crystalwall>() ||
                Main.tile[i, j - 1].type == mod.TileType<m_totem>()))
            {
                return false;
            }
            else return true;
        }
        public override bool CanExplode(int i, int j, int type)
        {
            if ((type == mod.TileType<m_stone>() || type == mod.TileType<m_ore>() || type == mod.TileType<m_dirt>()) &&
                (Main.tile[i, j - 1].type == mod.TileType<c_crystalsmall>() ||
                Main.tile[i, j - 1].type == mod.TileType<c_crystal2x1>() ||
                Main.tile[i, j - 1].type == mod.TileType<c_crystal2x2>() ||
                Main.tile[i + 1, j].type == mod.TileType<c_crystalwall>() ||
                Main.tile[i - 1, j].type == mod.TileType<c_crystalwall>() ||
                Main.tile[i, j - 1].type == mod.TileType<m_totem>()))
            {
                return false;
            }
            else return true;
        }
    }
}
