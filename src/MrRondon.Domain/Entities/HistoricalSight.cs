using System.ComponentModel.DataAnnotations;

namespace MrRondon.Domain.Entities
{
    public class HistoricalSight
    {
        [Key]
        public int HistoricalSightId { get; set; }
        public string Name { get; set; }
        public string SightHistory { get; set; }
    }
}