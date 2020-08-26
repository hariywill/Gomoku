using System;
using System.Collections.Generic;

namespace gomoku
{
    public class Move
    {
        private int row;
        private int column;
        private string usercommand;
        private int playerid;
        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                row = value;
            }
        }
        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
            }
        }
        
        public string UserCommand
        {
            get
            {
                return usercommand;
            }
            set
            {
                usercommand = value;
            }
        }
        public int PlayerID
        {
            get 
            {
                return playerid;
            }
            set
            {
                playerid = value;
            }
        }
        public Move(int row, int column, string usercommand, int playerid)
        {
            Row = row;
            Column = column;
            UserCommand = usercommand;
            PlayerID = playerid;
        }
        public void GetMove()
        {
            Console.Write("Quit the game(q)? or Redo last move(r)? Play(any key) >> ");
            UserCommand = Console.ReadLine();
            string str1 = "row";
            string str2 = "column";
            Row = CheckInputValid(str1);
            Column = CheckInputValid(str2);
        }
        public int CheckInputValid(string str) 
        {
            Console.Write("Player-{0}: place on {1} >> ", PlayerID, str);
            string line = Console.ReadLine();
            int value;
            if (int.TryParse(line, out value))
            {
                value = Convert.ToInt32(line);
                if (value<= 0 || value>16) {
                    Console.WriteLine("Please input a number between 1 and 16.");
                    CheckInputValid(str);
                }
            }
            else
            {
                Console.WriteLine("Please input a number.");
                CheckInputValid(str);
            }
            return value;
        }
        
        public string DisplayMove()
        {
            return("Player ID: " + PlayerID + "; Position: row " + Row + ", column " + Column);
        }

        public string GetUserCommand()
        {
            return UserCommand;
        }
    }
}