using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseRepository.Models
{
    public class Match
    {
        public string Guid;
        public Player Player1;
        public Player Player2;

        public int RoundsCount;
        public int CurrentRoundNumber;
        public bool IsFull;
        public List<Question> Questions;

    }
}