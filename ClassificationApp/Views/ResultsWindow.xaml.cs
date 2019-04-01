using ClassificationApp.ViewModels;
using System.Windows;

namespace ClassificationApp.Views
{
    /// <summary>
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsWindow : Window
    {
        public ResultsWindow(ResultsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
