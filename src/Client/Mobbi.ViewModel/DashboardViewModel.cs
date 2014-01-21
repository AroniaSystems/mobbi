using System.Linq;
using System.Threading.Tasks;
using Mobbi.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Mobbi.ViewModel
{
    public class DashboardViewModel : INotifyCollectionChanged, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private ICollection<Category> _allCategories = new Collection<Category>();
        private ICollection<CategoryDetails> _categoriesExtended = new Collection<CategoryDetails>();
        private ICollection<Category> _mainCategories = new Collection<Category>();
        private ICollection<Slide> _slides = new Collection<Slide>();

        //TODO: add AddCategory and RemoveCategory functions
        public ICollection<Category> AllCategories
        {
            get
            {
                return _allCategories;
            }
            set
            {
                _allCategories = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("AllCategories"));
            }
        }

        //TODO: check that we have only ExtendedCategoriesNumber extended categories also add Replace/Add/Remove ExtendedCategory functions
        public ICollection<CategoryDetails> CategoriesExtended
        {
            get
            {
                return _categoriesExtended;
            }
            set
            {
                _categoriesExtended = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("CategoriesExtended"));
            }
        }

        //TODO: add AddCategoryToMain and RemoveCategoryFromMain functions
        public ICollection<Category> MainCategories
        {
            get
            {
                return _mainCategories;
            }
            set
            {
                _mainCategories = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("MainCategories"));
            }
        }

        //TODO: add AddSlide and RemoveSlide functions
        public ICollection<Slide> Slides
        {
            get
            {
                return _slides;
            }
            set
            {
                _slides = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Slides"));
                //if (CollectionChanged != null) CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public async Task<bool> Initialise()
        {
            var categories = await DataSource.GetGroupsAsync();
            CategoriesExtended = categories;
            AllCategories = categories.Select(c => new Category() {ExternalId = c.ExternalId, Title = c.Title}).ToList();
            MainCategories = AllCategories.Take(10).ToList();
            Slides = CategoriesExtended.SelectMany(c => c.Items).Select(
                i => new Slide()
                {
                    ExternalId = i.ExternalId,
                    Title = i.Title,
                    Description = i.Description,
                    OwnerName = i.OwnerName,
                    OwnerSurname = i.OwnerSurname
                }).Take(5).ToList();
            return true;
        }
    }
}