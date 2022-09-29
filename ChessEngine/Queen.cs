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
        int[,,] points;
        public override int[,,] Points
        {
            get
            {
                if(points == null)
                { 
                    points = new int[Constants.RookMoves.GetLength(0) + Constants.BishopMoves.GetLength(0), Constants.RookMoves.GetLength(1), Constants.RookMoves.GetLength(2)];
                    var xLen = Constants.RookMoves.GetLength(0);
                    var yLen = Constants.RookMoves.GetLength(1);
                    var zLen = Constants.RookMoves.GetLength(2);
                    for (int x = 0; x < xLen; x++)
                    {
                        for (int y = 0; y < yLen; y++)
                        {
                            for (int z = 0; z < zLen; z++)
                            {
                                points[x, y, z] = Constants.RookMoves[x, y, z];
                            }
                        }
                    }
                    xLen = Constants.BishopMoves.GetLength(0);
                    yLen = Constants.BishopMoves.GetLength(1);
                    zLen = Constants.BishopMoves.GetLength(2);
                    for (int x = 0; x < xLen; x++)
                    {
                        for (int y = 0; y < yLen; y++)
                        {
                            for (int z = 0; z < zLen; z++)
                            {
                                points[x + 4, y, z] = Constants.BishopMoves[x, y, z];
                            }
                        }
                    }
                }
                return points;
            }
        }
        public Queen(Board board, int pieceColor, int x, int y) : base(board, pieceColor, x, y)
        {
        }
    }
}
