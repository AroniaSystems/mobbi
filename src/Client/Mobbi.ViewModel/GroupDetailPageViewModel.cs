using Mobbi.Model;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Mobbi.ViewModel
{
    public class GroupDetailPageViewModel : INotifyCollectionChanged, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private CategoryDetails _categoriesDetails = new CategoryDetails();

        //TODO: add methods to change group details
        public CategoryDetails Category
        {
            get
            {
                return _categoriesDetails;
            }
            set
            {
                _categoriesDetails = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Category"));
            }
        }

        public async Task<bool> Initialise(string id)
        {
            var category = await DataSource.GetGroupAsync(id);
            Category = category;
            return true;
        }
    }
}