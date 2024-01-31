using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    [Table ("product")]
    public class Product
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

        [Column("ownerName")]
        public string OwnerName { get; set; }

        [Column("ownerPhone")]
        public string OwnerPhone { get; set; }

        [Column("ownerEmail")]
        public string OwnerEmail { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("priority")]
        public int Priority { get; set; }

        [Column("alerts")]
        public string Alerts { get; set; }

        [Indexed]
        [Column("userId")]
        public int UserId { get; set; } // foreign key       
    }
}
