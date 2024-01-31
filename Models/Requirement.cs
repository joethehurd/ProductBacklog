using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    [Table("requirement")]
    public class Requirement
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public Guid Id { get; set; } // primary key      

        [Column("name")]
        public string Name { get; set; }      

        [Column("type")]
        public string Type { get; set; }

        [Column("description")]
        public string Description { get; set; }
           
        [Indexed]
        [Column("sprintId")]
        public Guid SprintId { get; set; } // foreign key

        [Indexed]
        [Column("userId")]
        public int UserId { get; set; } // foreign key      
    }
}
