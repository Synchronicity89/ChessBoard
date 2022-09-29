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
        public int LastMoveNumber { get; internal set; }
        public int LastMoveDistance { get; internal set; }

        public virtual int[,,] Points { get; }

        public Piece(Board board, int pieceColor, int x, int y)
        {
            xLen = Points.GetLength(0);
            yLen = Points.GetLength(1);
            zLen = Points.GetLength(2);

            Board = board;
            PieceColor = pieceColor;
            X = x;
            Y = y;
        }

        public virtual Move[,,] Moves()
        {
            Move[,,] moves = new Move[xLen, yLen, zLen];
            GenerateMoves(moves, Points, this);
            return moves;
        }
        protected Move[,,] TakeMoves()
        {
            Move[,,] takeMoves = new Move[xLen, yLen, zLen];

            var moves = this.Moves();
            for (int x = 0; x < xLen; x++)
            {
                for (int y = 0; y < yLen; y++)
                {
                    if (moves[x, y, 0] != null && moves[x, y, 0].Take == true)
                    {
                        takeMoves[x, y, 0] = moves[x, y, 0];
                    }
                }
            }
            return takeMoves;
        }

        public void GenerateMoves(Move[,,] moves, int[,,] points, Piece piece)
        {
            for (int x = 0; x < xLen; x++)
            {
                var doBreak = false;
                for (int y = 0; y < yLen; y++)
                {
                    int relX = points[x, y, 0];
                    int relY = points[x, y, 1];
                    int absX = this.X + relX;
                    int absY = this.Y + relY;
                    if (absX < 0 || absY < 0 || absX > 7 || absY > 7) continue;

                    if (Board.Grid[absX, absY] == null)
                    { 
                        //castle logic
                        if((this is King) && Math.Abs(relX) == 2)
                        { 
                            if ((this is King) && (this as King).LastMoveDistance == 0)
                            {
                                //More testing:
                                Piece rookCandidate = Board.Grid[relX < 0 ? 0 : 7, Y];
                                if (rookCandidate is Rook == false || rookCandidate.PieceColor != this.PieceColor) break;
                                //Has rook been moved
                                if (rookCandidate == null ||  rookCandidate.LastMoveDistance != 0) break;
                                //Any pieces between king and rook
                                for(int i = X + (relX / 2); i != rookCandidate.X; i+=(relX/2))
                                {
                                    if (Board.Grid[i, Y] != null) break;
                                }
                                //Any pieces attacking squares the king would have to move over, plus is the king already in check
                                //Make a fake Queen and then a Knight on each of the three squares, make a list of pieces they could "take"
                                //See if any of these fake victims is threatening the King or any of its traversed squares
                                for (int i = X; Math.Abs(X - i) < 3; i += (relX / 2))
                                {
                                    Queen fakeQueen = new Queen(Board, rookCandidate.PieceColor, i, Y);
                                    Knight fakeKnight = new Knight(Board, rookCandidate.PieceColor, i, Y);
                                    doBreak = IsCastleThreatened(doBreak, fakeQueen) || IsCastleThreatened(doBreak, fakeKnight);
                                    if (doBreak) break;
                                }
                                if (doBreak) break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        //move available
                        moves[x, y, 0] = new Move(relX, relY, this);
                        continue;
                    }
                    if (Board.Grid[absX, absY].PieceColor != this.PieceColor) 
                    {
                        if ((this is King) && Math.Abs(relX) == 2) break;
                        moves[x, y, 0] = new Move(relX, relY, this, true);
                        break;
                    }
                    break;
                }
            }
        }

        private bool IsCastleThreatened(bool doBreak, Piece fakePiece)
        {
            var takes = Board.Flatten(fakePiece.TakeMoves());
            if (takes.Count() > 0)
            {
                foreach (var take in takes)
                {
                    var fakeTake = Board.Grid[fakePiece.X + take.X, fakePiece.Y + take.Y];
                    if (Board.Flatten(fakeTake.Moves()).Where(tm => tm.X + fakeTake.X == fakePiece.X &&
                        tm.Y + fakeTake.Y == fakePiece.Y).Any())
                    {
                        doBreak = true;
                        break;
                    }
                }
            }

            return doBreak;
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