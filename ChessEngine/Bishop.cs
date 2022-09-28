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
        public Bishop(Board board, int pieceColor, int x, int y) : base(board, pieceColor, x, y)
        {
            xLen = Constants.BishopMoves.GetLength(0);
            yLen = Constants.BishopMoves.GetLength(1);
            zLen = Constants.BishopMoves.GetLength(2);
        }
        //public override Move[,,] Moves(int[,,] points)
        //{
        //    int index = 0;
        //    Move[,,] moves = new Move[xLen, yLen, zLen];
        //    index = base.GenerateMoves(index, moves, points, this);
        //    return moves;
        //}

        //public override Move[,,] TakeMoves(int[,,] points)
        //{
        //    //return this.Moves()[0,0,].Where(m => m.Take == true);
        //    Move[,,] takeMoves = new Move[xLen, yLen, zLen];
        //    var moves = this.Moves(points);
        //    for (int i = 0; i < 8; i++)
        //    {
        //        if (moves[i, 0, 0] != null && moves[i, 0, 0].Take == true)
        //        {
        //            takeMoves[i, 0, 0] = moves[i, 0, 0];
        //        }
        //    }
        //    return takeMoves;
        //}

    }
}
