using System;
using System.Collections.Generic;

namespace gomoku
{
    public class AIMove
    { 
        private int row;
        private int column;
        private int aicode;
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

        
        public int AICode
        {
            get
            {
                return aicode;
            }
            set
            {
                aicode = value;
            }
        }
        public AIMove(int row, int column, int aicode)
        {
            Row = row;
            Column = column;
            AICode = aicode;
        }
        
        public string DisplayMove()
        {
            return ("AI Code: " + AICode + "; Position: row " + Row + ", column " + Column);
        }
    }
}