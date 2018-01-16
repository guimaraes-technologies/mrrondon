using System;
using Microsoft.Win32;

namespace MrRondon.Domain.Entities
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CellPhone { get; set; }
        public string Telephone { get; set; }

        public Guid UserId { get; set; }
        public Person User { get; set; }
    }
}