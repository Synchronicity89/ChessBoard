using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public static class Constants
    {
        //points containing -9, -9, are to be ignored
        public static readonly int[,,] PawnMoves = { { { 0, 1 }, { 0, 2 } }, { { 1, 1 }, { -9, -9 } }, { { -1, 1 }, { -9, -9 } } };
        public static readonly int[,,] KnightMoves = {{{ 1, 2} }, {{ 2, 1} }, {{ -1, 2 } }, { {2, -1} },
            { { -1, -2} }, {{ -2, -1} }, {{ 1, -2} }, {{-2, 1 } } };
        public static readonly int[,,] BishopMoves = {
            { { 1, 1}, { 2, 2}, { 3, 3}, {4, 4}, {5,5}, { 6,6}, { 7, 7} },
            { { -1, 1}, { -2, 2}, { -3, 3}, {-4, 4}, {-5,5}, {-6,6}, {-7, 7} },
            { { -1, -1}, { -2, -2}, { -3, -3}, {-4, -4}, {-5,-5}, { -6,-6}, { -7, -7} },
            { { 1, -1}, { 2, -2}, { 3, -3}, {4, -4}, {5,-5}, { 6,-6}, { 7, -7} }
        };
        public static readonly int[,,] RookMoves = {
            { { 1, 0}, { 2, 0}, { 3, 0}, {4, 0}, {5, 0}, { 6, 0}, { 7, 0} },
            { { -1, 0}, { -2, 0}, { -3, 0}, {-4, 0}, {-5,0}, {-6,0}, {-7, 0} },
            { { 0, -1}, { 0, -2}, { 0, -3}, {0, -4}, {0,-5}, { 0,-6}, { 0, -7} },
            { { 0, 1}, { 0, 2}, { 0, 3}, {0, 4}, {0,5}, { 0,6}, { 0, 7} }
        };
        public static readonly int[,,] KingMoves =
        {
            { {1, 1} }, { { 1, 0} }, { { 1, -1} }, { { 0, 1 } },
            { {-1, -1} }, { { -1, 0} }, { { -1, 1} }, { { 0, -1 } },
                //Queenside and Kingside castles
            { {-2, 0} }, { { 2, 0 } }
        };
    }
}
