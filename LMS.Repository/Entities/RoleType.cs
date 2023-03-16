using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Entities
{
    [Table("RoleType")]
    public class RoleType
    {
        public int Id { get; set; }
        public string? RoleName { get; set; }
    }
}
