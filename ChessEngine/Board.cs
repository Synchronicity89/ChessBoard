using System.Security.Cryptography.X509Certificates;

namespace ChessEngine
{
    public class Board
    {

        public Board()
        {
            Grid = new Piece[8, 8];
        }

        public Piece[,] Grid { get; set; }
        public int ColorToMove { get; set; } = 1;
        public int NumberOfMoves { get; private set; }

        public void StandardSetup()
        {
            for (int i = 0; i < 8; i++)
            {
                AddPiece<Pawn>(i, 1, 1);

                if (i == 0 || i == 7)
                {
                    AddPiece<Rook>(i, 0, 1);
                }
                if (i == 1 || i == 6)
                {
                    AddPiece<Knight>(i, 0, 1);
                }
                if (i == 2 || i == 5)
                {
                    AddPiece<Bishop>(i, 0, 1);
                }
                if (i == 3)
                {
                    AddPiece<Queen>(i, 0, 1);
                }
                if (i == 4)
                {
                    AddPiece<King>(i, 0, 1);
                }
            }
        }

        public void AddPiece<T>(int x, int y, int color, bool mirror = true) where T : Piece
        {
            Grid[x, y] = (Piece)Activator.CreateInstance(typeof(T), this, color, x, y);
            if (mirror)
            {
                Grid[x, 7 - y] = (Piece)Activator.CreateInstance(typeof(T), this, -1 * color, x, 7 - y);
            }
        }

        public int[,,] GetMoves(int col, int row)
        {
            if (Grid[col, row] == null || ColorToMove != Grid[col, row].PieceColor) return null;
            int[,,] points = { { { } } };

            switch (Grid[col, row].GetType().Name)
            {
                case "Knight":
                    points = Constants.KnightMoves;
                    break;
                case "Bishop":
                    points = Constants.BishopMoves;
                    break;
                case "Pawn":
                    points = Constants.PawnMoves;
                    break;
                case "Rook":
                    points = Constants.RookMoves;
                    break;
                case "King":
                    points = Constants.KingMoves;
                    break;
                case "Queen":
                    points = new int[Constants.RookMoves.GetLength(0) + Constants.BishopMoves.GetLength(0), Constants.RookMoves.GetLength(1), Constants.RookMoves.GetLength(2)];
                    var xLen1 = Constants.RookMoves.GetLength(0);
                    var yLen1 = Constants.RookMoves.GetLength(1);
                    var zLen1 = Constants.RookMoves.GetLength(2);
                    for (int x = 0; x < xLen1; x++)
                    {
                        for (int y = 0; y < yLen1; y++)
                        {
                            for (int z = 0; z < zLen1; z++)
                            {
                                points[x, y, z] = Constants.RookMoves[x, y, z];
                            }
                        }
                    }
                    xLen1 = Constants.BishopMoves.GetLength(0);
                    yLen1 = Constants.BishopMoves.GetLength(1);
                    zLen1 = Constants.BishopMoves.GetLength(2);
                    for (int x = 0; x < xLen1; x++)
                    {
                        for (int y = 0; y < yLen1; y++)
                        {
                            for (int z = 0; z < zLen1; z++)
                            {
                                points[x + 4, y, z] = Constants.BishopMoves[x, y, z];
                            }
                        }
                    }
                    break;
                default:
                    throw new InvalidOperationException("case statement failed");
            }

            return points;
        }

        public void MovePiece(Move move)
        {
            Grid[move.X + move.Piece.X, move.Y + move.Piece.Y] = move.Piece;
            Grid[move.Piece.X, move.Piece.Y] = null;
            move.Piece.X = move.X + move.Piece.X;
            move.Piece.Y = move.Y + move.Piece.Y;
            ColorToMove *= -1;
            move.Piece.LastMove = NumberOfMoves;
            if(move.Piece is Pawn)
            {
                (move.Piece as Pawn).LastMoveDistance = move.Y;
            }
            NumberOfMoves++; 
        }
    }
}