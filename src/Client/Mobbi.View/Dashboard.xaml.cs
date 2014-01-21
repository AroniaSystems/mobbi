using Windows.UI.Xaml;
using Mobbi.Common;
using Mobbi.Model;
using Mobbi.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Mobbi.View
{
    public sealed partial class Dashboard : Page
    {
        private DashboardViewModel _viewModel = new DashboardViewModel();

        public DashboardViewModel ViewModel
        {
            get { return this._viewModel; }
        }

        public NavigationHelper NavigationHelper { get; private set; }

        public Dashboard()
        {
            this.InitializeComponent();
            this.NavigationHelper = new NavigationHelper(this);
            this.NavigationHelper.LoadState += navigationHelper_LoadState;
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            await _viewModel.Initialise();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        private void CategoryItemClicked(object sender, ItemClickEventArgs e)
        {
            var itemId = ((Category)e.ClickedItem).ExternalId.ToString();
            this.Frame.Navigate(typeof(GroupDetailPage), itemId);
        }

        private void CategoryClicked(object sender, RoutedEventArgs e)
        {
            var category = (sender as FrameworkElement).DataContext;
            this.Frame.Navigate(typeof(GroupDetailPage), ((Category)category).ExternalId.ToString());
        }

        private void IdeaItemClicked(object sender, ItemClickEventArgs e)
        {
            var itemId = ((Idea)e.ClickedItem).ExternalId.ToString();
            this.Frame.Navigate(typeof(IdeaDetailsPage), itemId);
        }

        private void AllCategoriesClicked(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(AllGroupsPage), null);
        }
    }
}
