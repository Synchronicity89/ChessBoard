using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class King : Piece
    {
        //public King() { }
        public King(Board board, int pieceColor, int x, int y) : base(board, pieceColor, x, y)
        {
            xLen = Constants.KingMoves.GetLength(0);
            yLen = Constants.KingMoves.GetLength(1);
            zLen = Constants.KingMoves.GetLength(2);
        }

        public bool HasMoved { get; internal set; }
    }
}
