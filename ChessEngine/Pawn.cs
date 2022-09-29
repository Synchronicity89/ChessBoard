using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Pawn : Piece
    {
        public override int[,,] Points { get; } = Constants.PawnMoves;

        public Pawn(Board board, int pieceColor, int x, int y) : base(board, pieceColor, x, y)
        {
            xLen = Constants.PawnMoves.GetLength(0);
            yLen = Constants.PawnMoves.GetLength(1);
            zLen = Constants.PawnMoves.GetLength(2);
        }

        public override Move[,,] Moves()
        {
            Move[,,] moves = new Move[xLen, yLen, zLen];

            for (int x = 0; x < xLen; x++)
            {
                for (int y = 0; y < yLen; y++)
                {
                    int relX = Points[x, y, 0];
                    int relY = Points[x, y, 1];
                    int absX = this.X + PieceColor * relX;
                    int absY = this.Y + PieceColor * relY;
                    if (absX < 0 || absY < 0 || absX > 7 || absY > 7) continue;

                    if (Board.Grid[absX, absY] == null && (Math.Abs(relX) == 1 && Math.Abs(relY) == 1) == false)
                    {
                        if (Math.Abs(relY) == 2 && !((this.PieceColor == -1 && this.Y == 6) || (this.PieceColor == 1 && this.Y == 1))) continue;
                        moves[x, y, 0] = new Move(PieceColor * relX, PieceColor * relY, this);
                        continue;
                    }
                    if (Board.Grid[absX, absY] != null && Board.Grid[absX, absY].PieceColor != this.PieceColor && Math.Abs(relX) == 1 && Math.Abs(relY) == 1 ||
                        //en passant
                        //checking to the right
                        (absX > X && EnPassant(absX, 1)) || (absX < X && EnPassant(absX, -1)))
                    {
                        moves[x, y, 0] = new Move(PieceColor * relX, PieceColor * relY, this, true);
                        break;
                    }
                    break;
                }
            }
            return moves;
        }

        private bool EnPassant(int absX, int lr)
        {
            var IsThereAPieceThere = Board.Grid[X + lr, Y] != null; if (IsThereAPieceThere != true) goto Exit;
            //is it a pawn
            var IsItAPawn = Board.Grid[X + lr, Y] is Pawn; if(IsItAPawn != true) goto Exit;
            //is it an opponent piece
            var IsItAnOpponentPiece = Board.Grid[X + lr, Y].PieceColor != this.PieceColor;
            //did it just move
            var DidItJustMove = Board.Grid[X + lr, Y].LastMoveNumber == Board.NumberOfMoves - 1;
            //was the move distance 2
            var WasMoveDistance2 = Math.Abs((Board.Grid[X + lr, Y] as Pawn).LastMoveDistance) == 2;
            Exit:
            return (
                //is there a piece there
                Board.Grid[X + lr, Y] != null &&
                //is it a pawn
                Board.Grid[X + lr, Y] is Pawn &&
                //is it an opponent piece
                Board.Grid[X + lr, Y].PieceColor != this.PieceColor &&
                //did it just move
                Board.Grid[X + lr, Y].LastMoveNumber == Board.NumberOfMoves - 1 &&
                //was the move distance 2
                Math.Abs((Board.Grid[X + lr, Y] as Pawn).LastMoveDistance) == 2);
        }

        private bool IsOnBoard(Move m)
        {
            return (m.Y + PieceColor * 1 <= 8 && m.Y + PieceColor * 1 >= 0) &&
                (m.X + PieceColor * 1 <= 8 && m.X + PieceColor * 1 >= 0);
        }

    }
}
