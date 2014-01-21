using System.Collections.ObjectModel;

namespace Mobbi.Model
{
    public class CategoryDetails : Category
    {
        public string Description { get; set; }

        public ObservableCollection<Idea> Items { get; set; }
    }
}