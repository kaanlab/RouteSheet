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
        High,
        Normal
    }
    
    public class Lesson
    {
        public int Id { get; set; }
        [Required]
        public DateOnly? Date { get; set; }
        [Required]
        public int Hour { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public Priority Prioriy { get; set; }

        //
        public int CadetId { get; set; }
        [Required]
        public Cadet Cadet { get; set; }

        public string AppUserId { get; set; }
        [Required]
        public AppUser AppUser { get; set; }
    }
}
