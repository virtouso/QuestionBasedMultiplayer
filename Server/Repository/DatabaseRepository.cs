using DatabaseRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseRepository.Repository;
namespace DatabaseRepository
{
    public class DatabaseRepository
    {

        public List<Question> GetQuestions()
        {
            return QuestionsRepository.GetQuestions();
        }



    }
}
