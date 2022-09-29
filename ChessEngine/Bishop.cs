using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Bishop : Piece
    {
        //public Bishop() { }
        public override int[,,] Points { get; } = Constants.BishopMoves;
        public Bishop(Board board, int pieceColor, int x, int y) : base(board, pieceColor, x, y)
        {
        }
    }
}
