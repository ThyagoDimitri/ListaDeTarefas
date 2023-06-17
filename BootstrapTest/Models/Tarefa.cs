using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BootstrapTest.Models
{
    [Table("TAREFA_TAR")]
    public class Tarefa
    {
        [Key]
        [Display(Name = "ID da Tarefa")]
        public int TAR_ID { get; set; }
        [Display(Name = "ID do Usuário")]
        public int TAR_USUID { get; set; }
        [Display(Name = "Descrição da Tarefa")]
        public string TAR_DESCRICAO { get; set; }
        [Display(Name = "Concluída?")]
        public bool TAR_CONCLUIDA { get; set; }
    }
}