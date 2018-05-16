using System.ComponentModel.DataAnnotations;

namespace DTO.Models
{
    public class KasiskiResultItem
    {
        public int Id { get; set; }

        [Required]
        public int KasiskiResultId { get; set; }
        public virtual KasiskiResult KasiskiResultField { get; set; }

        [Required]
        public int Size { get; set; }
        [Required]
        public double Probability { get; set; }
    }
}
