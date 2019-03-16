using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Hangman
{
    public partial class MainWindow : Window
    {
        string answer;
        string playerguess;
        enum Difficulty {Easy, Hard}
        Difficulty difficulty = Difficulty.Easy;
        int wrongguesses = 0;
        int playerwins;
        int playerloses;
        string[] easywords;
        string[] hardwords;
        Random rnd = new Random();
        PopupWindow popup = new PopupWindow();
        float windowwidth = 500f;
        float windowheight = 500f;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                easywords = File.ReadAllLines(@"EasyWords.txt"); //Loads words into arrays
                hardwords = File.ReadAllLines(@"HardWords.txt");
            }

            catch
            {
                MessageBox.Show("Error: failed to load word lists. Ensure EassyWords.txt and HardWords.txt are in the same dirrectory as the exe");
            }
           
            StartGame(); //Generates a new word on start
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            SetSizes();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            SetSizes();
            base.OnStateChanged(e);
        }

        public void SetSizes()
        {
            float xscale = (float) Width / windowwidth;
            float yscale = (float) Height / windowheight;
            float xyscale = (float) (((Width / windowwidth) + (Height / windowheight)) / 2);

            SetLabelsSize(xscale, yscale, xyscale);
            SetKeysSize(xscale, yscale, xyscale);

            ScaleObject(null, txtWinLose, null, 143, 50, 36, 175, 10, xscale, yscale, xyscale);
            ScaleObject(null, txtGuess, null, 224, 25, 12, 107, 310, xscale, yscale, xyscale);
            ScaleObject(btnGuess, null, null, 42, 25, 12, 338, 310, xscale, yscale, xyscale);
            ScaleObject(btnEasy, null, null, 92, 25, 12, 150, 433, xscale, yscale, xyscale);
            ScaleObject(btnHard, null, null, 92, 25, 12, 247, 433, xscale, yscale, xyscale);
            ScaleObject(null, null, imgPerson, 273, 198, 0, 107, 58, xscale, yscale, xyscale);
        }

        private void SetLabelsSize(float xscale, float yscale, float xyscale) //Resizes the sizes and fonts of the objects based on the screens starting size devided by the cuyrrent size. It also sets of the possitioning of the objects using nth term formulas 
        {
            grdLabels.Width = 500 * xscale;
            grdLabels.Height = 488 * yscale;

            for (int i = 0; i < grdLabels.Children.Count; i++)
            {
                ((TextBox)grdLabels.Children[i]).Width = 25 * xscale;
                ((TextBox)grdLabels.Children[i]).Height = 25 * yscale;

                ((TextBox)grdLabels.Children[i]).FontSize = 12 * xyscale;

                var margin = ((TextBox)grdLabels.Children[i]).Margin; //Asigns to a var beacuse you cant asign directly

                if (i < 11)
                {
                    margin.Left = ((28 * i) + 95) * xscale; //Places the first 10 labels at the starting number (95) which increments by 28 each time (the distance between the left side of each label) 
                    margin.Top = 246 * yscale;
                }

                else
                {
                    margin.Left = ((28 * (i - 11)) + 95) * xscale; //Places the rest at a lower height and starting at 95 again
                    margin.Top = 274 * yscale;
                }

                ((TextBox)grdLabels.Children[i]).Margin = margin;
            }
        }

        private void SetKeysSize(float xscale, float yscale, float xyscale)
        {
            grdButtons.Width = 492 * xscale;
            grdButtons.Height = 469 * (this.Height / windowheight);

            for (int i = 0; i < grdButtons.Children.Count; i++)
            {
                ((Button)grdButtons.Children[i]).Width = 25 * xscale;
                ((Button)grdButtons.Children[i]).Height = 25 * yscale;

                ((Button)grdButtons.Children[i]).FontSize = 12 * xyscale;

                var margin = ((Button)grdButtons.Children[i]).Margin;

                if (i < 10)
                {
                    margin.Left = ((28 * i) + 105) * xscale;
                    margin.Top = 339 * yscale;
                }

                else if (i < 19)
                {
                    margin.Left = ((28 * (i - 10)) + 119) * xscale;
                    margin.Top = 367 * yscale;
                }

                else
                {
                    margin.Left = ((28 * (i - 19)) + 147) * xscale;
                    margin.Top = 395 * yscale;
                }

                ((Button)grdButtons.Children[i]).Margin = margin;
            }
        }

        void ScaleObject(Button button, TextBox textbox, Image image, int width, int height, int fontsize, int marginleft, int margintop, float xscale, float yscale, float xyscale)
        {
            var margin = txtWinLose.Margin;

            if (button != null)
            {
                button.Width = width * xscale;
                button.Height = height * yscale;
                button.FontSize = fontsize * xyscale;
                margin.Left = marginleft * xscale;
                margin.Top = margintop * yscale;
                button.Margin = margin;
            }

            else if (textbox != null)
            {
                textbox.Width = width * xscale;
                textbox.Height = height * yscale;
                textbox.FontSize = fontsize * xyscale;
                margin.Left = marginleft * xscale;
                margin.Top = margintop * yscale;
                textbox.Margin = margin;
            }

            else if (image != null)
            {
                image.Width = width * xscale;
                image.Height = height * yscale;
                margin.Left = marginleft * xscale;
                margin.Top = margintop * yscale;
                image.Margin = margin;
            }
        }

        private async Task PutTaskDelayanimAsync(int i) //Waits i ms without freezing the program
        {
            await Task.Delay(i);
        }

        async void StartGame() //Starts a new round 
        {
            wrongguesses = 0;
            SetImage();
            await PutTaskDelayanimAsync(50); //Delay so the code dosent disable one of the the letter keys
            txtGuess.Text = "";
            UpdatePlayerScore();
            ClearLabels();

            if (difficulty == Difficulty.Easy) //Selects a word from the easy words
            {
                answer = easywords[rnd.Next(0, easywords.Length)];
            }

            else if (difficulty == Difficulty.Hard) //Selects a word from the hard words
            {
                answer = hardwords[rnd.Next(0, hardwords.Length)];
            }

            for (int i = 0; i < grdLabels.Children.Count; i++) //Goes though the answer boxes and shows/hises the ones according to the length of the word. ie. If the word is 7 letters long it will show 7 boxes
            {
                if (i <= answer.Length - 1)
                {
                    ((TextBox)grdLabels.Children[i]).Visibility = Visibility.Visible;
                }

                else if (i > answer.Length - 1)
                {
                    ((TextBox)grdLabels.Children[i]).Visibility = Visibility.Hidden;
                }
            }
        }

        void PlayerGuessLetter(char c) //Whenever the player clicks one of the guess boxes
        {
            for (int i = 0; i < answer.Length; i++) //Goes through the answer and finds which characters are equal to the players guess. ie. if the player guesses q, every q in the answer will be shown
            {
                if (c == answer[i])
                {
                    ((TextBox)grdLabels.Children[i]).Text = c.ToString();
                }
            }

            if (!answer.Contains(c)) //If the leter guessed dose not exsist within the answe thr amount of wrong guessses go up
            {
                wrongguesses++;
            }

            if (wrongguesses > 10) //If the player guesses wrong too many times they lose
            {
                PlayerLose();
            }

            CheckWinLose(); //Checks the answer the player has guessed (via the buttons) 
            SetImage(); //Changes the image to show how many guesses the player has left
        }

        void CheckWinLose()
        {
            playerguess = "";

            for (int i = 0; i < grdLabels.Children.Count; i++) //Builds a string of the guess from the guessed boxes. ie. if the boxes look like: h_llo world it will return "hllo world"
            {
                if (((TextBox)grdLabels.Children[i]).Text != "")
                {
                    playerguess = playerguess + ((TextBox)grdLabels.Children[i]).Text;
                }
            }

            if (playerguess.ToLower() == answer) //If the built string matches the answer the player wins
            {
                PlayerWin();
            }
        }

        void SetImage()
        {
            string imagepath;
            imagepath = wrongguesses + ".png";
            imgPerson.Source = new BitmapImage(new Uri(@imagepath, UriKind.Relative));
        }

        private void BtnGuess_Click(object sender, RoutedEventArgs e) //When the player guesses a word by typing it in the box this checks if the word matches
        {
            if (txtGuess.Text.ToLower() == answer.ToLower())
            {
                PlayerWin();
            }

            else
            {
                PlayerLose();
            }
        }

        void PlayerWin()
        {
            ShowPopup(true);
            playerwins++;
            StartGame();
        }

        void PlayerLose()
        {
            ShowPopup(false);
            playerloses++;
            StartGame();
        }

        private void ShowPopup(bool win) //Opens a popup window
        {
            popup = new PopupWindow();
            popup.SetVariables(answer);
            popup.SetLabel(win);
            popup.ShowDialog();
        }

        void UpdatePlayerScore()
        {
            txtWinLose.Text = playerwins + ":" + playerloses;
        }

        void ClearLabels() //Resets the guess buttons and labels 
        {
            for (int i = 0; i < grdLabels.Children.Count; i++)
            {
                ((TextBox)grdLabels.Children[i]).Text = "";
            }

            for (int i = 0; i < grdButtons.Children.Count; i++)
            {
                ((Button)grdButtons.Children[i]).IsEnabled = true;
            }
        }

        private void BtnEasy_Click(object sender, RoutedEventArgs e) 
        {
            difficulty = Difficulty.Easy;
            DifficultyChange();
        }

        private void BtnHard_Click(object sender, RoutedEventArgs e)
        {
            difficulty = Difficulty.Hard;
            DifficultyChange();
        }

        private void DifficultyChange() //Toggles which difficulty button is enabled and starts a new game
        {
            btnEasy.IsEnabled = !btnEasy.IsEnabled;
            btnHard.IsEnabled = !btnHard.IsEnabled;
            StartGame();
        }

        #region Input for every key

        private void BtnQ_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('q');
            btnQ.IsEnabled = false;
        }

        private void BtnW_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('w');
            btnW.IsEnabled = false;
        }

        private void BtnE_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('e');
            btnE.IsEnabled = false;
        }

        private void BtnR_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('r');
            btnR.IsEnabled = false;
        }

        private void BtnT_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('t');
            btnT.IsEnabled = false;
        }

        private void BtnY_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('y');
            btnY.IsEnabled = false;
        }

        private void BtnU_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('u');
            btnU.IsEnabled = false;
        }

        private void BtnI_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('i');
            btnI.IsEnabled = false;
        }

        private void BtnO_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('o');
            btnO.IsEnabled = false;
        }

        private void BtnP_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('p');
            btnP.IsEnabled = false;
        }

        private void BtnA_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('a');
            btnA.IsEnabled = false;
        }

        private void BtnS_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('s');
            btnS.IsEnabled = false;
        }

        private void BtnD_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('d');
            btnD.IsEnabled = false;
        }

        private void BtnF_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('f');
            btnF.IsEnabled = false;
        }

        private void BtnG_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('g');
            btnG.IsEnabled = false;
        }

        private void BtnH_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('h');
            btnH.IsEnabled = false;
        }

        private void BtnJ_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('j');
            btnJ.IsEnabled = false;
        }

        private void BtnK_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('k');
            btnK.IsEnabled = false;
        }

        private void BtnL_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('l');
            btnL.IsEnabled = false;
        }

        private void BtnZ_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('z');
            btnZ.IsEnabled = false;
        }

        private void BtnX_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('x');
            btnX.IsEnabled = false;
        }

        private void BtnC_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('c');
            btnC.IsEnabled = false;
        }

        private void BtnV_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('v');
            btnV.IsEnabled = false;
        }

        private void BtnB_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('b');
            btnB.IsEnabled = false;
        }

        private void BtnN_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('n');
            btnN.IsEnabled = false;
        }

        private void BtnM_Click(object sender, RoutedEventArgs e)
        {
            PlayerGuessLetter('m');
            btnM.IsEnabled = false;
        }

        #endregion
    }
}
