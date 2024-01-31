using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    [Table("user")]
    public class User
    {
        [PrimaryKey]
        [Column("id")]
        public int Id { get; set; } // primary key        

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("loggedIn")]
        public string LoggedIn { get; set; }       
    }
}
