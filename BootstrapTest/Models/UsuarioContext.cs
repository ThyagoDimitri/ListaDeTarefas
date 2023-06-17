using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BootstrapTest.Models
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext() : base("DatabaseConnection")
        {

        }
        public DbSet<Usuario> UsuarioTable { get; set; }
    }
}