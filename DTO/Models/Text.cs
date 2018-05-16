using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Models
{
    public class Text
    {
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        public virtual IEnumerable<EncodedText> EncodedTexts { get; set; }
    }
}
