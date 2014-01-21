using System.Collections;
using System.Collections.Generic;
using Mobbi.Common;
using Mobbi.Model.Enums;
using System;

namespace Mobbi.Model
{
    public class Idea
    {
        public Guid ExternalId { get; set; }

        public string OwnerName { get; set; }

        public string OwnerSurname { get; set; }

        public IEnumerable<Category> Categories { get; set; } 

        public string Title { get; set; }

        public string Description { get; set; }

        public IdeaStatus Status { get; set; }
    }
}