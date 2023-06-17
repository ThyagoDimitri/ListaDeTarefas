using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BootstrapTest.Models
{
    [Table("USUARIO_USU")]
    public class Usuario
    {
        [Key]
        [Display(Name = "ID do Usuário")]
        public int USU_ID { get; set; }
        [Display(Name = "Nome do Usuário")]
        public string USU_NOME { get; set; }
        [Display(Name = "E-mail de Contato")]
        public string USU_EMAIL { get; set; }
        [Display(Name = "Nivel de permissão")]
        public string USU_PERMISSAO { get; set; }
        [Display(Name = "Senha")]
        public string USU_SENHA { get; set; }
    }
}