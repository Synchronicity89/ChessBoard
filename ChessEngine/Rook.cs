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
        public override int[,,] Points { get; } = Constants.RookMoves;
        public Rook(Board board, int pieceColor, int x, int y) : base(board, pieceColor, x, y)
        {
        }
    }
}
