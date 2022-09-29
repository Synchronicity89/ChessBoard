﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Knight : Piece
    {
        public override int[,,] Points { get; } = Constants.KnightMoves;
        public Knight(Board board, int pieceColor, int x, int y) : base(board, pieceColor, x, y)
        {
        }
    }
}