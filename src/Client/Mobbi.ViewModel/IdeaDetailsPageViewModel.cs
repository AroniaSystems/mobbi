using System;
using Mobbi.Common;
using Mobbi.Model;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Mobbi.ViewModel
{
    public class IdeaDetailsPageViewModel : INotifyCollectionChanged, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private Idea _idea = new Idea();

        public Idea Idea
        {
            get
            {
                return _idea;
            }
            set
            {
                _idea = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Idea"));
            }
        }

        private Guid _categoryId = Guid.Empty;

        public async Task<bool> Initialise(string id)
        {
            var idea = await DataSource.GetItemAsync(id);
            if (idea == null)
            {
                idea = new Idea();
                _categoryId = new Guid(id);
            }
            Idea = idea;
            return true;
        }

        public virtual void Save()
        {
            if (this.Idea != null)
            {
                if (this._categoryId != Guid.Empty)
                {
                    DataSource.SaveNewIdeaAsync(this.Idea, this._categoryId);
                }
                else
                {
                    DataSource.SaveIdeaAsync(this.Idea);
                }
            }
        }
    }
}