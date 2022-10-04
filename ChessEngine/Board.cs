using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace ChessEngine
{
    public class Board
    {
        /// <summary>
        /// Currently the board only does standard chess.  No other variant
        /// </summary>
        public Board(bool fake)
        {
            Grid = new Piece[8, 8];
        }
        public Board() : this(false)
        {
            Instance = this;
        }
        public bool fake;
        public static Board Instance { get; private set; }
        public King KingDark { get; set; }
        public King KingLight { get; set; }
        public Piece[,] Grid { get; set; }
        public int ColorToMove { get; set; } = 1;
        public int NumberOfMoves { get; private set; }
        public int NumMovesSincePawnMoved { get; private set; }

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
                    KingDark = (King)Grid[4, 7];
                    KingLight = (King)Grid[4, 0];
                }
            }
        }

        public void AddPiece<T>(int x, int y, int color, bool mirror = true) where T : Piece
        {
            Grid[x, y] = (Piece)Activator.CreateInstance(typeof(T));
            Grid[x, y].PieceColor = color; Grid[x, y].X = x; Grid[x, y].Y = y; Grid[x, y].LastX = x; Grid[x, y].LastY = y;
            if (mirror)
            {
                Grid[x, 7 - y] = (Piece)Activator.CreateInstance(typeof(T));
                Grid[x, 7 - y].PieceColor = -1 * color; Grid[x, 7 - y].X = x; Grid[x, 7 - y].Y = 7 - y; 
                Grid[x, 7 - y].LastX = x; Grid[x, 7 - y].LastY = 7 - y;
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

        public string MovePiece(Move move, Action<King, KingStatus> kingThreat, Func<Pawn, char> promotion)
        {
            List<Piece> piecesMoved = new List<Piece>();
            List<Piece> piecesTaken= new List<Piece>();
            string moveText = "";
            Pawn promoted = null;
            bool darkKingThreat;
            bool lightKingThreat;
            if (move.Piece is Pawn)
            {
                if (Grid[move.X + move.Piece.X, move.Y + move.Piece.Y] == null)
                { 
                    //En Passant
                    piecesTaken.Add(move.Piece); 
                    Grid[move.Piece.X + move.X, move.Piece.Y] = null;
                }
                else if((move.Y + move.Piece.Y) % 7 == 0)
                {
                    //Promotion
                    promoted = (Pawn)move.Piece;
                }
            }
            Grid[move.X + move.Piece.X, move.Y + move.Piece.Y] = move.Piece;
            Grid[move.Piece.X, move.Piece.Y] = null;
            move.Piece.X = move.X + move.Piece.X;
            move.Piece.Y = move.Y + move.Piece.Y;
            piecesMoved.Add(move.Piece);
            if (move.Piece is King)
            {

                darkKingThreat = Threat(KingDark.PieceColor, KingDark.X, KingDark.Y);
                lightKingThreat = Threat(KingLight.PieceColor, KingLight.X, KingLight.Y);
                if(move.Piece.PieceColor == -1 && darkKingThreat) 
                    kingThreat(KingDark, KingStatus.UndoMove | KingStatus.Checked);
                if (move.Piece.PieceColor == -1 && lightKingThreat) 
                    kingThreat(KingLight, KingStatus.UndoMove | KingStatus.Checked);

                if (Math.Abs(move.X) == 2)
                { 
                    //Castling. Move rook too
                    if(move.X == -2)
                    {//Queen side castle
                        Grid[3, move.Piece.Y] = Grid[0, move.Piece.Y];
                        Grid[0, move.Piece.Y] = null;
                        Grid[3, move.Piece.Y].X = 3;
                        //Y should be correct already
                        moveText = "0-0-0";
                        piecesMoved.Add((Rook)Grid[3, move.Piece.Y]);
                    }
                    else
                    {
                        Grid[5, move.Piece.Y] = Grid[7, move.Piece.Y];
                        Grid[7, move.Piece.Y] = null;
                        Grid[5, move.Piece.Y].X = 5;
                        moveText = "0-0";
                        piecesMoved.Add((Rook)Grid[5, move.Piece.Y]);
                    }
                }
            }

            //check for king threat
            darkKingThreat = Threat(KingDark.PieceColor, KingDark.X, KingDark.Y);
            lightKingThreat = Threat(KingLight.PieceColor, KingLight.X, KingLight.Y);

            kingThreat(KingDark, darkKingThreat ? KingStatus.Checked : KingStatus.Unchecked);
            kingThreat(KingLight, lightKingThreat ? KingStatus.Checked : KingStatus.Unchecked);

            if(move.Piece.PieceColor == -1 && darkKingThreat || move.Piece.PieceColor == 1 && lightKingThreat)
            {//undo moves
                foreach(var piece in piecesMoved)
                {
                    Grid[piece.X, piece.Y] = null;
                    piece.UndoMove();
                    Grid[piece.X, piece.Y] = piece;
                }
                foreach(var piece in piecesTaken)
                {
                    Grid[piece.X, piece.Y] = piece;
                }
                return null;
            }
            //TODO: look for stalemate or checkmate
            if(promoted != null)
            {
                char choice = promotion(promoted);
                switch(choice)
                {
                    case 'Q':
                        Promote(promoted, new Queen()); break;
                    case 'R':
                        Promote(promoted, new Rook()); break;
                    case 'B':
                        Promote(promoted, new Bishop()); break;
                    case 'N':
                        Promote(promoted, new Knight()); break;
                }
                moveText = ("abcdefgh".Substring(promoted.X, 1)) + ("12345678".Substring(promoted.Y, 1)) + "=" + choice;
            }
            ColorToMove *= -1;
            move.Piece.LastMoveNumber = NumberOfMoves;
            move.Piece.LastMoveDistance = Math.Max(Math.Abs(move.Y), Math.Abs(move.X));
            NumberOfMoves++;
            NumMovesSincePawnMoved++;

            if (move.Piece is Pawn || move.Take == true)
            {
                this.NumMovesSincePawnMoved = 0;
            }
            return string.IsNullOrEmpty(moveText) ? move.ToString() : moveText;
        }

        private void Promote(Pawn promoted, Piece piece)
        {
            Grid[promoted.X, promoted.Y] = piece;
            piece.X = promoted.X;
            piece.Y = promoted.Y;
            piece.LastX = promoted.X;
            piece.LastY = promoted.Y;
            piece.PieceColor = promoted.PieceColor;
        }

        public string CreateFEN()
        {
            //En Passant
            int xEP = -1;
            int yEP = -1;
            var fen = "";
            for (int y = 7; y >= 0; y--)
            {
                int empty = 1;
                for (int x = 0; x < 8; x++)
                {
                    if (Grid[x, y] != null)
                    {
                        Type type = Grid[x, y].GetType();
                        char f = type.Name != "Knight" ? type.Name[0] : 'N';
                        f = Grid[x, y].PieceColor == -1 ? f.ToString().ToLower()[0] : f;
                        fen += f;
                        
                        if(type.Name == "Pawn") 
                            if(Math.Abs(Grid[x, y].LastMoveDistance) == 2) 
                                if( Grid[x, y].LastMoveNumber == this.NumberOfMoves - 1)
                        {
                            xEP = x;
                            yEP = y - Grid[x, y].PieceColor * Grid[x, y].LastMoveDistance/2;
                        }
                    }
                    else
                    {
                        fen += empty;
                    }
                }
                if(y > 0) fen += '/';
            }

            string fen1 = fen.Replace("11111111", "8").Replace("1111111", "7")
                .Replace("111111", "6").Replace("11111", "5")
                .Replace("1111", "4").Replace("111", "3")
                .Replace("11", "2");

            fen1 += " ";

            fen1 += (this.ColorToMove == -1 ? 'b' : 'w') + " ";

            string fen2 = (this.KingLight.CanCastleKingside ? "K" : "");
            fen2 += (this.KingLight.CanCastleQueenside ? 'Q' : "");
            fen2 += (this.KingDark.CanCastleKingside ? 'k' : "");
            fen2 += (this.KingDark.CanCastleQueenside ? 'q' : "");
            if(String.IsNullOrEmpty(fen2))
            {
                fen1 += '-';
            }
            else
            {
                fen1 += fen2;
            }

            fen1 += " ";

            if(xEP > -1 && yEP > -1)
            {
                fen1 += "abcdefgh"[xEP] + (yEP + 1).ToString();
            }
            else
            {
                fen1 += '-';
            }

            fen1 += " " + this.NumMovesSincePawnMoved + " ";

            fen1 += 1 + (int)((this.NumberOfMoves)/2);

            return fen1;

        }

        public bool Threat(int pieceColor, int a, int b)
        {
            Queen fakeQueen = new Queen { PieceColor = pieceColor, X = a, Y = b };
            Knight fakeKnight = new Knight { PieceColor = pieceColor, X = a, Y = b };
            return IsCastleThreatened(fakeQueen) || IsCastleThreatened(fakeKnight);
        }

        private bool IsCastleThreatened(Piece fakePiece)
        {
            bool doBreak = false;
            var takes = Flatten(fakePiece.TakeMoves());
            if (takes.Count() > 0)
            {
                foreach (var take in takes)
                {
                    var fakeTake = Grid[fakePiece.X + take.X, fakePiece.Y + take.Y];
                    if (Flatten(fakeTake.Moves()).Where(tm => tm.X + fakeTake.X == fakePiece.X &&
                        tm.Y + fakeTake.Y == fakePiece.Y).Any())
                    {
                        doBreak = true;
                        break;
                    }
                }
            }

            return doBreak;
        }

        internal void AddIfLegal(Move[,,] moves, int x, int y, Move move)
        {
            //TODO: Check if move is legal before adding it to list of allowable moves
            //for now:
            moves[x, y, 0] = move;
        }
    }

    [Flags]
    public enum KingStatus
    {
        Unchecked = 0,
        Checked = 1,
        Checkmate = 2,
        Stalemate = 4,
        UndoMove = 8
    }
}