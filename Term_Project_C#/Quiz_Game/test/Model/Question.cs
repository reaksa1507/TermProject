using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuizGame.Model;
using System.Threading.Tasks;

namespace QuizGame.Model
{
    public class Question
    {
        public string Text { get; set; }
        public string[] Options { get; set; }
        public int CorrectOption { get; set; }
    }
}
