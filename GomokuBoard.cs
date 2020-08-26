using System;
using System.Collections.Generic;

namespace gomoku
{
    public class GomokuBoard
    {
        private static int BOARD_SIZE = 16;
        private static int WIN_PIECE = 5;
        
        private string[,] board;
        //
        private int[,] xy = new int[225, 2];
        //
        private int[,] idx = new int[BOARD_SIZE, BOARD_SIZE];

        bool CurrentPlayer = true;
        //count为胜利的步数，每种胜利类型应重置count
        int count = 0;
        //x,y为坐标,q为胜利类型,id来判定已经经历过的点，每种胜利切换时也应该重置count
        int WinnerID;


        //black piece: ○
        private Player player1;

        //white piece: ●
        private Player player2;
        
        public Player Player1
        {
            get
            {
                return player1;
            }
            set
            {
                player1 = value;
            }
        }
        public Player Player2
        {
            get
            {
                return player2;
            }
            set
            {
                player2 = value;
            }
        }
        
        public GomokuBoard(Player player1, Player player2)
        {
            Player1 = player1;
            Player2 = player2;
        }

        public void InitBoard(){
            board = new string[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < BOARD_SIZE; i++) 
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    board[i, j] = "+";
                    idx[i,j] = 0;
                }
            }
        }

        public void DrawBoard(){
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("If you want to quit, please input \"q\" at the \"row\" input and input any number at the \"row\" and \"column\" input.");
            Console.WriteLine("If you want to redo the last move, please input \"r\" at the \"row\" input and input any number at the \"row\" and \"column\" input.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            //Problem: Player2 can't quit && redo not working. Update: problem fixed
            while ((player1.Moves[count].GetUserCommand()!="q") && (player2.Moves[count].GetUserCommand()!="q"))
            {
                int num;
                if (CurrentPlayer) {
                    num = 1;
                } else
                {
                    num = 2;
                }
                Console.WriteLine("Round:" + (count + 1) + " Player " + num + "\'s move.");
                if ((player1.Moves[count].GetUserCommand() == "r") || (player2.Moves[count].GetUserCommand() == "r")) {
                    Redo();

                }
                if (CurrentPlayer)
                {
                    PlacePiece(player1.Moves[count]);
                    WinnerID = player1.ID;
                }
                else
                {
                    PlacePiece(player2.Moves[count]);
                    WinnerID = player2.ID;
                }
                Console.WriteLine("                               1  1  1  1  1  1  1");
                Console.WriteLine("    1  2  3  4  5  6  7  8  9  0  1  2  3  4  5  6");
                Console.WriteLine("   -----------------------------------------------");
                for (int i = 0; i < BOARD_SIZE; i++)
                {
                    if (i < 9)
                    {
                        Console.Write(" {0} ", i + 1);
                    }
                    else
                    {
                        Console.Write("{0} ", i + 1);
                    }
                    for (int j = 0; j < BOARD_SIZE; j++)
                    {
                        Console.Write(" ");
                        Console.Write(board[i, j]);
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("    -----------------------------------------------");
                if (!CurrentPlayer) 
                {
                    if (TestWin(1))
                    {
                        Console.WriteLine("The player 1 has won.");
                        break;
                    }
                } 
                else if (TestWin(2))
                {
                    Console.WriteLine("The player 1 has won.");
                    break;
                }
            }
        }

        public bool CheckEmpty(int row, int col){
            bool Empty;
            if (idx[row, col] == 0) {
                Empty = true;
            } else {
                Empty = false;
            }

            return Empty;
        }
        //use Player.Move as parameter
        public void PlacePiece(Move move){
            move.GetMove();
            if (CheckEmpty(move.Row-1, move.Column-1)) {
                if (CurrentPlayer)
                {
                    //black piece
                    idx[move.Row-1, move.Column-1] = 1;
                    board[move.Row-1, move.Column-1] = "○";
                    CurrentPlayer = !CurrentPlayer;
                }
                else
                {
                    //white piece
                    idx[move.Row-1, move.Column-1] = 2;
                    board[move.Row-1, move.Column-1] = "●";
                    CurrentPlayer = !CurrentPlayer;
                    count++;
                    UpdateMove();
                }
            }
            else
            {
                Console.WriteLine("This point is already taken, please select another point.");
            }
        }

        public void UpdateMove() {
            player1.Moves[count] = new Move(0, 0, "test", 1);
            player2.Moves[count] = new Move(0, 0, "test", 2);
        }

        public void Redo() {
            //Need to solve redo the fisrt move of player problem. 
            if (count < 1)
            {
                Console.WriteLine("This is the first round. There is no move to redo.");
                idx[player1.Moves[count].Row - 1, player1.Moves[count].Column - 1] = 0;
                board[player1.Moves[count].Row - 1, player1.Moves[count].Column - 1] = "+";
                CurrentPlayer = !CurrentPlayer;
                return;
            }
            if (!CurrentPlayer)
            {
                Console.WriteLine("Player 1 redo.");
                Console.WriteLine(player1.Moves[count - 1].Row);
                Console.WriteLine(player1.Moves[count-1].Column);
                idx[player1.Moves[count].Row - 1, player1.Moves[count].Column - 1] = 0;
                board[player1.Moves[count].Row - 1, player1.Moves[count].Column - 1] = "+";
                idx[player1.Moves[count - 1].Row - 1, player1.Moves[count - 1].Column - 1] = 0;
                board[player1.Moves[count - 1].Row - 1, player1.Moves[count - 1].Column - 1] = "+";
                idx[player2.Moves[count - 1].Row - 1, player2.Moves[count - 1].Column - 1] = 0;
                board[player2.Moves[count - 1].Row - 1, player2.Moves[count - 1].Column - 1] = "+";
                player1.Moves[count] = new Move(0, 0, "test", 1);
                player2.Moves[count - 1] = new Move(0, 0, "test", 2);
                player1.Moves[count - 1] = new Move(0, 0, "test", 1);
            }
            else
            {
                Console.WriteLine("Player 2 redo."); 
                Console.WriteLine(player2.Moves[count-1].Row);
                Console.WriteLine(player2.Moves[count-1].Column);
                idx[player2.Moves[count].Row - 1, player2.Moves[count].Column - 1] = 0;
                board[player2.Moves[count].Row - 1, player2.Moves[count].Column - 1] = "+";
                idx[player2.Moves[count - 1].Row - 1, player2.Moves[count - 1].Column - 1] = 0;
                board[player2.Moves[count - 1].Row - 1, player2.Moves[count - 1].Column - 1] = "+";
                idx[player1.Moves[count - 1].Row - 1, player1.Moves[count - 1].Column - 1] = 0;
                board[player1.Moves[count - 1].Row - 1, player1.Moves[count - 1].Column - 1] = "+";
                player2.Moves[count] = new Move(0, 0, "test", 2);
                player1.Moves[count - 1] = new Move(0, 0, "test", 1);
                player2.Moves[count - 1] = new Move(0, 0, "test", 2);
            }
            count--;
            CurrentPlayer = !CurrentPlayer;
            
        }

        public bool TestWin(int target) {
            bool Victory = false;
            if (TestRight(idx, target) || TestDown(idx, target) || TestLowerRight(idx, target) || TestLowerLeft(idx, target)) {
                return !Victory;
            }
            return Victory;
        }

        public bool TestDraw() {
            bool Draw = false;
            for (int i = 0; i < BOARD_SIZE; i++) {
                for (int  j = 0;  j < BOARD_SIZE;  j++)
                {
                    if (idx[i, j] == 0) {
                        return !Draw;
                    }
                }
            }
            return Draw;
        }

        public bool TestRight(int[,] array, int target) {
            bool HasFive = false;
            int count = 0;
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE - (WIN_PIECE - 1); j++)
                {
                    if ((array[i, j] == array[i, j + 1]) && (array[i,j] == target))
                    {
                        count++;
                        
                    }
                    //(WIN_PIECE - 1) = 4, which means needs compare for 4 times to get 5 same pieces in a line.
                    if (count == (WIN_PIECE - 1))
                    {
                        return !HasFive;
                    }
                    
                }
            }
            Console.WriteLine(count);
            return HasFive;
        }

        public bool TestDown(int[,] array, int target) {
            bool HasFive = false;
            int count = 0;
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                for (int i = 0; i < BOARD_SIZE - (WIN_PIECE - 1); i++)
                {
                    if ((array[i, j] == array[i + 1, j]) && (array[i, j] == target))
                    {
                        count++;
                    }
                    if (count == (WIN_PIECE - 1))
                    {
                        return !HasFive;
                    }
                }
            }
            Console.WriteLine(count);
            return HasFive;
        }   

        public bool TestLowerRight(int[,] array, int target) {
            bool HasFive = false;
            int count = 0;
            for (int i = 0; i < BOARD_SIZE - (WIN_PIECE - 1); i++)
            {
                for (int j = 0; j < BOARD_SIZE - (WIN_PIECE - 1); j++)
                {
                    if ((array[i, j] == array[i + 1, j + 1]) && (array[i, j] == target))
                    {
                        count++;
                    }
                    if (count == (WIN_PIECE - 1))
                    {
                        Console.WriteLine(count);
                        return !HasFive;
                    }
                }
            }
            
            return HasFive;
        }

        public bool TestLowerLeft(int[,] array, int target)
        {
            bool HasFive = false;
            int count = 0;
            for (int i = 0; i < BOARD_SIZE - (WIN_PIECE - 1); i++)
            {
                for (int j = (WIN_PIECE - 1); j < BOARD_SIZE; j++)
                {
                    if ((array[i, j] == array[i + 1, j - 1]) && (array[i, j] == target))
                    {
                        count++;
                    }
                    if (count == (WIN_PIECE - 1))
                    {
                        return !HasFive;
                    }
                }
            }
            Console.WriteLine(count);
            return HasFive;
        }
    }
}