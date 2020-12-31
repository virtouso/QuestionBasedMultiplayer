using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading;
using DatabaseRepository.Models;
using Newtonsoft.Json;

using DatabaseRepository;
namespace SignalRServer.Hubs
{
    public class MainHub : Hub
    {

        #region Users SharedData
        private static Dictionary<string, Match> _matches = new Dictionary<string, Match>();
        private DatabaseRepository.DatabaseRepository _repository = new DatabaseRepository.DatabaseRepository();
        private static string _latestMatchGuid;

        #endregion

        #region Handlers


        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }


        public async Task JoinLobby(string userId)
        {

            if (string.IsNullOrEmpty(_latestMatchGuid) || !_matches.ContainsKey(_latestMatchGuid) || _matches.Count == 0)
            {
                await MakeNewRoomAndJoin(userId);
                return;
            }

            Match existingMatch = _matches[_latestMatchGuid];
            if (existingMatch.IsFull)
            {
                //same as above but could not check at same time
                await MakeNewRoomAndJoin(userId);
                return;
            }




            await JoinExisitingRoom(userId, existingMatch);

            // get first question
            Question question = existingMatch.Questions[existingMatch.CurrentRoundNumber];
            string serializedQuestion = JsonConvert.SerializeObject(question);
            await Clients.Group(existingMatch.Guid).SendAsync("OnRoundStarted", existingMatch.CurrentRoundNumber, serializedQuestion, existingMatch.Player1.Score, existingMatch.Player2.Score);

        }


        public async Task CalculateMyAnswer(string matchGuid, int answerIndex)
        {

            //     int answerIndex = int.Parse(answerIndexx);

            Match playerMatch = _matches[matchGuid];
            bool TurnPlayer2 = false;

            if (playerMatch.CurrentRoundNumber % 2 == 0)
            {
                TurnPlayer2 = true;
                if (Context.ConnectionId != playerMatch.Player2.ConnectionId) return;
            }
            else { if (Context.ConnectionId != playerMatch.Player1.ConnectionId) return; }

            bool answerIsTrue = playerMatch.Questions[playerMatch.CurrentRoundNumber].RightAnswerIndex == answerIndex;
            if (TurnPlayer2)
            {
                if (answerIsTrue) { playerMatch.Player2.Score++; }
            }
            else
            {
                if (answerIsTrue) { playerMatch.Player1.Score++; }
            }


            if (playerMatch.CurrentRoundNumber < playerMatch.Questions.Count - 1)
            {
                playerMatch.CurrentRoundNumber++;

                //todo repetitive. fix it.
                Question question = playerMatch.Questions[playerMatch.CurrentRoundNumber];
                string serializedQuestion = JsonConvert.SerializeObject(question);
                await Clients.Group(playerMatch.Guid).SendAsync("OnRoundStarted", playerMatch.CurrentRoundNumber, serializedQuestion, playerMatch.Player1.Score, playerMatch.Player2.Score);
            }
            else
            {
                //todo: do server side calculation for scores and database stuff 

                await Clients.Group(playerMatch.Guid).SendAsync("OnGameFinished", playerMatch.Player1.Score, playerMatch.Player2.Score);
                // remove match if its possible.
                _matches.Remove(playerMatch.Guid);
            }

        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            // todo: better handling disconnections
            try
            {
                Match damagedmatch = null;
                if (_matches.ContainsKey((string)Context.Items["Match"]))
                {
                    damagedmatch = _matches[(string)Context.Items["Match"]];
                    if (damagedmatch.Player1 != null) Clients.Client(damagedmatch.Player1.ConnectionId).SendAsync("EndMatch", "end it");
                    if (damagedmatch.Player2 != null) Clients.Client(damagedmatch.Player2.ConnectionId).SendAsync("EndMatch", "end it");
                }
            }
            catch (Exception)
            {

                //   throw;
            }

            return base.OnDisconnectedAsync(exception);
        }


        #endregion
        #region Utility

        private async Task MakeNewRoomAndJoin(string userId)
        {
            string newMatchGuid = Guid.NewGuid().ToString();

            _latestMatchGuid = newMatchGuid;

            Match newMatch = new Match();
            newMatch.CurrentRoundNumber = 0;
            newMatch.Questions = _repository.GetQuestions();
            newMatch.Guid = newMatchGuid;
            newMatch.Player1 = new Player(userId, Context.ConnectionId);
            Context.Items["Match"] = newMatchGuid;
            _matches.Add(newMatchGuid, newMatch);
            await Groups.AddToGroupAsync(Context.ConnectionId, newMatchGuid);
            await Clients.Caller.SendAsync("OnJoinedLobby", newMatchGuid, 1);
        }


        private async Task JoinExisitingRoom(string userId, Match latestOpenMatch)
        {
            latestOpenMatch.Player2 = new Player(userId, Context.ConnectionId);
            Context.Items["Match"] = latestOpenMatch.Guid;
            latestOpenMatch.IsFull = true;
            await Groups.AddToGroupAsync(Context.ConnectionId, latestOpenMatch.Guid);
            await Clients.Caller.SendAsync("OnJoinedLobby", _latestMatchGuid, 2);
        }


        #endregion

    }
}
