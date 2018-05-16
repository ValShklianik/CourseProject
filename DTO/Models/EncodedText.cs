using System.ComponentModel.DataAnnotations;

namespace DTO.Models
{
    public class EncodedText
    {
        public int Id { get; set; }

        public int InitialTextId { get; set; }
        public virtual Text InitialText { get; set; }

        [Required]
        public string Value { get; set; }
        public string Keyword { get; set; }
    }
}
