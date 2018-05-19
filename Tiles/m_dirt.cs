using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArchaeaMod.Tiles
{
    public class m_dirt : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            // temporarily false for incorrect tile transition
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = false;
            drop = mod.ItemType("magno_dirt");
            //  UI map tile color
            AddMapEntry(new Color(141, 152, 115));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}