using System;
using System.ComponentModel.DataAnnotations;

namespace KanoonInternship.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Writer { get; set; } // username of the writer

        [Required]
        public DateTime EntranceTime { get; set; }

        [Required]
        public DateTime ExitTime { get; set; }

        [Required]
        public DateTime Date { get; set; } // time here is always 00:00

        [Required]
        public string Text { get; set; }
    }
}
