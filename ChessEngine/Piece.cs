using System.Drawing;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace ChessEngine
{
    public abstract class Piece
    {
        protected int xLen;
        protected int yLen;
        protected int zLen;
        public int X { get; set; }
        public int Y { get; set; }
        public int PieceColor { get; set; }
        public Board Board {get; set;}
        public int LastMove { get; internal set; }

        public Piece(Board board, int pieceColor, int x, int y)
        {
            Board = board;
            PieceColor = pieceColor;
            X = x;
            Y = y;
        }

        public virtual Move[,,] Moves(int[,,] points)
        {
            int index = 0;
            Move[,,] moves = new Move[xLen, yLen, zLen];
            index = GenerateMoves(index, moves, points, this);
            return moves;
        }

        public virtual Move[,,] TakeMoves(int[,,] points)
        {
            //return this.Moves()[0,0,].Where(m => m.Take == true);
            Move[,,] takeMoves = new Move[xLen, yLen, zLen];

            var moves = this.Moves(points);
            for (int x = 0; x < xLen; x++)
            {
                for (int y = 0; x < yLen; x++)
                {
                    if (moves[x, y, 0] != null && moves[x, 0, 0].Take == true)
                    {
                        takeMoves[x, y, 0] = moves[x, y, 0];
                    }
                }
            }
            return takeMoves;
        }

        public int GenerateMoves(int index, Move[,,] moves, int[,,] points, Piece piece)
        {
            for (int x = 0; x < xLen; x++)
            {
                for (int y = 0; y < yLen; y++)
                {
                    int relX = points[x, y, 0];
                    int relY = points[x, y, 1];
                    int absX = this.X + relX;
                    int absY = this.Y + relY;
                    if (absX < 0 || absY < 0 || absX > 7 || absY > 7) continue;

                    if (Board.Grid[absX, absY] == null)
                    {
                        moves[x, y, 0] = new Move(relX, relY, this);
                        if ((this is King) && (this as King).HasMoved && Math.Abs(y) == 2) continue;
                        continue;
                    }
                    if (Board.Grid[absX, absY].PieceColor != this.PieceColor) 
                    {
                        //if (this is Pawn)
                        //{
                        //    var pawn = (this as Pawn);
                        //    //check for opponent pawn to left and right
                        //    if(
                        //}
                        moves[x, y, 0] = new Move(relX, relY, this, true);
                        break;
                    }
                    break;
                }
            }
            return index;
        }

        public int GeneratePoints2(int index, int yInc, int xInc, Point[] points, int maxCount = 1)
        {
            for (int y = -1 * yInc * maxCount; y <= yInc; y += yInc * maxCount)
            {
                for (int x = -1 * xInc * maxCount; x <= xInc; x += xInc * maxCount)
                {
                    points[index++] = new Point(x, y);
                }
            }

            return index;
        }
        public int GeneratePoints3(int index, int yInc, int xInc, Move[] points, int maxCount = 1)
        {
            for(int dir = -1;dir < 1;dir+=2)
            {
                int startY = dir * yInc;
                int endY = yInc * maxCount * -dir;
                int startX = dir * xInc;
                int endX = xInc * maxCount * -dir;
                int incY = -dir * yInc;
                int incX = -dir * xInc;
                int x = startX;

                for (int y = startY; y < endY+1 && x < endX+1; y += incY, x += incX)
                {
                    if(x == 0 && y == 0)
                    {
                        continue;
                    }
                    int absX = this.X + x;
                    int absY = this.Y + y;
                    if(absX < 0 || absY < 0 || absX > 7 || absY > 7) continue;

                    if (Board.Grid[absX, absY] == null)
                    { 
                        points[index++] = new Move(x, y, this);
                        continue;
                    }
                    if (Board.Grid[absX, absY].PieceColor != this.PieceColor)
                    { 
                        points[index++] = new Move(x, y, this, true);
                    }
                    break;
                }
            }
            return index;
        }
    }
}