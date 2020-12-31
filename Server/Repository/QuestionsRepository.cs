using DatabaseRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseRepository.Repository
{
    public static class QuestionsRepository
    {
        // currently this class only fakes getting question list from database
        public static List<Question> GetQuestions()
        {
            List<Question> questions = new List<Question>();

            Dictionary<int, string> answers = new Dictionary<int, string>();
            int questionsCount = 6;
            int answersCount = 5;
            for (int i = 0; i < answersCount; i++)
            {
                answers.Add(i, "Answer" + i);
            }



            for (int i = 0; i < questionsCount; i++)
            {
                Question question = new Question(
                    Guid.NewGuid().ToString(),
                    "Question" + i,
                    QuestionLevel.VeryEasy,
                    i,
                    answers
                    );

                questions.Add(question);
            }


            return questions;
        }

    }
}
