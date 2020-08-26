using System;
using System.Collections.Generic;

namespace gomoku
{
    public class AIPlayer
    {
        private int id;
        private AIMove[] aimoves;
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public AIMove[] AIMoves
        {
            get
            {
                return aimoves;
            }
            set
            {
                aimoves = value;
            }
        }
        public AIPlayer(int id, AIMove[] aimoves)
        {
            ID = id;
            AIMoves = aimoves;
        }
    }
}