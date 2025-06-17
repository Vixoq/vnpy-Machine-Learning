using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FinanceManager.Models
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public float Balance { get; set; }
    }
}
