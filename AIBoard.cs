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



        string[] FourStrings = new[] { "22220", "22202", "22022", "20222", "02222" };
        string[] StraightThreeStrings = new[] { "2220", "2202", "2022", "0222" };

        string[] StraightTwoStrings = new[] { "220", "022", "202" };
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
                Console.WriteLine("Player1 redo");
                Console.WriteLine(player.Moves[count - 1].Row);
                Console.WriteLine(player.Moves[count - 1].Column);
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
            else
            {
                Console.WriteLine("aiplayer redo");
                Console.WriteLine(aiplayer.AIMoves[count - 1].Row);
                Console.WriteLine(aiplayer.AIMoves[count - 1].Column);
                idx[aiplayer.AIMoves[count].Row - 1, aiplayer.AIMoves[count].Column - 1] = 0;
                board[aiplayer.AIMoves[count].Row - 1, aiplayer.AIMoves[count].Column - 1] = "+";
                idx[aiplayer.AIMoves[count - 1].Row - 1, aiplayer.AIMoves[count - 1].Column - 1] = 0;
                board[aiplayer.AIMoves[count - 1].Row - 1, aiplayer.AIMoves[count - 1].Column - 1] = "+";
                idx[player.Moves[count - 1].Row - 1, player.Moves[count - 1].Column - 1] = 0;
                board[player.Moves[count - 1].Row - 1, player.Moves[count - 1].Column - 1] = "+";
                aiplayer.AIMoves[count] = new AIMove(0, 0, 2);
                player.Moves[count - 1] = new Move(0, 0, "test", 1);
                aiplayer.AIMoves[count - 1] = new AIMove(0, 0, 2);
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
                        Console.WriteLine("Compare count: " + count);
                        return !HasFive;
                    }

                }
            }
            Console.WriteLine(count);
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
                        Console.WriteLine("array[i, j]" + array[i, j]);
                        Console.WriteLine("array[i, j]" + array[i, j + 1]);
                        Console.WriteLine("count: " + count);
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
                case 3:
                    //Test lower right
                    for (int i = 0; i < BOARD_SIZE - (WIN_PIECE - 1); i++)
                    {
                        for (int j = 0; j < BOARD_SIZE - (WIN_PIECE - 1); j++)
                        {
                            TempString += idx[i, j];
                        }
                        foreach (string str in strings)
                        {
                            if (TempString.Contains(str))
                            {
                                Console.WriteLine("i: " + i);
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
                case 4:
                    //Test lower left
                    for (int i = 0; i < BOARD_SIZE - (WIN_PIECE - 1); i++)
                    {
                        for (int j = (WIN_PIECE - 1); j < BOARD_SIZE; j++)
                        {
                            TempString += idx[i, j];
                        }
                        foreach (string str in strings)
                        {
                            if (TempString.Contains(str))
                            {
                                Console.WriteLine("i: " + i);
                                Position[0] = i;
                                Position[1] = TempString.IndexOf(str) + str.IndexOf("0");
                                Console.WriteLine("AI move: " + Position[0] + Position[1]);
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
            /*
            Console.WriteLine("AI player random move: row "+(AIPlayer.AIMoves[count].Row - 1));
            Console.WriteLine("col "+(AIPlayer.AIMoves[count].Column - 1));
            idx[AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1] = 2;
            Console.WriteLine("-------------------");
            Console.WriteLine("Index was outside the bounds of the array: board[{0}, {1}]", AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1);
            Console.WriteLine("-------------------");
            board[AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1] = "●";
            count++;
            UpdateMove();
            */
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
            /*
            int[] FourPositions = new int[3];
            FourPositions = TestNumberInLine(FourStrings);
            int[] ThreePositions = new int[3];
            ThreePositions = TestNumberInLine(StraightThreeStrings);
            int[] TwoPositions = new int[3];
            TwoPositions = TestNumberInLine(StraightTwoStrings);
            */
            int[] OnePositions = new int [3];
            OnePositions = TestNumberInLine(OneStrings);
            /*
            //"22220", "22202", "22022", "20222", "02222"
            if (count >= 5 && FourPositions[2] > 0)
            {
                AIPlayer.AIMoves[count] = new AIMove(FourPositions[0], FourPositions[1], 2);
                idx[AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1] = 2;
                board[AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1] = "●";
                count++;
                UpdateMove();
                return;

            }
            //"2220", "2202", "2022", "0222"
            if (count >= 4 && ThreePositions[2] > 0)
            {
                AIPlayer.AIMoves[count] = new AIMove(ThreePositions[0], ThreePositions[1], 2);
                idx[AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1] = 2;
                board[AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1] = "●";
                count++;
                UpdateMove();
                return;
            }
            //"220", "022", "202" 
            if (count >= 3 && TwoPositions[2] > 0)
            {
                AIPlayer.AIMoves[count] = new AIMove(TwoPositions[0], TwoPositions[1], 2);
                idx[AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1] = 2;
                board[AIPlayer.AIMoves[count].Row - 1, AIPlayer.AIMoves[count].Column - 1] = "●";
                count++;
                UpdateMove();
                return;
            }
            */
            //"20", "02"
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


        /*
        public string[] PointDirectionCheck(int row, int col, string[,] matrix)
        {
            string[] results = new string[4];
            int a = row;
            int b = col;
            //Array from 4 directions
            //Horizon direction
            string[] horizon = new string[BOARD_SIZE];
            for (var i = 0; i < 15; i++)
                horizon[i] = matrix[row, i];
            //Vertical direction
            string[] vertical = new string[BOARD_SIZE];
            for (var i = 0; i < 15; i++)
                vertical[i] = matrix[i, col];
            //Right top
            string[] rightTop = new string[BOARD_SIZE];
            while (a < 14 && b > 0) { a++; b--; };
            for (var i = 0; a >= 0 && b < 15; i++)
                rightTop[i] = matrix[a--, b++];
            //Right down
            a = row;
            b = col;
            string[] rightDown = new string[BOARD_SIZE];
            while (a > 0 && b > 0) { a--; b--; };
            for (var i = 0; a < 15 && b < 15; i++)
                rightDown[i] = matrix[a++, b++];

            string horizonCheck = 'b' + string.Join("", horizon) + 'b';
            string verticalCheck = 'b' + string.Join("", vertical) + 'b';
            string rightTopCheck = 'b' + string.Join("", rightTop) + 'b';
            string rightDownCheck = 'b' + string.Join("", rightDown) + 'b';
            
            results[0] = horizonCheck;
            results[1] = verticalCheck;
            results[2] = rightTopCheck;
            results[3] = rightDownCheck;

            return results;
        }
        */
        
        
    }
}