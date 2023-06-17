using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BootstrapTest.Models
{
    public class TarefaContext : DbContext
    {
        public TarefaContext() : base("DatabaseConnection")
        {

        }
        public DbSet<Tarefa> TarefaTable { get; set; }
    }
}