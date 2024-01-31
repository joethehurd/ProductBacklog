using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    // renamed class from "task" to "sprint" for disambiguation from system.task
    [Table("sprint")]
    public class Sprint
    {        
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public Guid Id { get; set; } // primary key        

        [Column("name")]
        public string Name { get; set; }

        [Column("start")]
        public DateTime Start { get; set; }

        [Column("end")]
        public DateTime End { get; set; }

        [Column("assigneeName")]
        public string AssigneeName { get; set; }

        [Column("assigneePhone")]
        public string AssigneePhone { get; set; }

        [Column("assigneeEmail")]
        public string AssigneeEmail { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("priority")]
        public int Priority { get; set; }

        [Column("alerts")]
        public string Alerts { get; set; }

        [Indexed]
        [Column("productId")]
        public Guid ProductId { get; set; } // foreign key

        [Indexed]
        [Column("userId")]
        public int UserId { get; set; } // foreign key                                                  
    }   
}
