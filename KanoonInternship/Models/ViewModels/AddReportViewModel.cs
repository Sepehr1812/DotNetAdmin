﻿using System;
using System.ComponentModel.DataAnnotations;

namespace KanoonInternship.Models.ViewModels
{
    public class AddReportViewModel
    {
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime EntranceTime { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime ExitTime { get; set; }

        [Required]
        public DateTime Date { get; set; } // time here is always 00:00

        [Required]
        public string Text { get; set; }

    }
}
