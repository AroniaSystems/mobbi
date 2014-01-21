using System;

namespace Mobbi.Data
{
    public class IdeasGroup
    {
        public int Id { get; set; }

        public Guid ExternalId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}