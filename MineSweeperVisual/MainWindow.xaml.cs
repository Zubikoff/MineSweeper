using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MineSweeper;

namespace MineSweeperVisual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MineSweeperGame game;

        object clicked;
        bool? LeftButton;

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int w = Int32.Parse(tbWidth.Text);
                int h = Int32.Parse(tbHeight.Text);
                int m = Int32.Parse(tbMines.Text);

                game = new MineSweeperGame(w, h, m);

                CreateField();
            }
            catch
            {
                MessageBox.Show("Error in making a new game!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateField()
        {
            Field.Children.Clear();
            Field.Rows = game.FieldHeight;
            Field.Columns = game.FieldWidth;

            int count = game.FieldHeight * game.FieldWidth;

            for (int i = 0; i < count; i++)
            {
                Button b = new Button
                {
                    Style = this.FindResource("FieldButtonStyle") as Style
                };
                Field.Children.Add(b);
            }
        }

        private void UpdField()
        {
            int i, j;
            for (int k = 0; k < Field.Children.Count; k++)
            {
                i = k % Field.Columns;
                j = k / Field.Columns;
                (Field.Children[k] as Button).Content = game.Field[i, j];
            }
        }

        private void Field_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is Button)
            {
                if (e.Source != clicked) return;

                clicked = null;

                int i = Field.Children.IndexOf(e.Source as UIElement) % Field.Columns;
                int j = Field.Children.IndexOf(e.Source as UIElement) / Field.Columns;

                if (LeftButton == true)
                    game.DemineCell(i, j);

                if (LeftButton == false)
                    game.PutAFlag(i, j);

                UpdField();

                if (game.GameOver)
                {
                    MessageBox.Show("U won the game!", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void Field_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is Button)
            {
                clicked = e.Source;
                LeftButton = null;
                if (e.LeftButton == MouseButtonState.Pressed) LeftButton = true;
                if (e.RightButton == MouseButtonState.Pressed) LeftButton = false;
            }
        }
    }
}
