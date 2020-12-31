using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseRepository.Models
{
    public class Player
    {

        public string UserId;
        public string ConnectionId;
        public int Score;


        public Player(string userId)
        {
            this.UserId = userId;
        }

        public Player(string userId, string connectionId)
        {
            this.UserId = userId;
            this.ConnectionId = connectionId;
        }


    }
}