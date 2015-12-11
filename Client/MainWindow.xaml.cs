using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Client.Model;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new CurrentState();
        }

        async void GoClicked(object sender, RoutedEventArgs e)
        {
            SetEnabled(false);

            try
            {
                var document = TextBoxCommandText.Document;

                var blocks = document.Blocks.ToList();

                if (!blocks.Any()) return;

                var firstBlock = blocks.First();
                var defaultForeground = firstBlock.Foreground;
                var defaultBackground = firstBlock.Background;

                foreach (var block in blocks)
                {
                    var plainText = GetPlainText(block);
                    var canBeEmphasized = !string.IsNullOrWhiteSpace(plainText);
                    if (!canBeEmphasized) continue;

                    EmphasizeBlock(block, defaultForeground, defaultBackground);

                    ExecuteCommand(plainText);

                    await Task.Delay(400);
                }

                EmphasizeBlock(null, defaultForeground, defaultBackground);
            }
            finally
            {
                SetEnabled(true);
            }
        }

        void ExecuteCommand(string command)
        {
            
        }

        static string GetPlainText(Block block)
        {
            var textElement = block as TextElement;
            if (textElement == null) return null;

            var start = textElement.ContentStart;
            var end = textElement.ContentEnd;

            var textRange = new TextRange(start, end);

            return textRange.Text;
        }

        void SetEnabled(bool isEnabled)
        {
            TextBoxCommandText.IsEnabled = isEnabled;
            ButtonGo.IsEnabled = isEnabled;
        }

        void EmphasizeBlock(Block blockToEmphasize, Brush defaultForeground, Brush defaultBackground)
        {
            foreach (var block in TextBoxCommandText.Document.Blocks)
            {
                if (block == blockToEmphasize)
                {
                    block.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    block.Background = new SolidColorBrush(Color.FromRgb(155, 155, 255));
                }
                else
                {
                    block.Foreground = defaultForeground;
                    block.Background = defaultBackground;
                }
            }
        }
    }
}
