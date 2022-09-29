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
            return Grid[col, row].Points;
        }

        public List<Move> Flatten(Move[,,] pieceMoves)
        {
            var xLen = pieceMoves.GetLength(0);
            var yLen = pieceMoves.GetLength(1);
            var movesFlat = new List<Move>();
            for (int x = 0; x < xLen; x++)
            {
                for (int y = 0; y < yLen; y++)
                {
                    if (pieceMoves[x, y, 0] != null)
                    {
                        movesFlat.Add(pieceMoves[x, y, 0]);
                    }
                }
            }

            return movesFlat;
        }

        public void MovePiece(Move move)
        {
            if (move.Piece is Pawn && Grid[move.X + move.Piece.X, move.Y + move.Piece.Y] == null)
            {//En Passant
                Grid[move.Piece.X + move.X, move.Piece.Y] = null;
            }
            Grid[move.X + move.Piece.X, move.Y + move.Piece.Y] = move.Piece;
            Grid[move.Piece.X, move.Piece.Y] = null;
            move.Piece.X = move.X + move.Piece.X;
            move.Piece.Y = move.Y + move.Piece.Y;
            if (move.Piece is King && Math.Abs(move.X) == 2)
            {
                //Castling. Move rook too
                if(move.X == -2)
                {//Queen side castle
                    Grid[3, move.Piece.Y] = Grid[0, move.Piece.Y];
                    Grid[0, move.Piece.Y] = null;
                    Grid[3, move.Piece.Y].X = 3;
                    //Y should be correct already
                }
                else
                {
                    Grid[5, move.Piece.Y] = Grid[7, move.Piece.Y];
                    Grid[7, move.Piece.Y] = null;
                    Grid[5, move.Piece.Y].X = 5;
                }
            }

            ColorToMove *= -1;
            move.Piece.LastMoveNumber = NumberOfMoves;
            move.Piece.LastMoveDistance = Math.Max(Math.Abs(move.Y), Math.Abs(move.X));
            NumberOfMoves++; 
        }
    }
}