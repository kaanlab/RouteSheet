using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.Models
{
    public class Cadet
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        //
        [Required]
        public Classroom Classroom { get; set; }

        public IEnumerable<Lesson> Lessons { get; set; }
    }
}
