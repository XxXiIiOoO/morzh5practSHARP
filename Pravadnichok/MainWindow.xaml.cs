using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Pravadnichok.ViewModel;

namespace Pravadnichok
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        DirectoryItemViewModel directoryItemViewModel;
        public DirectoryItemViewModel DirectoryItemViewModel  {
            get => directoryItemViewModel;
            set
            {
                if(directoryItemViewModel != value)
                {
                    directoryItemViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DirectoryItemViewModel = new DirectoryItemViewModel();
            fileExplorerContentPresenter.SetBinding(ContentPresenter.ContentProperty, new Binding("DirectoryItemViewModel") { Source = this });
        }

        
        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
