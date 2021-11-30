using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteSheet.Shared.ViewModels
{
    public class ClassroomViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Classroom ToClassroomModel() => new Classroom
        {
            Id = Id,
            Name = Name
        };
    }
}
