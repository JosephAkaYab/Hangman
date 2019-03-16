using System;
using System.Windows;


namespace Hangman
{
    public partial class PopupWindow : Window
    {
        string answer;

        public PopupWindow()
        {
            InitializeComponent();
        }

        public void SetVariables(string answer)
        {
            this.answer = answer;
        }

        public void SetLabel(bool win)
        {
            switch (win)
            {
                case true:
                    txtMessage.Text = "Well done! The answer was:" + Environment.NewLine + answer;
                    break;

                default:
                    txtMessage.Text = "Hard luck :( The answer was:" + Environment.NewLine + answer;
                    break;
            }
        }

        private void BtnLookUp_Click(object sender, RoutedEventArgs e) //Allows the player to look up the definition for the answe, useful on the hard mode
        {
            System.Diagnostics.Process.Start("https://www.google.com/search?q=define+" + answer);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
