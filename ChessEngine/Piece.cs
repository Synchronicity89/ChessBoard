using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace ChessEngine
{
    public abstract class Piece
    {
        protected int xLen;
        protected int yLen;
        protected int zLen;

        int lastX = -1;
        int lastY = -1;
        int currentPieceX = -1;
        int currentPieceY = -1;

        public int X
        {
            get
            {
                return currentPieceX;
            }
            set
            {
                lastX = currentPieceX;
                currentPieceX = value;
            }
        }
        public int Y
        {
            get
            {
                return currentPieceY;
            }
            set
            {
                lastY = currentPieceY;
                currentPieceY = value;
            }
        }
        public int PieceColor { get; set; }
        [JsonIgnore]
        public Board Board 
        {
            get { return Board.Instance; }
        }
        public int LastMoveNumber { get; internal set; }
        public int LastMoveDistance { get; internal set; }

        [JsonIgnore]
        public virtual int[,,] Points { get; }
        public int LastX { get => lastX; set => lastX = value; }
        public int LastY { get => lastY; set => lastY = value; }

        public Piece()
        {
            xLen = Points.GetLength(0);
            yLen = Points.GetLength(1);
            zLen = Points.GetLength(2);
        }
        public virtual Move[,,] Moves()
        {
            Move[,,] moves = new Move[xLen, yLen, zLen];
            GenerateMoves(moves, Points, this);
            return moves;
        }
        public Move[,,] TakeMoves()
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
                                    if (Board.Grid[i, Y] != null)
                                    {
                                        doBreak = true;
                                        break;
                                    }
                                }
                                if (doBreak) break;
                                //Any pieces attacking squares the king would have to move over, plus is the king already in check
                                //Make a fake Queen and then a Knight on each of the three squares, make a list of pieces they could "take"
                                //See if any of these fake victims is threatening the King or any of its traversed squares
                                for (int i = X; Math.Abs(X - i) < 3; i += (relX / 2))
                                {
                                    int a = i;
                                    int b = Y;
                                    doBreak = Board.Threat(rookCandidate.PieceColor, a, b);
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
                        Board.AddIfLegal(moves, x, y, new Move(relX, relY, this));
                        continue;
                    }
                    if (Board.Grid[absX, absY].PieceColor != this.PieceColor) 
                    {
                        if ((this is King) && Math.Abs(relX) == 2) break;
                        Board.AddIfLegal(moves, x, y, new Move(relX, relY, this, true));
                        break;
                    }
                    break;
                }
            }
        }

        internal void UndoMove()
        {
            X = LastX;
            Y = LastY;
        }
    }
}