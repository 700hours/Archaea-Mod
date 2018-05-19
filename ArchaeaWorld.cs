using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace ArchaeaMod
{
    public class ArchaeaWorld : ModWorld
    {
        public override void PostDrawTiles()
        {
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (!Main.playerInventory)
            {
                //  Main.spriteBatch.DrawString(Main.fontMouseText, "Test Environment v0.3.33", new Vector2(192, 160), Color.White, 0f, new Vector2(8, 8) + Main.fontMouseText.MeasureString("|-0-0-0-0-0-0-|"), 1f, SpriteEffects.None, 0f);
                //  Main.spriteBatch.DrawString(Main.fontMouseText, "Most recent: 2 Walls and Totem", new Vector2(192, 192), Color.White, 0f, new Vector2(8, 8) + Main.fontMouseText.MeasureString("|-0-0-0-0-0-0-|"), 1f, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
        }

        // check is made in PostUpdate()
    /*  public override void Initialize()
        {
            if (MagnoDefeated) CinnabarSpawned = true;
        }   */

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (MagnoDefeated) downed.Add("magno");
            if (CinnabarSpawned) downed.Add("msg");

            return new TagCompound
            {
                { "downed", downed },
                { "msg", downed }
            };
        }
        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            MagnoDefeated = downed.Contains("magno");
            CinnabarSpawned = downed.Contains("msg");
        }
        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadByte();
            if (loadVersion == 0)
            {
                BitsByte flags = reader.ReadByte();
                MagnoDefeated = flags[0];
                CinnabarSpawned = flags[1];
            }
            else
            {
                ErrorLogger.Log("ArchaeaMod Mod: Unknown loadVersion: " + loadVersion);
            }
        }
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = MagnoDefeated;
            flags[1] = CinnabarSpawned;
            writer.Write(flags);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            MagnoDefeated = flags[0];
            CinnabarSpawned = flags[1];
        }

        public override void ResetNearbyTileEffects()
        {
            magnoTiles = 0;
            magnoTotem = 0;
        }
        public override void TileCountsAvailable(int[] tileCounts)
        {
            magnoTiles = tileCounts[modb.TileType<Tiles.m_dirt>()] + tileCounts[modb.TileType<Tiles.m_stone>()] + tileCounts[modb.TileType<Tiles.m_ore>()];
            magnoTotem = tileCounts[modb.TileType<Tiles.m_totem>()];
        }

        public static int magnoTiles;
        public int magnoTotem;
        public bool MagnoDefeated = false;
        public bool CinnabarSpawned = false;
        public static Miner miner = new Miner();
    //  public static SkyTower tower = new SkyTower();
        int buffer = 16, wellBuffer = 96, surfaceBuffer;
        int ID = 0;
        int ticks = 0;
        int[] HouseX = new int[216000];
        int[] HouseY = new int[216000];
        const int TileSize = 16;
        float PositionX;
        public Mod modb = ModLoader.GetMod("ArchaeaMod");
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int CavesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Granite")); // Granite
            if (CavesIndex != -1)
            {
                tasks.Insert(CavesIndex + 1, new PassLegacy("Miner " + Miner.progressText, delegate (GenerationProgress progress)
                {
                    miner.active = true;
                    miner.Reset();
                    for (int i = 0; miner.jobCount < miner.jobCountMax + miner.buffer; i++)
                    {
                        if (miner.active)
                        {
                            miner.Update();
                        }
                        else miner.jobCount = miner.jobCountMax + miner.buffer;

                        progress.Message = "Miner " + Miner.progressText;
                    }
                }));
            }

        /*  int TowerIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Mud Caves To Grass"));
            if (TowerIndex != -1)
            {
                tasks.Insert(TowerIndex + 1, new PassLegacy("Generate Sky Tower", delegate (GenerationProgress progress)
                {
                    tower.Reset();
                    if (tower.active)
                    {
                        tower.Update();
                    }

                    progress.Message = "Generate Sky Tower";
                }));
            }   */

            int OresIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (OresIndex != -1)
            {
                tasks.Insert(OresIndex + 1, new PassLegacy("ArchaeaMod Mod Shinies", delegate (GenerationProgress progress)
                {
                    progress.Message = "ArchaeaMod Mod Shinies";

                    for (int k = 0; k < (int)((4200 * 1200) * 6E-05); k++)
                    {
                        WorldGen.TileRunner(WorldGen.genRand.Next((int)(miner.genPos[0].X / 16) - miner.edge / 2, (int)(miner.genPos[1].X / 16) + miner.edge / 2), WorldGen.genRand.Next((int)miner.genPos[0].Y / 16 - miner.edge / 2, (int)miner.genPos[1].Y / 16 + miner.edge / 2), WorldGen.genRand.Next(15, 18), WorldGen.genRand.Next(2, 6), modb.TileType<Tiles.m_dirt>(), false, 0f, 0f, false, true);
                        WorldGen.TileRunner(WorldGen.genRand.Next((int)(miner.genPos[0].X / 16) - miner.edge / 2, (int)(miner.genPos[1].X / 16) + miner.edge / 2), WorldGen.genRand.Next((int)miner.genPos[0].Y / 16 - miner.edge / 2, (int)miner.genPos[1].Y / 16 + miner.edge / 2), WorldGen.genRand.Next(9, 12), WorldGen.genRand.Next(2, 6), modb.TileType<Tiles.m_ore>(), false, 0f, 0f, false, true);
                    }
                }));
            }

            int CrystalsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Lakes"));
            if (CrystalsIndex != -1)
            {
                tasks.Insert(CrystalsIndex + 1, new PassLegacy("Crystals", delegate (GenerationProgress progress)
                {
                    progress.Message = "Crystals";

                    int x = 0;
                    int y = 0;
                    for (int k = 0; k < (int)((4200 * 1200) * 6E-05); k++)
                    {
                        #region crystal runners
                        bool success2x2 = false;
                        Tile tile2x2;
                        int placeTile2x2 = modb.TileType<Tiles.c_crystal2x2>();
                        int attempts2x2 = 0;
                        int maxAttempts2x2 = 64;
                        while (!success2x2)
                        {
                            attempts2x2++;
                            x = WorldGen.genRand.Next((int)miner.genPos[0].X / 16, (int)miner.genPos[1].X / 16);
                            y = WorldGen.genRand.Next((int)miner.genPos[0].Y / 16, (int)miner.genPos[1].Y / 16);
                            tile2x2 = Main.tile[x, y];
                            if (tile2x2.wall == 0)
                            {
                                WorldGen.PlaceTile(x, y, placeTile2x2);
                            }
                            success2x2 = tile2x2.active() && tile2x2.type == placeTile2x2;
                        }

                        bool success2x1 = false;
                        Tile tile2x1;
                        int placeTile2x1 = modb.TileType<Tiles.c_crystal2x1>();
                        int attempts2x1 = 0;
                        int maxAttempts2x1 = 64;
                        while (!success2x1)
                        {
                            attempts2x1++;
                            x = WorldGen.genRand.Next((int)miner.genPos[0].X / 16, (int)miner.genPos[1].X / 16);
                            y = WorldGen.genRand.Next((int)miner.genPos[0].Y / 16, (int)miner.genPos[1].Y / 16);
                            tile2x1 = Main.tile[x, y];
                            if (tile2x1.wall == 0)
                            {
                                WorldGen.PlaceTile(x, y, placeTile2x1);
                            }
                            success2x1 = tile2x1.active() && tile2x1.type == placeTile2x1;
                        }

                        bool successSmall = false;
                        Tile tileSmall;
                        Tile tile2;
                        int placeTileSmall = modb.TileType<Tiles.c_crystalsmall>();
                        int placeTileWall = modb.TileType<Tiles.c_crystalwall>();
                        int attemptsSmall = 0;
                        int maxAttemptsSmall = 194;
                        while (!successSmall)
                        {
                            attemptsSmall++;
                            x = WorldGen.genRand.Next((int)miner.genPos[0].X / 16, (int)miner.genPos[1].X / 16);
                            y = WorldGen.genRand.Next((int)miner.genPos[0].Y / 16, (int)miner.genPos[1].Y / 16);
                            tileSmall = Main.tile[x, y];
                            bool tileCheck = tileSmall.wall == 0; // WallID.Granite || tileSmall.wall == WallID.GraniteBlock || tileSmall.wall == WallID.GraniteUnsafe || tileSmall.wall == WallID.Marble || tileSmall.wall == WallID.MarbleBlock || tileSmall.wall == WallID.MarbleUnsafe || tileSmall.wall == WallID.SpiderUnsafe;
                            if (tileSmall.wall == 0)
                            {
                                WorldGen.PlaceTile(x, y, placeTileWall);
                            }
                            successSmall = tileSmall.active() && (tileSmall.type == placeTileSmall || tileSmall.type == placeTileWall); //(Main.tile[x, y + 1].type == placeTileSmall || Main.tile[x, y - 1].type == placeTileSmall || Main.tile[x + 1, y].type == placeTileSmall || Main.tile[x - 1, y].type == placeTileSmall);
                        }
                        #endregion
                    }

                /*  bool successTotem = false;
                    Tile tileTotem;
                    int placeTotem = miner.mod.TileType<Tiles.m_totem>();
                    while (!successTotem)
                    {
                        x = WorldGen.genRand.Next((int)miner.genPos[0].X / 16, (int)miner.genPos[1].X / 16);
                        y = WorldGen.genRand.Next((int)miner.genPos[0].Y / 16, (int)miner.genPos[1].Y / 16);
                        tileTotem = Main.tile[x, y];
                        if (tileTotem.wall == 0 && Main.tile[x, y + 1].type == (ushort)mod.TileType<Tiles.m_stone>())
                        {
                            WorldGen.PlaceTile(x, y, placeTotem);
                        }
                        successTotem = tileTotem.type == placeTotem;
                    }   */
                }));
            }

            int HouseIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Clean Up Dirt"));
            if (HouseIndex != -1)
            {
                tasks.Insert(HouseIndex + 1, new PassLegacy("Magnoliac Houses", delegate (GenerationProgress progress)
                {
                    progress.Message = "Magnoliac Houses";

                    Reset();
                    int x = 0;
                    int y = 0;
                    int inflate = (int)Vector2.Distance(miner.genPos[0] - miner.genPos[1], Vector2.Zero);
                    if (inflate < 0) inflate *= (-1);
                    HouseX[0] = (int)miner.center.X;
                    HouseY[0] = (int)miner.center.Y;
                    for (int i = (int)miner.genPos[0].X / 16; i < (int)miner.genPos[1].X / 16; i++)
                    {
                        x = i;
                        for (int j = (int)miner.genPos[0].Y / 16; j < (int)miner.genPos[1].Y / 16; j++)
                        {
                            y = j;
                            if (Main.tile[x, y].active())
                            {
                                ID++;
                                HouseX[ID] = x;
                                HouseY[ID] = y;
                            }
                        }
                    }

                    for (int k = 0; k < 20600; k++)
                    {
                        for (int m = HouseY[k] - buffer * 2; m < HouseY[k]; m++)
                        {
                            ticks++;
                            if (HouseX[k] != 0 && ticks % 6400 == 0) // previous: 4800
                            {
                                if (!Main.tile[HouseX[k] + buffer / 2, m - 1].active() && (Main.tile[HouseX[k], HouseY[k]].type == modb.TileType<Tiles.m_stone>() || Main.tile[HouseX[k], HouseY[k]].type == modb.TileType<Tiles.m_ore>() || Main.tile[HouseX[k], HouseY[k]].type == modb.TileType<Tiles.m_dirt>()))
                                {
                                    if (Main.tile[HouseX[k], HouseY[k]].wall != WallID.ObsidianBrick)
                                    {
                                        PlaceHouse(HouseX[k], HouseY[k]);
                                    }
                                }
                            }
                        }
                    }

                    for (int i = (int)miner.genPos[0].X / 16; i < (int)miner.genPos[1].X / 16; i++)
                    {
                        x = i;
                        for (int j = (int)miner.genPos[0].Y / 16; j < (int)miner.genPos[1].Y / 16; j++)
                        {
                            y = j;
                            if (/*Main.tile[x, y + 1].active() && Main.tile[x + 1, y + 1].active() && */Main.tile[x, y].wall == (ushort)modb.WallType<Walls.magno_brick>() || Main.tile[x, y].wall == (ushort)modb.WallType<Walls.magno_stone>())
                            {
                                int Randomizer = WorldGen.genRand.Next(64);
                                if (Randomizer <= 2)
                                {
                                    WorldGen.PlaceTile(x, y, modb.TileType<Tiles.m_chest>(), true, false, -1, 0);
                                }
                                if (Randomizer >= 3 && Randomizer <= 8)
                                {
                                    int num = WorldGen.genRand.Next(2, WorldGen.statueList.Length);
                                    WorldGen.PlaceTile(x, y, WorldGen.statueList[num].X, true, true, -1, WorldGen.statueList[num].Y);
                                }
                                if (Randomizer == 9)
                                {
                                    WorldGen.PlaceTile(x, y, TileID.Tables, true, false, -1, 0);
                                    //  need to change frame to obsidian table
                                    //  Main.tile[x, y].frameX
                                }
                            }
                        }
                    }

                //  GenerateGrinder((int)miner.minerPos.X / 16, (int)miner.baseCenter.Y / 16);
                }));
            }

            int WellIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Larva"));
            if (WellIndex != -1)
            {
                tasks.Insert(WellIndex + 1, new PassLegacy("Digging Well", delegate (GenerationProgress progress)
                {
                    progress.Message = "Digging Well";
                    Vector2 Center = new Vector2((Main.maxTilesX / 2) * 16, (Main.maxTilesY / 2) * 16);
                    if ((miner.genPos[0].X + wellBuffer / 3) / TileSize > miner.baseCenter.X / TileSize)
                    {
                        PositionX = miner.genPos[1].X / 16;
                    }
                    else PositionX = miner.genPos[0].X / 16;

                    int gap = 5;
                    int MaxTries = 128;
                    bool dirtWall = Main.tile[(int)PositionX, Main.spawnTileY - surfaceBuffer].wall == WallID.DirtUnsafe || Main.tile[(int)PositionX, Main.spawnTileY - surfaceBuffer].wall == WallID.DirtUnsafe1 || Main.tile[(int)PositionX, Main.spawnTileY - surfaceBuffer].wall == WallID.DirtUnsafe2 || Main.tile[(int)PositionX, Main.spawnTileY - surfaceBuffer].wall == WallID.DirtUnsafe3 || Main.tile[(int)PositionX, Main.spawnTileY - surfaceBuffer].wall == WallID.DirtUnsafe4;
                    for (int i = 0; i < MaxTries; i++)
                    {
                        if (Main.tile[(int)PositionX + gap, Main.spawnTileY - surfaceBuffer].active() && Main.tile[(int)PositionX + gap, Main.spawnTileY - surfaceBuffer].wall != 0)
                        {
                            surfaceBuffer++;
                        }
                        if (!Main.tile[(int)PositionX + gap, Main.spawnTileY - surfaceBuffer].active() && Main.tile[(int)PositionX + gap, Main.spawnTileY - surfaceBuffer].wall == 0)
                        {
                            surfaceBuffer--;
                        }
                    }

                    int buffer = 3;
                    float distance = Vector2.Distance(new Vector2(PositionX, Main.spawnTileY + 10 - surfaceBuffer) - new Vector2(PositionX, miner.genPos[1].Y / 16), Vector2.Zero);
                    // comment out '/ 3' for max well length
                    PlaceWell((int)PositionX, Main.spawnTileY - surfaceBuffer - buffer, distance / 3);
                }));
            }
        }

        int HouseWidth = 20;
        int HouseHeight = 8;
        int columnID = 0;
        int rowID = 0;
        int[] frame = new int[3];
        int[] column;
        int[] row;
        int[,] houseShape;
        public bool PlaceHouse(int i, int j)
        {
            HouseWidth = 24 + WorldGen.genRand.Next(-6, 4);
            HouseHeight = 12 + WorldGen.genRand.Next(-2, 2);
            //  different house generation based on stored values
            //  column = new int[HouseWidth];
            //  row = new int[HouseHeight];
            houseShape = new int[HouseHeight, HouseWidth];
            for (int k = 0; k < houseShape.GetLength(0); k++)
            {
                //  different house generation based on stored values
                //  column[columnID] = k;
                houseShape[k, 0] = 1;
                houseShape[k, HouseWidth - 1] = 1;
                //  columnID++;
            }
            for (int l = 0; l < houseShape.GetLength(1); l++)
            {
                //  different house generation based on stored values
                //  row[rowID] = l;
                houseShape[0, l] = 1;
                houseShape[HouseHeight - 1, l] = 1;
                if (l > 4 && l < 9 + WorldGen.genRand.Next(-1, 3))
                {
                    houseShape[0, l] = 2;
                }
                if (l > 0 && l < 9 + WorldGen.genRand.Next(-1, 4))
                {
                    houseShape[5, l] = 3;
                }
                //  rowID++;
            }
            for (int y = 0; y < houseShape.GetLength(0); y++)
            {
                for (int x = 0; x < houseShape.GetLength(1); x++)
                {
                    int m = i + x;
                    int n = j + y;
                    if (WorldGen.InWorld(m, n, 30))
                    {
                        Tile tile = Framing.GetTileSafely(m, n);
                        switch (houseShape[y, x])
                        {
                            case 0:
                                tile.type = TileID.Dirt;
                                tile.active(false);
                                int RandomWall = WorldGen.genRand.Next(6);
                                if (RandomWall == 0) tile.wall = (ushort)modb.WallType<Walls.magno_stone>();
                                else tile.wall = (ushort)modb.WallType<Walls.magno_brick>();
                                break;
                            case 1:
                                tile.type = (ushort)modb.TileType<Tiles.m_brick>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                for (int o = 0; o < 3; o++)
                                {
                                    frame[o] = (18 * WorldGen.genRand.Next(9, 13));
                                    tile.frameY = (short)frame[o];
                                }
                                break;
                            case 3:
                                tile.type = TileID.Platforms;
                                tile.active(true);
                                tile.wall = (ushort)modb.WallType<Walls.magno_brick>();
                                for (int o = 0; o < 3; o++)
                                {
                                    frame[o] = (18 * WorldGen.genRand.Next(9, 13));
                                    tile.frameY = (short)frame[o];
                                }
                                Main.tile[m, n - 2].active(false);
                                Main.tile[m, n - 3].active(false);
                                Main.tile[m, n - 4].active(false);
                                WorldGen.PlaceTile(m, n - 1, modb.TileType<Tiles.m_book>(), true, false, -1, 0);
                                Main.tile[m, n - 1].frameX = (short)(18 * Main.rand.Next(0, 4));
                                if (WorldGen.genRand.Next(25) == 0 && Main.tile[m, n - 1].type == modb.TileType<Tiles.m_book>())
                                {
                                    Main.tile[m, n - 1].frameX = 90;
                                }
                                break;
                        }
                    }
                    WorldGen.SquareTileFrame(m, n, true);
                    WorldGen.SquareWallFrame(m, n, true);
                }
            }
            return true;
        }

        int[,] wellShape = new int[,]
        {
            { 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0 },
            { 0, 0, 5, 2, 4, 0, 5, 2, 4, 0, 0 },
            { 0, 0, 0, 6, 0, 0, 0, 6, 0, 0, 0 },
            { 0, 0, 5, 2, 2, 2, 2, 2, 4, 0, 0 },
            { 0, 0, 0, 6, 0, 3, 0, 6, 0, 0, 0 },
            { 0, 0, 0, 6, 0, 3, 0, 6, 0, 0, 0 },
            { 0, 0, 0, 6, 0, 3, 0, 6, 0, 0, 0 },
            { 0, 0, 0, 6, 0, 3, 0, 6, 0, 0, 0 },
            { 0, 0, 0, 1, 0, 3, 0, 1, 0, 0, 0 },
            { 0, 0, 1, 1, 0, 3, 0, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 0, 3, 0, 1, 1, 1, 0 },
            { 7, 7, 7, 1, 0, 3, 0, 1, 7, 7, 7 }
        };
        int[,] wellShapeWall = new int[,]
        {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 }
        };
        public bool PlaceWell(int i, int j, float length)
        {
            for (int m = 0; m < (int)length; m++)
            {
                for (int n = -2; n < 3; n++)
                {
                    WorldGen.KillTile(i + n, j + m, false, false, true);
                    WorldGen.KillTile(i + n, j + m, false, false, true);
                    if (n == -2 || n == 2)
                    {
                        WorldGen.PlaceTile(i + n, j + m, modb.TileType<Tiles.m_brick>(), true, true);
                    }
                    if (n > -2 && n < 2)
                    {
                        WorldGen.PlaceWall(i + n, j + m, modb.WallType<Walls.magno_brick>(), true);
                        Main.tile[i + n, j + m].wall = (ushort)modb.WallType<Walls.magno_brick>();
                    }
                }
                WorldGen.PlaceTile(i, j + m, TileID.Rope);
            }
            for (int y = 0; y < wellShape.GetLength(0); y++)
            {
                for (int x = 0; x < wellShape.GetLength(1); x++)
                {
                    int k = i - 5 + x;
                    int l = j - 8 + y;
                    if (WorldGen.InWorld(k, l, 30))
                    {
                        Tile tile = Framing.GetTileSafely(k, l);
                        switch (wellShape[y, x])
                        {
                            case 1:
                                tile.type = (ushort)modb.TileType<Tiles.m_brick>();
                                tile.active(true);
                                tile.slope(0);
                                break;
                            case 2:
                                if (WorldGen.crimson)
                                    tile.type = TileID.RedDynastyShingles;
                                else tile.type = TileID.BlueDynastyShingles;
                                tile.active(true);
                                tile.slope(0);
                                break;
                            case 3:
                                tile.type = TileID.Rope;
                                tile.active(true);
                                break;
                            case 4:
                                if (WorldGen.crimson)
                                    tile.type = TileID.RedDynastyShingles;
                                else tile.type = TileID.BlueDynastyShingles;
                                tile.active(true);
                                tile.slope(3);
                                break;
                            case 5:
                                if (WorldGen.crimson)
                                    tile.type = TileID.RedDynastyShingles;
                                else tile.type = TileID.BlueDynastyShingles;
                                tile.active(true);
                                tile.slope(4);
                                break;
                            case 6:
                                tile.type = TileID.WoodenBeam;
                                tile.active(true);
                                break;
                            case 7:
                                tile.type = (ushort)modb.TileType<Tiles.m_brick>();
                                tile.active(true);
                                tile.slope(0);
                                for (int o = 0; o < 6; o++)
                                {
                                    Tile tileY = Main.tile[k, l + o];
                                    tileY.type = (ushort)modb.TileType<Tiles.m_brick>();
                                    tileY.active(true);
                                    tileY.slope(0);
                                }
                                break;
                        }
                        switch (wellShapeWall[y, x])
                        {
                            case 1:
                                tile.wall = WallID.Planked;
                                break;
                        }
                    }
                }
            }
            return true;
        }

        int chamberWidth;
        int chamberHeight;
        int offset, offsetX, offsetY;
        int oldNum, iterate = 32;
        int[,] chamber;
        public bool GenerateGrinder(int i, int j)
        {
            chamberWidth = 128;
            chamberHeight = 96;
            offset = 32;
            offsetX = 0;
            offsetY = 32;
            chamber = new int[chamberHeight, chamberWidth];
            // down (sides)
            for (int k = 0; k < chamber.GetLength(0); k++)
            {
                while (oldNum < iterate)
                {
                    for (int m = 0; m < offset; m++)
                    {
                        chamber[m, oldNum] = -1;
                        if (m == offset - 1)
                        {
                            offset--;
                            oldNum++;
                        }
                    }
                    for (int n = chamberHeight - 1; n > chamberHeight - oldNum; n--)
                    {
                        chamber[n, chamberWidth - offset - 1] = -1;
                    }
                    if (oldNum == iterate) break;
                }
                if (k > 32) chamber[k, 0] = 1;
                if (k < chamberHeight - 32) chamber[k, chamberWidth - 1] = 1;
            }
            // left (cieling and floor)
            for (int l = 0; l < chamber.GetLength(1); l++)
            {
                offsetX++;
                if(offsetX > 32) chamber[0, l] = 1;
                if(l < chamberWidth - 32) chamber[chamberHeight - 1, l] = 1;
            }
            for (int y = 0; y < chamber.GetLength(0); y++)
            {
                for (int x = 0; x < chamber.GetLength(1); x++)
                {
                    int m = i + x;
                    int n = j + y;
                    if (WorldGen.InWorld(m, n, 30))
                    {
                        Tile tile = Framing.GetTileSafely(m, n);
                        switch (chamber[y, x])
                        {
                            case 0:
                                tile.type = (ushort)modb.TileType<Tiles.m_brick>();
                                tile.active(true);
                                break;
                            case 1:
                                tile.type = (ushort)modb.TileType<Tiles.m_brick>();
                                tile.active(true);
                                break;
                            case 2:
                                tile.type = 0;
                                tile.active(false);
                                break;
                        }
                    }
                }
            }

            return true;
        }

        public void Reset()
        {
            ticks = 0;
            for (int m = 0; m < HouseX.Length; m++)
            {
                HouseX[m] = default(int);
            }
            for (int n = 0; n < HouseY.Length; n++)
            {
                HouseX[n] = default(int);
            }
        }
        private bool Gemmable(int type)
        {
            return type == 0 || type == 1 || type == 40 || type == 59 || type == 60 || type == 70 || type == 147 || type == 161 || type == modb.TileType<Tiles.m_stone>();
        }
        int[][] items = 
        {
            new int[] { miner.moda.ItemType<Items.magno_yoyo>(), miner.moda.ItemType<Items.Summons.boss_summon>(), miner.moda.ItemType<Items.Summons.boss_summon>() },
            new int[] { miner.moda.ItemType<Items.magno_javelin>(), miner.moda.ItemType<Items.cinnabar_dagger>(), miner.moda.ItemType<Items.magno_bar>() },
            new int[] { ItemID.ShoeSpikes, ItemID.ClimbingClaws, ItemID.BandofRegeneration },
            new int[] { ItemID.FlamingArrow, ItemID.WoodenArrow, ItemID.JestersArrow },
            new int[] { ItemID.RecallPotion, ItemID.SwiftnessPotion, ItemID.RegenerationPotion },
            new int[] { ItemID.GoldCoin, ItemID.SilverCoin, ItemID.CopperCoin }
        };
        public override void PostWorldGen()
        {
            int num = 0;
            int choice = 0;
            for (int index = 0; index < 1000; index++)
            {
                Chest chest = Main.chest[index];
                if (chest != null && Main.tile[chest.x, chest.y].type == modb.TileType<Tiles.m_chest>())
                {
                    for (int contents = 0; contents < 6; contents++)
                    {
                        choice = WorldGen.genRand.Next(3);
                        chest.item[contents].SetDefaults(items[contents][choice]);
                        #region item stacks
                        if (chest.item[contents].type == modb.ItemType<Items.magno_javelin>() || chest.item[contents].type == modb.ItemType<Items.cinnabar_dagger>() || chest.item[contents].type == ItemID.WoodenArrow || chest.item[contents].type == ItemID.FlamingArrow || chest.item[contents].type == ItemID.JestersArrow)
                        {
                            chest.item[contents].stack = WorldGen.genRand.Next(15, 25);
                        }
                        if (chest.item[contents].type == modb.ItemType<Items.magno_bar>())
                        {
                            chest.item[contents].stack = WorldGen.genRand.Next(5, 8);
                        }
                        if (chest.item[contents].type == ItemID.RecallPotion || chest.item[contents].type == ItemID.SwiftnessPotion || chest.item[contents].type == ItemID.RegenerationPotion || chest.item[contents].type == ItemID.GoldCoin)
                        {
                            chest.item[contents].stack = WorldGen.genRand.Next(1, 3);
                        }
                        #endregion
                    }
                }
            }
        }

        public override void PostUpdate()
        {
            if (MagnoDefeated && !CinnabarSpawned)
            {
                // antiquated cinnabar ore spawn
            /*  for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 6E-05); k++)
                {
                    WorldGen.TileRunner(WorldGen.genRand.Next((int)(miner.genPos[0].X / 16) - miner.edge / 2, (int)(miner.genPos[1].X / 16) + miner.edge / 2), WorldGen.genRand.Next((int)miner.genPos[0].Y / 16 - miner.edge / 2, (int)miner.genPos[1].Y / 16 + miner.edge / 2), WorldGen.genRand.Next(15, 18), WorldGen.genRand.Next(2, 6), miner.mod.TileType<Tiles.c_ore>(), false, 0f, 0f, false, true);
                }   */
                if (Main.netMode == 2)
                {
                    Color color = new Color(210, 110, 110);
                    NetMessage.BroadcastChatMessage(NetworkText.FromKey("The blood red crystal's curse has been lifted"), color);
                }
                if (Main.netMode == 0)
                {
                    Color color = new Color(210, 110, 110);
                    Main.NewText("The blood red crystal's curse has been lifted", color);
                }
                CinnabarSpawned = true;
            }
        }

        public static void DrawChain(Vector2 start, Vector2 end, Texture2D texture, SpriteBatch spriteBatch)
        {
            start -= Main.screenPosition;
            end -= Main.screenPosition;

            int linklength = texture.Height;
            Vector2 chain = end - start;

            float length = (float)chain.Length();
            int numlinks = (int)Math.Ceiling(length / linklength);
            Vector2[] links = new Vector2[numlinks];
            float rotation = (float)Math.Atan2(chain.Y, chain.X);

            for (int i = 0; i < numlinks; i++)
            {
                links[i] = start + chain / numlinks * i;
            }

            for (int i = 0; i < numlinks; i++)
            {
                Color color = Lighting.GetColor((int)((links[i].X + Main.screenPosition.X) / 16), (int)((links[i].Y + Main.screenPosition.Y) / 16));
                spriteBatch.Draw(texture,
                new Rectangle((int)links[i].X, (int)links[i].Y, texture.Width, linklength), null,
                color, rotation + 1.57f, new Vector2(texture.Width / 2f, linklength), SpriteEffects.None, 1f);
            }
        }
    }
    public class Miner : ModWorld
    {
        public Mod moda = ModLoader.GetMod("ArchaeaMod");
        public static string progressText = "";
        static int numMiners = 0, randomX, randomY, bottomBounds = Main.maxTilesY, rightBounds = Main.maxTilesX, circumference, ticks;
        public int edge = 128;
        float mineBlockX = 256, mineBlockY = 256;
        float RightBounds;
        static bool runner = false, grassRunner = false, fillerRunner = false, russianRoulette = false;
        public Vector2 center = new Vector2((Main.maxTilesX / 2) * 16, (Main.maxTilesY / 2) * 16);
        public int buffer = 1, offset = 200;
        int whoAmI = 0, type = 0;
        int XOffset = 512, YOffset = 384;
        public int jobCount = 0;
        public int jobCountMax = 32;
        static int moveID, lookFurther, size = 1;
        public Vector2 minerPos;
        public Vector2 finalVector;
        static Vector2 oldMinerPos, deadZone = Vector2.Zero;
        Vector2 position;
        Vector2 mineBlock;
        public Vector2 baseCenter;
        bool init = false;
        bool fail;
        bool switchMode = false;
        public bool active = true;
        public Vector2[] genPos = new Vector2[2];
        Vector2[] minePath = new Vector2[800*800];
    //  for loop takes care of need to generate new miners
    //  Miner[] ID = new Miner[400];
        public void Init()
        {
            if (whoAmI == 0)
            {
            //  remove these comments for public version
                float offset = XOffset * WorldGen.genRand.Next(-1, 1);
                if(offset == 0)
                {
                    offset = XOffset;
                } 
                minerPos = center + new Vector2(offset * 16f, Main.maxTilesY - YOffset);
                center = minerPos;
                baseCenter = minerPos;
            }
            else
            {
                int RandomX = WorldGen.genRand.Next(-2, 2);
                int RandomY = WorldGen.genRand.Next(-2, 2);
                if (RandomX != 0 && RandomY != 0)
                {
                    mineBlock = new Vector2(mineBlockX * RandomX, mineBlockY * RandomY);
                    minerPos += mineBlock;
                }
                else
                {
                    mineBlock = new Vector2(mineBlockX, mineBlockY);
                    minerPos += mineBlock;
                    return;
                }
            }
            minePath[0] = center;
            init = true;
        //  Main.spawnTileX = (int)center.X / 16;
        //  Main.spawnTileY = (int)center.Y / 16;
            progressText = jobCount + " initiated, " + Math.Round((double)((float)jobCount / jobCountMax) * 10, 0) + "%";
        }
        public void Update()
        {
            if (!init) Init();
            if (init && whoAmI == 0)
            {
                for (int k = 0; whoAmI < 800; k++)
                {
                    Mine();
                }
            }
            else if (whoAmI > 0 && whoAmI <= 800)
            {
                for (int k = 0; whoAmI < 800; k++)
                {
                    Mine();
                }
                if (whoAmI == 800)
                {
                    jobCount++;
                    Init();
                    whoAmI = 1;
                }
            }

            if (minerPos.X < center.X)
                center.X = minerPos.X;
            if (minerPos.Y < center.Y)
                center.Y = minerPos.Y;
            if (minerPos.X > oldMinerPos.X)
                oldMinerPos.X = minerPos.X;
            if (minerPos.Y > oldMinerPos.Y)
                oldMinerPos.Y = minerPos.Y;
        
            if (jobCount > jobCountMax)
            {
                progressText = "Process complete";
                int layer = (int)Main.worldSurface;
                int offset = Main.maxTilesY / 2;
                if (minerPos.X < center.X)
                {
                    genPos[0] = new Vector2(minerPos.X, center.Y);
                    genPos[1] = oldMinerPos;
                }
                if(minerPos.X > center.X)
                {
                    genPos[0] = center;
                    genPos[1] = oldMinerPos;
                }
                if (!switchMode)
                {
                    switchMode = true;
                    Dig();
                }
            }
            if (switchMode)
            {
                //  jobCount--;
                Terminate();
                //  Reset();
            }
        }
        public void AverageMove() // most average path, sometimes most interesting
        {
            size = Main.rand.Next(1, 3);
            if (Main.rand.Next(4) == 1)
            {
                minerPos.X += 16;
                Dig();
            }
            if (Main.rand.Next(4) == 1)
            {
                minerPos.X -= 16;
                Dig();
            }
            if (Main.rand.Next(4) == 1)
            {
                minerPos.Y += 16;
                Dig();
            }
            if (Main.rand.Next(4) == 1)
            {
                minerPos.Y -= 16;
                Dig();
            }
            GenerateNewMiner();
        }
        public void DirectionalMove() // tends to stick to a path
        {
            size = Main.rand.Next(1, 3);
            if (Main.rand.Next(4) == 1 && Main.tile[(int)(minerPos.X + 16 + (16 * lookFurther)) / 16, (int)minerPos.Y / 16].active())
            {
                minerPos.X += 16;
                Dig();
            }
            if (Main.rand.Next(4) == 1 && Main.tile[(int)(minerPos.X - 16 - (16 * lookFurther)) / 16, (int)minerPos.Y / 16].active())
            {
                minerPos.X -= 16;
                Dig();
            }
            if (Main.rand.Next(4) == 1 && Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y + 16 + (16 * lookFurther)) / 16].active())
            {
                minerPos.Y += 16;
                Dig();
            }
            if (Main.rand.Next(4) == 1 && Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y - 16 - (16 * lookFurther)) / 16].active())
            {
                minerPos.Y -= 16;
                Dig();
            }
            if (!Main.tile[(int)(minerPos.X + 16 + (16 * lookFurther)) / 16, (int)minerPos.Y / 16].active() &&
                !Main.tile[(int)(minerPos.X - 16 - (16 * lookFurther)) / 16, (int)minerPos.Y / 16].active() &&
                !Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y + 16 + (16 * lookFurther)) / 16].active() &&
                !Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y - 16 - (16 * lookFurther)) / 16].active())
            {
                lookFurther++;
                if (lookFurther % 2 == 0) progressText = "Looking " + lookFurther + " tiles further";
                PlaceWater();
            }
            else lookFurther = 0;
            GenerateNewMiner();
        }
        public void ToTheSurfaceMove() // it likes randomizer = 3
        {
            moveID = 0;
            if (Main.tile[(int)(minerPos.X + 16 + (16 * lookFurther)) / 16, (int)minerPos.Y / 16].active())
            {
                moveID++;
            }
            if (Main.tile[(int)(minerPos.X - 16 - (16 * lookFurther)) / 16, (int)minerPos.Y / 16].active())
            {
                moveID++;
            }
            if (Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y + 16 + (16 * lookFurther)) / 16].active())
            {
                moveID++;
            }
            if (Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y - 16 - (16 * lookFurther)) / 16].active())
            {
                moveID++;
            }
            int randomizer = Main.rand.Next(0, moveID);
            size = Main.rand.Next(1, 3);
            if (randomizer == 0)
            {
                lookFurther++;
                int adjust = Main.rand.Next(1, 4);
                if (adjust == 1)
                {
                    minerPos.X -= 16;
                    PlaceWater();
                    Dig();
                }
                else if (adjust == 2)
                {
                    minerPos.X += 16;
                    PlaceWater();
                    Dig();
                }
                else if (adjust == 3)
                {
                    minerPos.Y -= 16;
                    PlaceWater();
                    Dig();
                }
                else if (adjust == 4)
                {
                    minerPos.Y += 16;
                    PlaceWater();
                    Dig();
                }
                return;
            }
            if (randomizer == 1)
            {
                minerPos.X -= 16;
                Dig();
            }
            if (randomizer == 2)
            {
                minerPos.Y -= 16;
                Dig();
            }
            if (randomizer == 3)
            {
                minerPos.Y += 16;
                Dig();
            }
            if (randomizer == 4)
            {
                minerPos.X += 16;
                Dig();
            }
            GenerateNewMiner();
            lookFurther = 0;
        }
        public void StiltedMove()    // stilted, might work if more iterations of movement, sometimes longest tunnel
        {                                   // best water placer, there's another move that could be extracted from this if the ID segments were removed
            moveID = 0;
            if (Main.tileSolid[Main.tile[(int)(minerPos.X + 16 + (16 * lookFurther)) / 16, (int)minerPos.Y / 16].type])
            {
                moveID++;
            }
            if (Main.tileSolid[Main.tile[(int)(minerPos.X - 16 - (16 * lookFurther)) / 16, (int)minerPos.Y / 16].type])
            {
                moveID++;
            }
            if (Main.tileSolid[Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y + 16 + (16 * lookFurther)) / 16].type])
            {
                moveID++;
            }
            if (Main.tileSolid[Main.tile[(int)minerPos.X / 16, (int)(minerPos.Y - 16 - (16 * lookFurther)) / 16].type])
            {
                moveID++;
            }
            int randomizer = Main.rand.Next(0, moveID);
            size = Main.rand.Next(1, 3);
            if (randomizer == 0)
            {
                lookFurther++;
                int adjust = Main.rand.Next(1, 4);
                if (adjust == 1)
                {
                    minerPos.X -= 16 * 2;
                    PlaceWater();
                }
                else if (adjust == 2)
                {
                    minerPos.X += 16 * 2;
                    PlaceWater();
                }
                else if (adjust == 3)
                {
                    minerPos.Y -= 16 * 2;
                    PlaceWater();
                }
                else if (adjust == 4)
                {
                    minerPos.Y += 16 * 2;
                    PlaceWater();
                }
                return;
            }
            if (randomizer == 1 && Main.rand.Next(6) == 2)
            {
                minerPos.X -= 16;
                Dig();
            }
            if (randomizer == 2 && Main.rand.Next(10) == 4)
            {
                minerPos.Y -= 16;
                Dig();
            }
            if (randomizer == 3)
            {
                minerPos.Y += 16;
                Dig();
            }
            if (randomizer == 4 && Main.rand.Next(5) == 4)
            {
                minerPos.X += 16;
                Dig();
            }
            GenerateNewMiner();
            lookFurther = 0;
        }
        public void GenerateNewMiner()
        {
            int randomizer = Main.rand.Next(0, 100);
            if (randomizer < 20 && whoAmI < 800)
            {
            //  Codable.RunGlobalMethod("ModWorld", "miner.Init", new object[] { 0 });
            //  progressText = "Miner " + whoAmI + " created";
                whoAmI++;
            
            //  unecessary, jobCount takes care of new mining tasks
                //  miner.whoAmI = whoAmI;
            /*  int newMiner = NewMiner(minerPos.X, minerPos.Y, 0, whoAmI);
                ID[newMiner].init = false;
                ID[newMiner].Dig(); */
            //  miner.ID[newID].minerPos = Miner.minerPos;
            }
        }
        public void Dig()
        {
            if (type < 800 * 800)
            {
                type++;
                minePath[type] = minerPos;
            }

            if (!switchMode)
            {
                for (int k = 2; k < 24; k++)
                {
                    WorldGen.PlaceTile((int)(minerPos.X / 16) + k, (int)(minerPos.Y / 16) + k, moda.TileType<Tiles.m_stone>(), true, true);
                    WorldGen.PlaceTile((int)(minerPos.X / 16) - k, (int)(minerPos.Y / 16) - k, moda.TileType<Tiles.m_stone>(), true, true);
                    WorldGen.PlaceTile((int)(minerPos.X / 16) + k, (int)(minerPos.Y / 16) - k, moda.TileType<Tiles.m_stone>(), true, true);
                    WorldGen.PlaceTile((int)(minerPos.X / 16) - k, (int)(minerPos.Y / 16) + k, moda.TileType<Tiles.m_stone>(), true, true);
                    WorldGen.KillWall((int)minerPos.X / 16 + k, (int)minerPos.Y / 16 + k, false);
                    WorldGen.KillWall((int)minerPos.X / 16 - k, (int)minerPos.Y / 16 - k, false);
                    WorldGen.KillWall((int)minerPos.X / 16 + k, (int)minerPos.Y / 16 - k, false);
                    WorldGen.KillWall((int)minerPos.X / 16 - k, (int)minerPos.Y / 16 + k, false);
                }
            }
            if (switchMode)
            {
                for (int k = 0; k < type; k++)
                {
                    minerPos = minePath[k];
                    if (WorldGen.genRand.Next(60) == 0) PlaceWater();
                    if (size == 1)
                    {
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16) + circumference, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 1, false, false, true);
                    }
                    else if (size == 2)
                    {
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16) + circumference, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference * 2, (int)(minerPos.Y / 16) + circumference * 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference * 2, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference * 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16) + 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16) - 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16) - 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16) + 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 2, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 2, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 2, false, false, true);
                    }
                    else if (size == 3)
                    {
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16) + circumference, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference * 2, (int)(minerPos.Y / 16) + circumference * 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference * 2, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference * 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference * 3, (int)(minerPos.Y / 16) + circumference * 3, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + circumference * 3, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference * 3, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16) + 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16) - 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 1, (int)(minerPos.Y / 16) - 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 1, (int)(minerPos.Y / 16) + 1, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 2, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 2, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 2, (int)(minerPos.Y / 16) + 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 2, (int)(minerPos.Y / 16) - 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 2, (int)(minerPos.Y / 16) - 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 2, (int)(minerPos.Y / 16) + 2, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) + 3, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16) - 3, (int)(minerPos.Y / 16), false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) + 3, false, false, true);
                        WorldGen.KillTile((int)(minerPos.X / 16), (int)(minerPos.Y / 16) - 3, false, false, true);
                    }
                }
            }
        }
        public void PlaceWater()
        {
            int randomizer = Main.rand.Next(0, 100);
            if (randomizer < 8)
            { // old randomizer%12 == 0
                Main.tile[(int)(minerPos.X / 16) + circumference, (int)(minerPos.Y / 16)].liquid = 255;
                Main.tile[(int)(minerPos.X / 16), (int)(minerPos.Y / 16) + circumference].liquid = 255;
                WorldGen.SquareTileFrame((int)(minerPos.X / 16), (int)(minerPos.Y / 16));
            }
            if (circumference != 1) return;
        }
        public void Mine()
        {
            //	AverageMove();
            DirectionalMove();
            //	ToTheSurfaceMove();
            //	StiltedMove();
            //  used only in the removal of newly generated miners
/*          if(russianRoulette){
                int life = -5;
                int death = 60;
                int roulette = Main.rand.Next(life, death);
                if(roulette == death && whoAmI > 0){
                    ID[whoAmI] = null;
                    whoAmI++;
                }
            }   */
            if (!switchMode && minerPos != Vector2.Zero && jobCount < jobCountMax/* && (minerPos.X < edge * 16 && minerPos.X > (rightBounds - edge) * 16 && minerPos.Y < edge * 16 && minerPos.Y > (bottomBounds - edge) * 16)*/)
            {
                int RandomX = WorldGen.genRand.Next(-2, 2);
                int RandomY = WorldGen.genRand.Next(-2, 2);
                if (RandomX != 0 && RandomY != 0)
                {
                    if (minerPos.Y / 16 > Main.maxTilesY / 3 && minerPos.Y < bottomBounds - edge)
                    {
                        mineBlock = new Vector2(mineBlockX * RandomX, mineBlockY * RandomY);
                        minerPos += mineBlock;
                    }
                    if (minerPos.Y / 16 < Main.maxTilesY / 3)
                    {
                        minerPos.Y += mineBlockY;
                    }
                    if (minerPos.Y /16 > bottomBounds - edge)
                    {
                        minerPos.Y -= mineBlockY;
                    }
                }
                else return;
            }
        }
        public void Randomizer()
        {
            randomX = Main.rand.Next(edge, rightBounds - edge);
            randomY = Main.rand.Next((int)Main.rockLayer, bottomBounds - edge);
            for (int j = -1; j < 1; j++)
            {
                circumference = j;
            }
        }
        public void Terminate()
        {
            ArchaeaWorld.miner.active = false;          
        }
        public void Reset()
        {
            progressText = "";
            type = 0;
            whoAmI = 0;
            jobCount = 0;
            switchMode = false;
            init = false;
            center = new Vector2((Main.maxTilesX / 2) * 16, (Main.maxTilesY / 2) * 16);
            minerPos = center;
            oldMinerPos = default(Vector2);
            genPos[0] = default(Vector2);
            genPos[1] = default(Vector2);
            for (int i = 0; i < minePath.Length - 1; i++)
            {
                minePath[i] = default(Vector2);
            }
            if (Main.maxTilesX == 4200)
                jobCountMax = 32;
            if (Main.maxTilesX == 6400)
                jobCountMax = 48;
            if (Main.maxTilesX == 8400)
                jobCountMax = 64;
        }
    }

    /*
    public class SkyTower : ModWorld
    {
        bool init;
        bool nextTask;
        public bool active = true;
        int wallLeft, wallRight;
        int towerSize = 80;
        int fill, iterate;
        int layers;
        int variance;
        int[,] room = default(int[,]);
        float appreciation = 1.2f;
        Vector2 origin;
        public void Reset()
        {
            init = false;
            nextTask = false;
            active = true;

            fill = 0;
            iterate = 0;
            layers = 0;
        } 
        public void Init()
        {
            bool choice = WorldGen.genRand.Next(2) == 0;
            variance = WorldGen.genRand.Next(-128, 128);

            origin = new Vector2(Main.spawnTileX, Main.spawnTileY);
            if (choice)
                origin.X /= 2f;
            else origin.X *= 1.5f;
            origin.X += variance;

            wallLeft = (int)origin.X - towerSize;
            wallRight = (int)origin.X + towerSize;
        }

        public void Update()
        {
            if(!init)
            {
                Init();
                init = true;
            }

            TowerLayers(wallLeft, wallRight, (int)origin.Y, 24f, 3);
            if (nextTask)
            {
                InteriorGen(wallLeft, wallRight, 32);
                nextTask = false;
            }
        }

        public void TowerLayers(int leftBounds, int rightBounds, int cieling, float height, int maxLayers)
        {
            for (int i = leftBounds; i < rightBounds; i++)
            {
                if (cieling - fill > 30)
                {
                    Tile tile = Framing.GetTileSafely(i, cieling - fill);
                    tile.type = TileID.Sunplate;
                    tile.active(true);
                    tile.slope(0);
                }
                if (i == rightBounds - 1)
                {
                    fill++;
                    i = leftBounds;
                }
                if (fill == (int)height)
                {
                    leftBounds += 15;
                    rightBounds -= 15;
                    cieling -= (int)height;
                    height *= appreciation;
                    layers++;
                    fill = 0;
                }
                if (layers >= maxLayers)
                {
                    nextTask = true;
                    break;
                }
            }
        }

        public void InteriorGen(int left, int right, int roomCount)
        {
            for (int i = 0; i < roomCount; i++)
            {
                int x = WorldGen.genRand.Next(left, right);
                int y = WorldGen.genRand.Next((int)((int)origin.Y - (24f * appreciation * layers)), (int)origin.Y);
                if (y > 30)
                {
                    if (Main.tile[x, y].type == TileID.Sunplate)
                    {
                        PlaceRoom(x, y);
                    }
                }
                if(i == roomCount)
                    active = false;
            }
        }

        public void PlaceRoom(int i, int j)
        {
            RandomRoom();
            for (int y = 0; y < room.GetLength(0); y++)
            {
                for (int x = 0; x < room.GetLength(1); x++)
                {
                    int m = i + x;
                    int n = j + y;
                    if (WorldGen.InWorld(m, n, 30))
                    {
                        Tile tile = Framing.GetTileSafely(m, n);
                        switch (room[y, x])
                        {
                            case 0:
                                tile.type = 0;
                                tile.active(false);
                                break;
                        }
                    }
                }
            }
        }

        public void RandomRoom()
        {
            int choice = WorldGen.genRand.Next(4);
            switch (choice)
            {
                case 0: // 10 x 8
                    room = new int[,]
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                    };
                    break;
                case 1: // 12 x 6
                    room = new int[,]
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                    };
                    break;
                case 2: // 12 x 10
                    room = new int[,]
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                    };
                    break;
                case 3: // 16 x 8
                    room = new int[,]
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                    };
                    break;
                case 4: // 8 x 8
                    room = new int[,]
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                    };
                    break;
            }
        }
    }   */
}