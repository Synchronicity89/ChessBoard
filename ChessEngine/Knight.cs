using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Knight : Piece
    {
        //public Knight() { }
        public Knight(Board board, int pieceColor, int x, int y) : base(board, pieceColor, x, y)
        {
            xLen = Constants.KnightMoves.GetLength(0);
            yLen = Constants.KnightMoves.GetLength(1);
            zLen = Constants.KnightMoves.GetLength(2);
        }
    }
}
