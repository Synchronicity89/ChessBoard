using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class Bishop : Piece
    {
        [JsonIgnore]
        public override int[,,] Points { get; } = Constants.BishopMoves;
        public Bishop() { }
    }
}
