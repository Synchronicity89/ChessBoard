using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text; 
using System.Threading.Tasks;

namespace ChessEngine
{
    public struct Square
    {
        public List<Square> AttackedBy = new List<Square>();
        public List<Square> Attacking = new List<Square>();
        public char Piece = ' ';
        //e4
        public int Xc = -1; //e would be 4
        public int Yr = -1; //4 would actually be 3
         
        public Square()
        {
        }

        public override int GetHashCode()
        {
            return Xc * 10 + Yr;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return GetHashCode().Equals(obj?.GetHashCode());
        }
        public override string ToString() 
        {
            return Piece.ToString().Trim() + "abcdefgh"[Xc] + (Yr + 1).ToString();
        }   

    }


    public struct BoardState
    {
        public Square[,] Squares = new Square[8,8];
        public char MoveSide = 'w';
        public bool CastleWhiteKingside = true;
        public bool CastleWhiteQueenside = true;
        public bool CastleBlackKingside = true;
        public bool CastleBlackQueenside = true;
        public Square? EnPassantSquare = null;

        public BoardState()
        {
        }
    }

    public class StateMachine
    {
        BoardState board = new BoardState();

        public void FillFromFEN(string fen = null)
        {

            if(fen == null)
            {
                fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            }
            int xc = 0;
            int yr = 7;
            var fenSplit = fen.Split(' ');
            foreach(char c in fenSplit[0]) 
            {
                if(yr >= 0)
                { 
                if(Char.IsLetter(c))
                {
                    board.Squares[yr, xc] = new Square { Piece = c, Xc = xc, Yr = yr };
                    xc++;
                } else if(Char.IsNumber(c))
                {
                    xc += int.Parse(c.ToString()) % 8;
                    //if(xc == 8)
                    //{ 
                    //    yr--; 
                    //    xc= 0;
                    //}
                    //else
                    //{
                    //    //TODO: compare with ascii code for 8
                    //    //yr -= int.Parse(c.ToString()) == 8 ? 1 : 0;
                    //}
                }
                else
                {
                    if (c == '/')
                    {
                        yr--;
                        xc = 0;
                    }
                }
                }
            }
            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 8; y++)
                {
                    if(board.Squares[x, y].Piece == ' ')
                    {
                        board.Squares[x, y] = new Square { Xc = x, Yr = y };
                    }
                }
            }

        }
    }
}
