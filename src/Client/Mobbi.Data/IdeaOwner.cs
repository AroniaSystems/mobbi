using System;

namespace Mobbi.Data
{
    public class IdeaOwner
    {
        public int Id { get; set; }

        public Guid ExternalId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}