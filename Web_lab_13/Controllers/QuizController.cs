using Microsoft.AspNetCore.Mvc;
using Web_lab_13.Models;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Web_lab_13.Controllers
{
    public class QuizController : Controller
    {
        private Random _random = new Random();
        public QuizModel model;


        [HttpGet]
        public IActionResult Start()
        {
            QuizModel model;

            model = new QuizModel
            {
                Questions = GenerateQuestions(4),
                CurrentQuestionIndex = 0
            };

            var jsonString2 = JsonConvert.SerializeObject(model);
            HttpContext.Session.SetString("QuizModel", jsonString2);

            return View("Start", model);
        }

        [HttpPost]
        public IActionResult Next(QuizModel model)
        {

            if (HttpContext.Session.TryGetValue("QuizModel", out var sessionData))
            {
                string previousAnsver = model.SelectedAnswer;
                var jsonString = System.Text.Encoding.UTF8.GetString(sessionData);
                model = JsonConvert.DeserializeObject<QuizModel>(jsonString);
                model.Questions[model.CurrentQuestionIndex].SelectedAnswer = previousAnsver;
            }
            
            if (model.CurrentQuestionIndex < model.Questions.Count)
            {
                model.Questions[model.CurrentQuestionIndex].SelectedAnswer = model.Questions[model.CurrentQuestionIndex].SelectedAnswer;
                model.CurrentQuestionIndex++;
            }
            var jsonString2 = JsonConvert.SerializeObject(model);
            HttpContext.Session.SetString("QuizModel", jsonString2);
            if (model.CurrentQuestionIndex >= model.Questions.Count)
            { 
                return View("Results", model);
            }
            return View("Start", model);
        }

        [HttpPost]
        public IActionResult Finish(QuizModel model)
        {
            // Обработать ответы и перейти к результатам
            return RedirectToAction("Results", model);
        }

        public IActionResult Results(QuizModel model)
        {
            if (HttpContext.Session.TryGetValue("QuizModel", out var sessionData))
            {
                string previousAnswer = "null";
                if (model.SelectedAnswer != null && model.SelectedAnswer != "null") {
                    previousAnswer = model.SelectedAnswer;
                }
                var jsonString = System.Text.Encoding.UTF8.GetString(sessionData);
                model = JsonConvert.DeserializeObject<QuizModel>(jsonString);
                if (model.SelectedAnswer != null && previousAnswer != "null")
                {
                    model.Questions[model.CurrentQuestionIndex].SelectedAnswer = previousAnswer;
                }
            }

            return View(model);
        }

        private List<QuestionModel> GenerateQuestions(int count)
        {
            var questions = new List<QuestionModel>();

            for (int i = 0; i < count; i++)
            {
                int number1 = _random.Next(1, 101); // Случайное число от 1 до 100
                int number2 = _random.Next(1, 101); // Случайное число от 1 до 100
                char operation = GetRandomOperation();

                var question = new QuestionModel
                {
                    QuestionText = $"{number1} {operation} {number2} = ",
                    CorrectAnswer = CalculateAnswer(number1, number2, operation),
                    SelectedAnswer = "No answer"
                };

                questions.Add(question);
            }

            return questions;
        }

        private char GetRandomOperation()
        {
            char[] operations = { '+', '-', '*', '/' };
            return operations[_random.Next(operations.Length)];
        }

        private int CalculateAnswer(int number1, int number2, char operation)
        {
            return operation switch
            {
                '+' => number1 + number2,
                '-' => number1 - number2,
                '*' => number1 * number2,
                '/' => number1 / number2,
                _ => 0
            };
        }
    }
}