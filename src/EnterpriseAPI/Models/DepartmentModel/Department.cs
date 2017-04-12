using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.DepartmentModel
{
    public class Department
    {
        [Key]
        public int departmentId { get; set; }
        [Required]
        public string departmentName { get; set; }
        [Required]
        public int offeringId { get; set; }

        public Department() { }
    }
}
