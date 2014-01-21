using System.IO;
using Mobbi.Model;
using Mobbi.Model.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace Mobbi.ViewModel
{
    public sealed class DataSource
    {
        private static readonly DataSource Instance = new DataSource();

        public ObservableCollection<CategoryDetails> Groups { get; private set; }

        public DataSource()
        {
            Groups = new ObservableCollection<CategoryDetails>();
        }

        public static async Task<ObservableCollection<CategoryDetails>> GetGroupsAsync()
        {
            await Instance.GetSampleDataAsync();

            return Instance.Groups;
        }

        public static async Task<CategoryDetails> GetGroupAsync(string uniqueId)
        {
            await Instance.GetSampleDataAsync();
            var matches = Instance.Groups.Where((group) => group.ExternalId ==new Guid(uniqueId));
            return matches.Count() == 1 ? matches.First() : null;
        }

        public static async Task<Idea> GetItemAsync(string uniqueId)
        {
            await Instance.GetSampleDataAsync();
            var matches = Instance.Groups.SelectMany(group => group.Items).Where((item) => item.ExternalId == new Guid(uniqueId));
            if (!matches.Any())
            {
                return null;
            }
            var idea = matches.First();
            idea.Categories =
                Instance.Groups.Where(g => g.Items.Any(i => i.ExternalId == new Guid(uniqueId)))
                    .Select(g => new Category() {ExternalId = g.ExternalId, Title = g.Title});

            return idea;
        }

        private async Task GetSampleDataAsync()
        {
            if (Groups.Count != 0) return;

            var dataUri = new Uri("ms-appdata:///roaming/DataSample.xml");

            try
            {
                await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            }
            catch (FileNotFoundException exception)
            {
                InitializeRoamingFile().Wait();
            }
            var file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);

            var fileText = await FileIO.ReadTextAsync(file);
            var doc = XDocument.Parse(fileText);

            var groups = doc.Root.Element("Groups").Elements("Group");
            var ideas = doc.Root.Element("Ideas").Elements("Idea");

            foreach (var group in groups)
            {
                var dataGroup = new CategoryDetails()
                {
                    ExternalId = Guid.Parse(group.Attribute("ExternalId").Value),
                    Title = group.Attribute("Title").Value,
                    Description = group.Attribute("Description").Value,
                    Items = new ObservableCollection<Idea>()
                };
                var ideaIds = group.Elements("GroupItem").Select(x => x.Attribute("ExternalId").Value);
                foreach (var idea in ideas.Where(x => ideaIds.Contains(x.Attribute("ExternalId").Value)))
                {
                    dataGroup.Items.Add(new Idea()
                    {
                        ExternalId = Guid.Parse(idea.Attribute("ExternalId").Value),
                        Title = idea.Attribute("Title").Value,
                        Description = idea.Attribute("Description").Value,
                        OwnerName = idea.Attribute("OwnerName").Value,
                        OwnerSurname = idea.Attribute("OwnerSurname").Value,
                        Status = (IdeaStatus)Enum.Parse(typeof(IdeaStatus), idea.Attribute("Status").Value)
                    });
                }

                this.Groups.Add(dataGroup);
            }
        }

        private async static Task InitializeRoamingFile()
        {
            var sampleDataUri = new Uri("ms-appx:///DataSample.xml");
            var sampleFile = await StorageFile.GetFileFromApplicationUriAsync(sampleDataUri);
            var folder = ApplicationData.Current.RoamingFolder;
            await sampleFile.CopyAsync(folder);
        }

        public async static Task SaveIdeaAsync(Idea idea)
        {
            var dataUri = new Uri("ms-appdata:///roaming/DataSample.xml");

            var file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var fileText = await FileIO.ReadTextAsync(file);
            var doc = XDocument.Parse(fileText);

            var ideas = doc.Root.Element("Ideas").Elements("Idea");
            if (ideas.Any(i => new Guid(i.Attribute("ExternalId").Value) == idea.ExternalId))
            {
                foreach (var i in ideas.Where(i => new Guid(i.Attribute("ExternalId").Value) == idea.ExternalId))
                {
                    i.Attribute("Title").SetValue(idea.Title);
                    i.Attribute("Description").SetValue(idea.Description);
                    i.Attribute("OwnerName").SetValue(idea.OwnerName);
                    i.Attribute("OwnerSurname").SetValue(idea.OwnerSurname);
                    i.Attribute("Status").SetValue(idea.Status.ToString());
                }
            }
            await FileIO.WriteTextAsync(file, doc.ToString());
            Instance.Groups.Clear();
        }

        public async static Task SaveNewIdeaAsync(Idea idea, Guid groupExternalId)
        {
            var dataUri = new Uri("ms-appdata:///roaming/DataSample.xml");

            var file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var fileText = await FileIO.ReadTextAsync(file);
            var doc = XDocument.Parse(fileText);

            var groups = doc.Root.Element("Groups").Elements("Group");

            var ideaGuid = Guid.NewGuid();
            idea.ExternalId = ideaGuid;

            doc.Root.Element("Ideas")
                .Add(new XElement("Idea",
                    new XAttribute("ExternalId", ideaGuid.ToString()),
                    new XAttribute("Title", idea.Title ?? ""),
                    new XAttribute("Description", idea.Description ?? ""),
                    new XAttribute("OwnerName", idea.OwnerName ?? ""),
                    new XAttribute("OwnerSurname", idea.OwnerSurname ?? ""),
                    new XAttribute("Status", IdeaStatus.New.ToString())));

            if (groups.Any(i => new Guid(i.Attribute("ExternalId").Value) == groupExternalId))
            {
                foreach (var g in groups.Where(i => new Guid(i.Attribute("ExternalId").Value) == groupExternalId))
                {
                    g.Add(new XElement("GroupItem", new XAttribute("ExternalId", ideaGuid.ToString())));
                }
            }
            var content = doc.ToString(SaveOptions.DisableFormatting);
            await FileIO.WriteTextAsync(file, content);
            Instance.Groups.Clear();
        }
    }
}