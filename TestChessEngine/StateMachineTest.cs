using ChessEngine;

namespace TestChessEngine
{
    public class StateMachineTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FillFromFEN_Test()
        {
            var sm = new StateMachine();
            sm.FillFromFEN();

        }
    }
}