using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.ViewModels
{
    public class LessonAddViewModel
    {
        public DateTime? Date { get; set; }
        [Required]
        public int Hour { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public Priority Prioriy { get; set; }

        //
        public int CadetId { get; set; }

        public string AppUserId { get; set; }

    }
}
