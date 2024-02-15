

namespace HangmanAssignment
{
    public partial class HangmanGamePage : ContentPage
    {
        private List<string> secretWords = new List<string>() { "PlayStation", "Xbox", "PSP", "NintendoSwitch", "GameBoy" };
        private string secretWord;
        private string guessedWord;
        private int turns;
        private int hangmanState;
        private List<char> guessedLetters = new List<char>();

        public HangmanGamePage()
        {
            InitializeComponent();
            StartBounceAnimation();
            ResetGame();
        }

        private async void StartBounceAnimation()
        {
            while (true)
            {
                await ConsoleImage.ScaleTo(1.2, 250);
                await ConsoleImage.ScaleTo(1.0, 250);
                await Task.Delay(2000);
            }
        }

        private void Guess_Clicked(object sender, EventArgs e)
        {
            string guess = Entry.Text.Trim().ToUpper();
            Entry.Text = "";

            if (string.IsNullOrEmpty(guess))
            {
                DisplayAlert("Invalid Guess", "Please enter a letter.", "OK");
                return;
            }

            if (guessedLetters.Contains(guess[0]))
            {
                DisplayAlert("Already Guessed", "You have already guessed this letter :(.", "OK");
                return;
            }

            guessedLetters.Add(guess[0]);

            bool isCorrect = false;
            for (int i = 0; i < secretWord.Length; i++)
            {
                if (secretWord[i] == ' ')
                {
                    continue;
                }

                if (secretWord[i] == guess[0] && guessedWord[i] == '_')
                {
                    guessedWord = guessedWord.Substring(0, i) + secretWord[i] + guessedWord.Substring(i + 1);
                    isCorrect = true;
                }
            }

            if (!isCorrect)
            {
                turns--;
                hangmanState++;
            }

            UpdateUI();

            if (guessedWord.Replace(" ", "").Equals(secretWord.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
            {
                DisplayAlert("Congratulations!", "You won! Collect Your " + guessedWord, "OK");
                ResetGame();
            }
            else if (turns == 0)
            {
                DisplayAlert("Sorry!", "You lost! The word was " + secretWord, "OK");
                ResetGame();
            }
        }


        private void UpdateUI()
        {
            string spacedGuessedWord = string.Join(" ", guessedWord.ToCharArray());
            WordLabel.Text = spacedGuessedWord;

            string imageName = $"hang{hangmanState}.png";
            HangmanImage.Source = imageName;

            if (turns > 0)
            {
                TurnsLabel.Text = "Turns remaining: " + turns;
            }
            else
            {
                TurnsLabel.Text = "Game Over!";
            }
        }

        private void ResetGame()
        {
            secretWord = secretWords[new Random().Next(secretWords.Count)];
            guessedWord = new string('_', secretWord.Length);
            turns = 8;
            hangmanState = 1;
            guessedLetters.Clear();
            UpdateUI();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ResetGame();
        }
    }
}
