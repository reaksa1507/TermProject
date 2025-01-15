using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Controls;
using System.Linq;
using QuizGame.Model;
using System.Windows.Media.Effects;
using System.Windows.Media;

namespace QuizGame
{
    public partial class MainWindow : Window
    {
        private List<Question> questions;
        private int currentQuestionIndex = 0;
        private DispatcherTimer timer;
        private int timeRemaining = 15;
        private int score = 0;
        private List<bool> answerResults;

        public MainWindow()
        {
            InitializeComponent();
            LoadQuestions();
            InitializeTimer();
            DisplayQuestion();
            answerResults = new List<bool>();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void LoadQuestions()
        {
            var fileManager = new FileManager();
            var allQuestions = fileManager.Read<List<Question>>("questions");

            if (allQuestions != null && allQuestions.Count >= 20)
            {
                Random random = new Random();
                questions = allQuestions.OrderBy(q => random.Next()).Take(10).ToList();
            }
            else
            {
                MessageBox.Show("Not enough questions available.");
                questions = new List<Question>();
            }
        }

        private void DisplayQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                var question = questions[currentQuestionIndex];
                QuestionText.Text = question.Text;
                OptionsPanel.Children.Clear();

                for (int i = 0; i < question.Options.Length; i++)
                {
                    var button = new Button
                    {
                        Content = question.Options[i],
                        Width = 500,
                        Height = 50,
                        FontSize = 25,
                        Background = new SolidColorBrush(Colors.MidnightBlue),
                        Foreground = new SolidColorBrush(Colors.White),
                        BorderBrush = new SolidColorBrush(Colors.White),
                        Margin = new Thickness(0, 20, 0, 20)
                    };
                    button.Click += OptionButton_Click;
                    OptionsPanel.Children.Add(button);
                }

                timeRemaining = 15;
                TimerText.Text = $"Time remaining: {timeRemaining}s";
                TimerProgressBar.Value = timeRemaining;
                timer.Start();
            }
            else
            {
                EndQuiz();
            }
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = sender as Button;
            int selectedOptionIndex = OptionsPanel.Children.IndexOf(selectedButton);
            bool isCorrect = selectedOptionIndex == questions[currentQuestionIndex].CorrectOption;
            answerResults.Add(isCorrect);

            if (isCorrect) score++;

            currentQuestionIndex++;
            if (currentQuestionIndex < questions.Count)
            {
                DisplayQuestion();
            }
            else
            {
                EndQuiz();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeRemaining > 0)
            {
                timeRemaining--;
                TimerText.Text = $"Time remaining: {timeRemaining}s";
                TimerProgressBar.Value = timeRemaining;
            }
            else
            {
                timer.Stop();
                MessageBox.Show("Time's up!");
                answerResults.Add(false);
                currentQuestionIndex++;

                if (currentQuestionIndex < questions.Count)
                {
                    DisplayQuestion();
                }
                else
                {
                    EndQuiz();
                }
            }
        }

        private void EndQuiz()
        {
            QuestionText.Visibility = Visibility.Collapsed;
            OptionsPanel.Children.Clear();
            TimerText.Visibility = Visibility.Collapsed;
            TimerProgressBar.Visibility = Visibility.Collapsed;
            timer.Stop();

            string resultMessage = $"Your score: {score} out of {questions.Count}\n\n";
            for (int i = 0; i < questions.Count; i++)
            {
                resultMessage += $"{questions[i].Text}\nYour Answer: {(i < answerResults.Count && answerResults[i] ? "Correct" : "Incorrect")}\n\n";
            }

            if (score == questions.Count)
            {
                resultMessage += "Excellent! You got all answers right!";
            }
            else if (score >= questions.Count / 2)
            {
                resultMessage += "Good job! You have a solid understanding!";
            }
            else
            {
                resultMessage += "Keep trying! Review the material and take the quiz again!";
            }

            ResultText.Text = resultMessage;
            SaveQuizHistory(score);
            EndScreen.Visibility = Visibility.Visible;
        }

        private void SaveQuizHistory(int score)
        {
            var history = LoadQuizHistory();
            history.Add(new QuizHistoryEntry { Score = score, Date = DateTime.Now });

            var topScores = history.OrderByDescending(h => h.Score).Take(5).ToList();
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Historys");
            Directory.CreateDirectory(directoryPath);
            string filePath = Path.Combine(directoryPath, "quiz_history.json");
            File.WriteAllText(filePath, JsonConvert.SerializeObject(topScores, Formatting.Indented));
        }

        private List<QuizHistoryEntry> LoadQuizHistory()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Historys", "quiz_history.json");

            if (File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject<List<QuizHistoryEntry>>(File.ReadAllText(filePath)) ?? new List<QuizHistoryEntry>();
            }

            return new List<QuizHistoryEntry>();
        }

        private void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex = 0;
            score = 0;
            answerResults.Clear();
            LoadQuestions();
            DisplayQuestion();

            QuestionText.Visibility = Visibility.Visible;
            TimerText.Visibility = Visibility.Visible;
            TimerProgressBar.Visibility = Visibility.Visible;
            EndScreen.Visibility = Visibility.Collapsed;

            timer.Start();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}