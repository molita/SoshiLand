﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SoshiLandSilverlight
{
    public class Tile
    {
        private string Name;
        private Texture2D texture;
        private TileType tileType;

        public TileType getTileType
        {
            get { return tileType; }
        }

        public string getName
        {
            get { return Name; }
        }

        public Tile(string n, TileType t)
        {
            Name = n;
            tileType = t;
        }
    }
}
