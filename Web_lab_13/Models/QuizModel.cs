using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace Web_lab_13.Models
{
    public class QuizModel
    {
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();

        public int CurrentQuestionIndex { get; set; } = 0; // Индекс текущего вопроса

        [Required(ErrorMessage = "Input an answer")]
        public string SelectedAnswer { get; set; }

        public string SerializedModel { get; set; }
        
    }

    public class QuestionModel
    {
        public string QuestionText { get; set; }

        public string SelectedAnswer { get; set; }

        public int CorrectAnswer { get; set; } // Для хранения правильного ответа
    }
}