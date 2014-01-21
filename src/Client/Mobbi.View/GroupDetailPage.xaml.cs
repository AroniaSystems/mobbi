using Windows.UI.Xaml;
using Mobbi.Common;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Mobbi.Model;
using Mobbi.ViewModel;

namespace Mobbi.View
{
    public sealed partial class GroupDetailPage : Page
    {
        private GroupDetailPageViewModel _viewModel = new GroupDetailPageViewModel();

        public GroupDetailPageViewModel ViewModel
        {
            get { return this._viewModel; }
        }

        public NavigationHelper NavigationHelper { get; private set; }


        public GroupDetailPage()
        {
            this.InitializeComponent();
            this.NavigationHelper = new NavigationHelper(this);
            this.NavigationHelper.LoadState += navigationHelper_LoadState;
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            await _viewModel.Initialise((String)e.NavigationParameter);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        private void IdeaItemClicked(object sender, ItemClickEventArgs e)
        {
            var itemId = ((Idea)e.ClickedItem).ExternalId.ToString();
            this.Frame.Navigate(typeof(IdeaDetailsPage), itemId);
        }

        private void AddNewIdeaClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditIdeaDetailsPage), this.ViewModel.Category.ExternalId.ToString());
        }
    }
}
