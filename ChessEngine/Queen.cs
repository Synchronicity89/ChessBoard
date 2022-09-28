using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Queen : Piece
    {
        //public Queen() { }
        public Queen(Board board, int pieceColor, int x, int y) : base(board, pieceColor, x, y)
        {
            xLen = Constants.BishopMoves.GetLength(0) + Constants.RookMoves.GetLength(0);
            yLen = Constants.BishopMoves.GetLength(1);
            zLen = Constants.BishopMoves.GetLength(2);
        }
    }
}
