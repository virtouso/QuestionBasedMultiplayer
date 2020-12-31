using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

    public class Question
    {


        public string Guid;
        public string QuestionBody;
        public QuestionLevel QuestionLevel;
        [JsonIgnore] public int RightAnswerIndex;
        public Dictionary<int, string> Choices;



        public Question(string guid, string questionBody,
            QuestionLevel questionLevel, int rightAnswerIndex, Dictionary<int, string> choices)
        {
            this.Guid = guid;
            this.QuestionBody = questionBody;
            this.QuestionLevel = questionLevel;
            this.RightAnswerIndex = rightAnswerIndex;
            this.Choices = choices;
        }


    }






    public enum QuestionLevel
    {
        VeryEasy,
        Easy,
        Medium,
        Hard,
        VeryHard,
        Expert
    }




