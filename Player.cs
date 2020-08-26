using System;
using System.Collections.Generic;

namespace gomoku
{
    public class Player
    {
        private int id;
        private Move[] moves;
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
        public Move[] Moves
        {
            get
            {
                return moves;
            }
            set
            {
                moves = value;
            }
        }
        public Player(int id, Move[] moves) 
        {
            ID = id;
            Moves = moves;
        }
    }
}