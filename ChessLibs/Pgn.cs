using System.Threading.Tasks.Dataflow;

namespace ChessLibs
{
    public class Pgn
    {
        int numMoves = 0;
        public string PgnText { get; private set; } = "";
        public void RecordMove(string moveText)
        {
            if(string.IsNullOrEmpty(PgnText))
            {
                PgnText = pgnStart.Replace("1. *", "1. " + moveText );
                numMoves++;
                return;
            }
            if (numMoves % 2 == 0)
            {
                PgnText += " " + (1 + (int)(numMoves / 2)) + ". " + moveText;
            }
            else
            {
                PgnText += " " + moveText;
            }

            numMoves++;
        }



        public static readonly string pgnStart = 
@"[Event '']
[Result '*']
[UTCDate '']
[UTCTime '']
[Variant 'Standard']
[ECO '']
[Opening '']

1. *";

    }
}