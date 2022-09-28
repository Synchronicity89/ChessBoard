using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Rook : Piece
    {
        //public Rook() { }
        public Rook(Board board, int pieceColor, int x, int y) : base(board, pieceColor, x, y)
        {
            xLen = Constants.RookMoves.GetLength(0);
            yLen = Constants.RookMoves.GetLength(1);
            zLen = Constants.RookMoves.GetLength(2);
        }
    }
}
