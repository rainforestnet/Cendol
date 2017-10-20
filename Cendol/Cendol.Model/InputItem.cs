using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cendol.Model
{
    public class InputItem : EntityBase
    {
        [Required]
        public new long Id { get; set; }

        [StringLength(1)]
        public string Status { get; set; }

        [StringLength(100)]
        public string Item { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ProcessedOn { get; set; }
    }
}
