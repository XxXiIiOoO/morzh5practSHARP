using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Pravadnichok.FileExplorer;

namespace Pravadnichok.ViewModel
{
    public class DirectoryItemViewModel : BaseViewModel
    {
        #region Variable

        private DirectoryItemType type;
        private string fullPath;
        private ObservableCollection<DirectoryItemViewModel> initialItems;
        private ObservableCollection<DirectoryItemViewModel> children;

        #endregion

        #region Property
        public DirectoryItemType Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    type = value;
                    OnPropertyChanged();
                }

            }
        }
        public string FullPath
        {
            get => fullPath;
            set
            {
                if (fullPath != value)
                {
                    fullPath = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Name
        {
            get => this.type == DirectoryItemType.Drive ? this.fullPath : DirectoryStructure.GetFileFolderName(this.fullPath);
        }
        public string ImageName => Type == DirectoryItemType.Drive ? "drive" : (Type == DirectoryItemType.File ? "file" : (IsExpanded ? "folder-open" : "folder-closed"));
        public ObservableCollection<DirectoryItemViewModel> Children
        {
            get => children;
            private set
            {
                if (children != value)
                {
                    children = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<DirectoryItemViewModel> InitialItems
        {
            get => initialItems;
            set
            {
                if (initialItems != value)
                {
                    initialItems = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool CanExpand
        {
            get
            {
                return this.Type != DirectoryItemType.File;
            }
        }
        public bool IsExpanded
        {
            get
            {
                return this.Children?.Count(f => f != null) > 0;
            }
            set
            {
                // If the UI tells to expand
                if (value == true)
                    // Find all children
                    Expand();
                // If the UI tells to close
                else
                    this.ClearChildren();

                OnPropertyChanged();
            }
        }
        public ICommand ExpandCommand { get; set; }

        #endregion

        #region Method

        public DirectoryItemViewModel()
        {
            var children = DirectoryStructure.GetLogicalDrives();

            // Create the view models from the data
            initialItems = new ObservableCollection<DirectoryItemViewModel>(
                children.Select(item => new DirectoryItemViewModel(item.FullPath, DirectoryItemType.Drive)));
        }
        public DirectoryItemViewModel(string fullPath, DirectoryItemType type)
        {
            this.ExpandCommand = new RelayCommand(Expand);

            this.FullPath = fullPath;
            this.Type = type;

            this.ClearChildren();
        }

        // Removes all children from the list, adding a dummy item to show the expand icon if required
        private void ClearChildren()
        {
            this.Children = new ObservableCollection<DirectoryItemViewModel>();

            // Show the expand arrow if we are not a file
            if (this.Type != DirectoryItemType.File)
                this.Children.Add(null);
        }

        // Expands this directory and finds all children
        private void Expand()
        {
            // cannot expand a file
            if (this.Type == DirectoryItemType.File)
                return;

            // Find all children
            var children = DirectoryStructure.GetDirectoryContents(this.FullPath);
            this.Children = new ObservableCollection<DirectoryItemViewModel>(
                                children.Select(content => new DirectoryItemViewModel(content.FullPath, content.Type)));
        }

        #endregion
    }
}
