using System;

namespace Mobbi.Model
{
    public class Slide
    {
        public Guid ExternalId { get; set; }

        public string OwnerName { get; set; }

        public string OwnerSurname { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}