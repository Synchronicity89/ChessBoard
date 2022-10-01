﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Knight : Piece
    {
        [JsonIgnore]
        public override int[,,] Points { get; } = Constants.KnightMoves;
        public Knight() { }
    }
}
