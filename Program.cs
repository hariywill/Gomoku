using System;


namespace gomoku
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
            Console.Write("Please choose game mode: human vs human(input 1) or computer vs human(input 2):");
            int mode = Convert.ToInt32(Console.ReadLine());
            if (mode == 1) {
                Console.WriteLine("The player has chosed human vs human mode.");
                Move[] moves1 = new Move[225];
                moves1[0] = new Move(0, 0, "test", 1);
                Move[] moves2 = new Move[225];
                moves2[0] = new Move(0, 0, "test", 2);
                Player player1 = new Player(1, moves1);
                Player player2 = new Player(2, moves2);
                GomokuBoard gomoku = new GomokuBoard(player1, player2);
                gomoku.InitBoard();
                gomoku.DrawBoard();
            } else if (mode == 2) {
                Console.WriteLine("The player has chosed computer vs human mode.");
                Console.Write("Please choose game difficulty: easy(input 1) or hard(input 2):");
                int difficulty = Convert.ToInt32(Console.ReadLine());
                if (difficulty == 1)
                {
                    Console.WriteLine("The player has chosed easy difficulty.");
                    //Current testing place

                }
                else if (mode == 2)
                {
                    Console.WriteLine("The player has chosed hard difficulty.");
                    //Current testing place

                }
            }
            /*
            black: ○
            white: ●
            */
            /*
            Move[] moves = new Move[225];
            moves[0] = new Move(0, 0, "test", 1);
            AIMove[] aimoves = new AIMove[225];
            aimoves[0] = new AIMove(0, 0, 2);
            Player player = new Player(1, moves);
            AIPlayer aiplayer = new AIPlayer(2, aimoves);
            AIBoard aigomoku = new AIBoard(player, aiplayer);
            aigomoku.InitBoard();
            aigomoku.DrawBoard();
            */

            Move[] moves1 = new Move[225];
            moves1[0] = new Move(0, 0, "test", 1);
            Move[] moves2 = new Move[225];
            moves2[0] = new Move(0, 0, "test", 2);
            Player player1 = new Player(1, moves1);
            Player player2 = new Player(2, moves2);
            GomokuBoard gomoku = new GomokuBoard(player1, player2);
            gomoku.InitBoard();
            gomoku.DrawBoard();
            
        }
    }
}
