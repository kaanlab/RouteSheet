using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.Models
{
    public enum Priority
    {
        [Display(Name ="Высокий")]
        High,
        [Display(Name ="Нормальный")]
        Normal
    }

    public class Lesson
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }
        [Required]
        public int Hour { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public Priority Prioriy { get; set; }

        //

        public Cadet Cadet { get; set; }

        public AppUser AppUser { get; set; }
    }
}
