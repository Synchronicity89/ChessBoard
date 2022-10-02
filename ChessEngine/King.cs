using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class King : Piece
    {
        [JsonIgnore]
        public override int[,,] Points { get; } = Constants.KingMoves;
        [JsonIgnore]
        public bool CanCastleQueenside
        {
            get
            {
                return NoMovement(0);
            }
        }

        [JsonIgnore]
        public bool CanCastleKingside
        {
            get
            {
                return NoMovement(7);
            }
        }

        private bool NoMovement(int rookX)
        {
            return Board.Grid[rookX, Y] != null && Board.Grid[rookX, Y] is Rook && Board.Grid[rookX, Y].LastMoveDistance == 0 &&
                this.LastMoveDistance == 0;
        }

        public King() { }
    }
}
