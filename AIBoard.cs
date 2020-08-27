using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace gomoku
{
    public class AIBoard 
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

        string[] OneStrings = new[] { "20", "02" };


        //black piece: ○
        private Player player;

        //white piece: ●
        private AIPlayer aiplayer;

        public Player Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }
        public AIPlayer AIPlayer
        {
            get
            {
                return aiplayer;
            }
            set
            {
                aiplayer = value;
            }
        }

        public AIBoard(Player player, AIPlayer aiplayer)
        {
            Player = player;
            AIPlayer = aiplayer;
        }

        public void InitBoard()
        {
            board = new string[BOARD_SIZE, BOARD_SIZE];
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    board[i, j] = "+";
                    idx[i, j] = 0;
                }
            }
        }

        public void DrawBoard()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("If you want to quit, please input \"q\" at the \"row\" input and input any number at the \"column\" input.");
            Console.WriteLine("If you want to redo the last move, please input \"r\" at the \"row\" input and input any number at the \"column\" input.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            while ((player.Moves[count].GetUserCommand() != "q"))
            {
                Console.WriteLine("Round: {0}", count+1);
                if ((player.Moves[count].GetUserCommand() == "r"))
                {
                    Redo();
                }
                if (CurrentPlayer)
                {
                    PlacePiece(player.Moves[count]);
                    Console.WriteLine("Player");
                    WinnerID = player.ID;
                }
                else
                {
                    AIPlacePiece();
                    //RandomMove();
                    Console.WriteLine("AIPlayer");
                    WinnerID = aiplayer.ID;
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
                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine();
                        Console.WriteLine("     The player 1 has won.");
                        Console.WriteLine();
                        Console.WriteLine("-----------------------------------");
                        break;
                    }
                }
                else if (TestWin(2))
                {
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("The AI has won.");
                    Console.WriteLine();
                    Console.WriteLine("-----------------------------------");
                    break;
                }
            }

        }
        
        //Maybe because -1
        public bool CheckEmpty(int row, int col)
        {   
            bool Empty = false;
            if (idx[row, col] == 0)
            {
                return !Empty;
            }
            else
            {
                return Empty;
            }
        }
        //use Player.Move as parameter
        public void PlacePiece(Move move)
        {
            move.GetMove();
            if (CheckEmpty(move.Row - 1, move.Column - 1))
            {
                //black piece: player
                idx[move.Row - 1, move.Column - 1] = 1;
                board[move.Row - 1, move.Column - 1] = "○";
                CurrentPlayer = !CurrentPlayer;
            }
            else
            {
                if (!(player.Moves[count].GetUserCommand() != "q"))
                {
                    Console.WriteLine("This point is already taken, please select another point.");
                }
            }
        }

        

        public void UpdateMove()
        {
            player.Moves[count] = new Move(0, 0, "test", 1);
            aiplayer.AIMoves[count] = new AIMove(0, 0, 2);
        }

        public void Redo()
        {
            //Need to solve redo the fisrt move of player problem. 
            if (count < 1)
            {
                Console.WriteLine("This is the first round. There is no move to redo.");
                idx[player.Moves[count].Row - 1, player.Moves[count].Column - 1] = 0;
                board[player.Moves[count].Row - 1, player.Moves[count].Column - 1] = "+";
                CurrentPlayer = !CurrentPlayer;
                return;
            }
            if (!CurrentPlayer)
            {
                Console.WriteLine("Player1 redo succeed.");
                idx[player.Moves[count].Row - 1, player.Moves[count].Column - 1] = 0;
                board[player.Moves[count].Row - 1, player.Moves[count].Column - 1] = "+";
                idx[player.Moves[count - 1].Row - 1, player.Moves[count - 1].Column - 1] = 0;
                board[player.Moves[count - 1].Row - 1, player.Moves[count - 1].Column - 1] = "+";
                idx[aiplayer.AIMoves[count - 1].Row - 1, aiplayer.AIMoves[count - 1].Column - 1] = 0;
                board[aiplayer.AIMoves[count - 1].Row - 1, aiplayer.AIMoves[count - 1].Column - 1] = "+";
                player.Moves[count] = new Move(0, 0, "test", 1);
                aiplayer.AIMoves[count - 1] = new AIMove(0, 0, 2);
                player.Moves[count - 1] = new Move(0, 0, "test", 1);
            }
            count--;
            CurrentPlayer = !CurrentPlayer;
        }

        public bool TestPieceInLine(int pieceNum)
        {
            bool PieceInLine = false;
            if (TestRight(idx, pieceNum) || TestDown(idx, pieceNum) || TestLowerRight(idx, pieceNum) || TestLowerLeft(idx, pieceNum))
            {
                return !PieceInLine;
            }
            return PieceInLine;
        }

        public bool TestWin(int target)
        {
            bool Victory = false;
            if (TestRight(idx, target) || TestDown(idx, target) || TestLowerRight(idx, target) || TestLowerLeft(idx, target))
            {
                return !Victory;
            }
            return Victory;
        }

        public bool TestRight(int[,] array, int target)
        {
            bool HasFive = false;
            int count = 0;
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE - (WIN_PIECE - 1); j++)
                {
                    if ((array[i, j] == array[i, j + 1]) && (array[i, j] == target))
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
            return HasFive;
        }

        public bool TestDown(int[,] array, int target)
        {
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
            return HasFive;
        }

        public bool TestLowerRight(int[,] array, int target)
        {
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
            return HasFive;
        }

    
        public int[] TestNumberInLine(string[] strings) {
            string TempString = "";
            int[] Position = new int[3];
            Random ran = new Random();
            int RanNum = ran.Next(1, 2);
            
            switch (RanNum)
            {
                case 1:
                    //Test right
                    for (int i = 0; i < BOARD_SIZE; i++)
                    {
                        for (int j = 0; j < BOARD_SIZE - (WIN_PIECE - 1); j++)
                        {
                            TempString += idx[i, j];
                        }
                        foreach (string str in strings)
                        {
                            if (TempString.Contains(str))
                            {
                                Position[0] = i;
                                Position[1] = TempString.IndexOf(str) + str.IndexOf("0");
                                Position[2] = Position[1];
                                if (Position[2] > 0)
                                {
                                    return Position;
                                }
                            }
                        }
                        TempString = "";
                    }
                    break;
                case 2:
                    //Test down     Problem
                    for (int j = 0; j < BOARD_SIZE; j++)
                    {
                        for (int i = 0; i < BOARD_SIZE - (WIN_PIECE - 1); i++)
                        {
                            TempString += idx[i, j];
                        }
                        foreach (string str in strings)
                        {
                            if (TempString.Contains(str))
                            {
                                Position[0] = TempString.IndexOf(str) + str.IndexOf("0");
                                Position[1] = j;
                                Position[2] = Position[0];
                                if (Position[2] > 0)
                                {
                                    
                                    return Position;
                                }
                            }
                        }
                        TempString = "";
                    }
                    break;
            }      
            return Position;
        }

        public bool OutOfBoard(int row, int col)
        {
            bool Out = false;
            if (row < 0 || col < 0 || row > 15 || col > 15) {
                return !Out;
            }
            return Out;
        }
        public void RandomMove()
        {
            int[] Position = new int[2];
            Position[0] = player.Moves[count].Row;
            Position[1] = player.Moves[count].Column;
            Random ran = new Random();
            int RanNum = ran.Next(1, 8);
            if (Position[0] == 1) RanNum = 7; 
            if (Position[0] == 16) RanNum = 2;
            if (Position[1] == 1) RanNum = 5;
            if (Position[1] == 16) RanNum = 4;
            switch (RanNum)
            {
                case 1:
                    if (CheckEmpty(Position[0] - 1, Position[1] - 1) && !OutOfBoard(Position[0] - 1, Position[1] - 1)){
                        AIPlayer.AIMoves[count] = new AIMove(Position[0] - 1, Position[1] - 1, 2);
                        AIUpdateMove(AIPlayer.AIMoves[count]);
                    }
                    else
                    {
                        RandomMove();
                    }
                    break;
                case 2:
                    if (CheckEmpty(Position[0] - 1, Position[1]) && !OutOfBoard(Position[0] - 1, Position[1]))
                    {
                        AIPlayer.AIMoves[count] = new AIMove(Position[0] - 1, Position[1], 2);
                        AIUpdateMove(AIPlayer.AIMoves[count]);
                    }
                    else
                    {
                        RandomMove();
                    }
                    break;
                case 3:
                    if (CheckEmpty(Position[0] - 1, Position[1] + 1) && !OutOfBoard(Position[0] - 1, Position[1] + 1))
                    {
                        AIPlayer.AIMoves[count] = new AIMove(Position[0] - 1, Position[1] + 1, 2);
                        AIUpdateMove(AIPlayer.AIMoves[count]);
                    }
                    else
                    {
                        RandomMove();
                    }
                    break;
                case 4:
                    if (CheckEmpty(Position[0], Position[1] - 1) && !OutOfBoard(Position[0], Position[1] - 1))
                    {
                        AIPlayer.AIMoves[count] = new AIMove(Position[0], Position[1] - 1, 2);
                        AIUpdateMove(AIPlayer.AIMoves[count]);
                    }
                    else
                    {
                        RandomMove();
                    }
                    break;
                case 5:
                    if (CheckEmpty(Position[0], Position[1] + 1) && !OutOfBoard(Position[0], Position[1] + 1))
                    {
                        AIPlayer.AIMoves[count] = new AIMove(Position[0], Position[1] + 1, 2);
                        AIUpdateMove(AIPlayer.AIMoves[count]);
                    }
                    else
                    {
                        RandomMove();
                    }
                    break;
                case 6:
                    if (CheckEmpty(Position[0] + 1, Position[1] - 1) && !OutOfBoard(Position[0] + 1, Position[1] - 1))
                    {
                        AIPlayer.AIMoves[count] = new AIMove(Position[0] + 1, Position[1] - 1, 2);
                        AIUpdateMove(AIPlayer.AIMoves[count]);
                    }
                    else
                    {
                        RandomMove();
                    }
                    break;
                case 7:
                    if (CheckEmpty(Position[0] + 1, Position[1]) && !OutOfBoard(Position[0] + 1, Position[1]))
                    {
                        AIPlayer.AIMoves[count] = new AIMove(Position[0] + 1, Position[1], 2);
                        AIUpdateMove(AIPlayer.AIMoves[count]);
                    }
                    else
                    {
                        RandomMove();
                    }
                    break;
                case 8:
                    if (CheckEmpty(Position[0] + 1, Position[1] + 1) && !OutOfBoard(Position[0] + 1, Position[1] + 1))
                    {
                        AIPlayer.AIMoves[count] = new AIMove(Position[0] + 1, Position[1] + 1, 2);
                        AIUpdateMove(AIPlayer.AIMoves[count]);
                    }
                    else
                    {
                        RandomMove();
                    }
                    break;
            }
        }

        public void AIUpdateMove(AIMove move) 
        {   
            idx[move.Row - 1, move.Column - 1] = 2;
            board[move.Row - 1, move.Column - 1] = "●";
            count++;
            UpdateMove();
        }
        public void AIPlacePiece() 
        {
            CurrentPlayer = !CurrentPlayer;
            if (count < 2)
            {
                RandomMove();
                return;
            }
            int[] OnePositions = new int [3];
            OnePositions = TestNumberInLine(OneStrings);
            if (count >= 2 && OnePositions[2] > 0)
            {
                if (CheckEmpty(OnePositions[0] - 1, OnePositions[1] - 1))
                {
                    AIPlayer.AIMoves[count] = new AIMove(OnePositions[0], OnePositions[1], 2);
                    idx[AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1] = 2;
                    board[AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1] = "●";
                    count++;
                    UpdateMove();
                    return;
                }
                else
                {
                    RandomMove();
                }
            }   
        }        
    }
}