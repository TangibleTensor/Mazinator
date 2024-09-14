using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Mazinator.Scripts;

namespace Mazinator
{
    public partial class MainWindow : Window
    {
        // Where the maze will be rendered
        WriteableBitmap bitmap;
        int mazeWidth = 31;
        int mazeHeight = 21;

        Maze mazer;

        // Initializing the Maze and bitmap image
        public MainWindow()
        {
            bitmap = new WriteableBitmap(mazeWidth, mazeHeight, 96, 96, PixelFormats.Bgr32, null);
            mazer = new Maze(mazeWidth, mazeHeight, bitmap, Color.FromRgb(255, 255, 255), Color.FromRgb(10, 10, 30));

            InitializeComponent();
            mazeImage.Source = bitmap;
        }

        #region Events

        private async void GenerateMazeButton_Click(object sender, RoutedEventArgs e)
        {
            bitmap = new WriteableBitmap(mazeWidth, mazeHeight, 96, 96, PixelFormats.Bgr32, null);
            mazer = new Maze(mazeWidth, mazeHeight, bitmap, Color.FromRgb(255, 255, 255), Color.FromRgb(10, 10, 30));
            mazeImage.Source = bitmap;
            await mazer.MakeMaze(animate: animateMazeCheckBox.IsChecked);
        }

        private void SliderWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mazeWidth = (int)e.NewValue * 2 + 1;
            textWidth.Text = "Width: " + ((int)e.NewValue).ToString();
        }

        #endregion

        private void SliderHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mazeHeight = (int)e.NewValue * 2 + 1;
            textHeight.Text = "Height: " + ((int)e.NewValue).ToString();
        }

        private void MazeImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition(mazeImage);

                int x = (int)(pos.X * mazeImage.Source.Width / mazeImage.ActualWidth);
                var y = (int)(pos.Y * mazeImage.Source.Height / mazeImage.ActualHeight);
                if (bitmap.GetPixel(x, y) == mazer.pathColour)
                    bitmap.SetPixel(x, y, Color.FromRgb(122, 150, 233));
            }
           
        }
    }
}
