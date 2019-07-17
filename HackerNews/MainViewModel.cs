using HackerNews.Models;
using HackerNews.MVVM;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

namespace HackerNews
{
    public class MainViewModel : ViewModel
    {
        readonly IHackerNewsClient _client;

        public MainViewModel(IHackerNewsClient client)
        {
            MVVM.WebBrowserHelper.SetBrowserEmulationMode(11001);
            _client = client;
            Items = new BindingList<Item>();

            LoadHeadlinesCommand = new RelayCommand(async () => await LoadItems(_client.GetTopItems), ()=> AllowLoadingItems);
            LoadIncomingCommand = new RelayCommand(async () => await LoadItems(_client.GetIncomingItems), () => AllowLoadingItems);
            LoadPopularCommand = new RelayCommand(async () => await LoadItems(_client.GetPopularItems), () => AllowLoadingItems);

            TopLevelComments = new BindingList<Item>();
            this.PropertyChanged += MainViewModel_PropertyChanged;
        }

        private async void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SelectedItem))
            {
                await LoadComments();
            }
        }

        public MVVM.RelayCommand LoadHeadlinesCommand { get; private set; }
        public MVVM.RelayCommand LoadIncomingCommand { get; private set; }
        public MVVM.RelayCommand LoadPopularCommand { get; private set; }

        async Task LoadItems(Func<Task<int[]>> load)
        {
            AllowLoadingItems = false;
            Items.Clear();
            var topItems = await load();
            foreach (var item in topItems.Take(10))
            {
                Items.Add(await _client.GetItem(item));
            }
            AllowLoadingItems = true;
        }

        public bool AllowLoadingItems
        {
            get { return _allowLoadingItems; }
            set
            {
                _allowLoadingItems = value;
                NotifyPropChange(nameof(AllowLoadingItems));
                CommandManager.InvalidateRequerySuggested();
            }
        }
        bool _allowLoadingItems = false;

        public Item SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropChange(nameof(SelectedItem));
            }
        }
        private Item _selectedItem;


        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set { _selectedTabIndex = value; NotifyPropChange(nameof(SelectedTabIndex)); }
        }
        private int _selectedTabIndex;


        public string Status
        {
            get { return _status; }
            set { _status = value; NotifyPropChange(nameof(Status)); }
        }
        private string _status;

        async Task LoadComments()
        {
            TopLevelComments.Clear();

            if (SelectedItem == null || SelectedItem.Kids == null) return;
            
            foreach (var comment in SelectedItem.Kids)
            {
                TopLevelComments.Add(await _client.GetItem(comment));
            }
        }

        public BindingList<Item> Items { get; private set; }

        public BindingList<Item> TopLevelComments { get; private set; }
    }
}

