using System;
using System.Collections.Generic;

namespace Mobbi.Data
{
    public class Idea
    {
        public int Id { get; set; }

        public Guid ExternalId { get; set; }

        public Guid OwnerId { get; set; }

        public List<Guid> Groups { get; set; } 

        public string Name { get; set; }

        public string Description { get; set; }

        public IdeaStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}