using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.Models
{
    public class KasiskiResult
    {
        public int Id { get; set; }

        [Required]
        public int EncodedTextId { get; set; }
        public virtual EncodedText EncodedTextField { get; set; }

        public virtual IEnumerable<KasiskiResultItem> Results { get; set; }
    }
}
