namespace ChessEngine
{
    public class Move
    {
        public Move(int x, int y, Piece piece, bool take = false)
        {
            X = x;
            Y = y;
            Take = take;
            Piece = piece;
        }
        public bool Take { get; set; }

        public int X { get; internal set; }
        public int Y { get; internal set; }

        public Piece Piece { get; set; }
        public override string ToString()
        {
            string moveText = "";
            string pieceType = Piece.GetType().Name;
            string piece = ""; //empty string is pawn
            string lastPos = "" + "abcdefgh"[Piece.LastX] + "12345678"[Piece.LastY];
            string currPos = "" + "abcdefgh"[Piece.X] + "12345678"[Piece.Y];
            switch (pieceType)
            {
                case "Rook":
                    piece += "R" + lastPos;
                    break;
                case "Knight":
                    piece += "N" + lastPos;
                    break;
                case "Bishop":
                    piece += "B" + lastPos;
                    break;
                case "Queen":
                    piece += "Q" + lastPos;
                    break;
                case "King":
                    piece += "K" + lastPos;
                    break;
                case "Pawn":
                    piece += "abcdefgh"[Piece.X + X];
                    break;
            }
            //TODO: castling, checkmate, promotion etc

            moveText += piece + (Take ? "x" : "") + currPos;

            return moveText;
        }
    }
}