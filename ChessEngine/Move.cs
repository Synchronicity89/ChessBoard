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
    }
}