using System.Windows;
using RIL19.FishAndShark.Windows.Application.ViewModels;

namespace RIL19.FishAndShark.Windows.Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ServicesLocator.GetService<MainWindowViewModel>();
        }

        private void New_OnClick(object sender, RoutedEventArgs e)
        {
            CreateAquarium.Visibility = Visibility.Visible;
            MenuLstAquarium.Visibility = Visibility.Collapsed;
        }

        private void Go_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;

            if (vm?.SelectedAquarium != null)
                MessageBox.Show(this, $"L'aquarium id :{vm.SelectedAquarium?.Id} est selectionné");
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
                vm.RefreshAquariumList();
        }

        private void Create_OnClick(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(txtHeight.Text, out var height))
                MessageBox.Show(this, "Valeur de hauteur invalide");

            if (!double.TryParse(txtWidth.Text, out var width))
                MessageBox.Show(this, "Valeur de largeur invalide");

            if (DataContext is MainWindowViewModel vm)
                vm.CreerAquarium(txtName.Text, height, width);

            CreateAquarium.Visibility = Visibility.Collapsed;
            MenuLstAquarium.Visibility = Visibility.Visible;
        }

        private void CancelCreate_OnClick(object sender, RoutedEventArgs e)
        {
            CreateAquarium.Visibility = Visibility.Collapsed;
            MenuLstAquarium.Visibility = Visibility.Visible;

        }
    }
}
