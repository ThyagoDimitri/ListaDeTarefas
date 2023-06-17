using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BootstrapTest.Models;
using System.Data.SqlClient;
using System.Data;
using System.Net;

namespace BootstrapTest.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioContext usuarioData = new UsuarioContext();
        TarefaContext tarefaData = new TarefaContext();
        public static bool isRedirected;

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["usuario"] == null)
            {
                return View();
            }
            else
            {
                return View("Connected");
            }
        }

        [HttpPost]
        public ActionResult SalvarUsuario(FormCollection formCollection)
        {
            Usuario novoUsuario = new Usuario();
            novoUsuario.USU_NOME = formCollection["USU_NOME"];
            novoUsuario.USU_EMAIL = formCollection["USU_EMAIL"];
            novoUsuario.USU_SENHA = formCollection["USU_SENHA"];
            novoUsuario.USU_PERMISSAO = formCollection["USU_PERMISSAO"];

            if (usuarioData.UsuarioTable.Where(u => u.USU_NOME == novoUsuario.USU_NOME).FirstOrDefault() != null)
            {
                ViewBag.Message = "jaCadastrado";
                return View("Usuario");
            }
            else
            {
                if (String.IsNullOrEmpty(novoUsuario.USU_NOME) || String.IsNullOrEmpty(novoUsuario.USU_SENHA) || String.IsNullOrEmpty(novoUsuario.USU_PERMISSAO))
                {
                    ViewBag.Message = "campoEmBranco";
                    return View("Usuario");
                }
                usuarioData.UsuarioTable.Add(novoUsuario);
                usuarioData.SaveChanges();
                return View("Connected");
            }
        }

        [HttpPost]
        public ActionResult SalvarTarefa(FormCollection formCollection)
        {
            string usuarioLogado = Session["usuario"].ToString();
            Usuario usuarioInfo = usuarioData.UsuarioTable.Where(u => u.USU_NOME == usuarioLogado).FirstOrDefault();
            if (String.IsNullOrEmpty(formCollection["TAR_DESCRICAO"]))
            {
                UsuarioController.isRedirected = true;
                return RedirectToAction("Tarefa");
            }
            else
            {
                Tarefa novaTarefa = new Tarefa();
                novaTarefa.TAR_USUID = usuarioInfo.USU_ID;
                novaTarefa.TAR_DESCRICAO = formCollection["TAR_DESCRICAO"];
                if (formCollection["TAR_CONCLUIDA"] == "on")
                {
                    novaTarefa.TAR_CONCLUIDA = true;
                }
                else
                {
                    novaTarefa.TAR_CONCLUIDA = false;
                }
                UsuarioController.isRedirected = false;
                tarefaData.TarefaTable.Add(novaTarefa);
                tarefaData.SaveChanges();
                return RedirectToAction("Tarefa");
            }
            
        }   

        [HttpPost]
        public ActionResult MarcarConcluido(bool newValue, int tarID)
        {
            Tarefa tarefaInfo = tarefaData.TarefaTable.Where(t => t.TAR_ID == tarID).FirstOrDefault();

            tarefaInfo.TAR_CONCLUIDA = newValue;
            tarefaData.SaveChanges();
            return RedirectToAction("Tarefa");
        }

        [HttpPost]
        public ActionResult EditarTarefa(FormCollection formCollection)
        {
            if (String.IsNullOrEmpty(formCollection["TAR_DESCRICAO"]))
            {
                UsuarioController.isRedirected = true;
                return RedirectToAction("Tarefa");
            }
            else
            {
                Tarefa tarefaEditada = new Tarefa();
                Tarefa tarefaNoBanco = new Tarefa();
                tarefaEditada.TAR_ID = Int32.Parse(formCollection["TAR_ID"]);

                tarefaNoBanco = tarefaData.TarefaTable.Find(tarefaEditada.TAR_ID);
                UpdateModel(tarefaNoBanco);
                tarefaData.SaveChanges();
                return RedirectToAction("Tarefa");
            }
        }


        [HttpPost]
        public ActionResult DeletarTarefa(FormCollection formCollection)
        {
            Tarefa tarefaNoBanco = new Tarefa();
            int idTarefaDeletada = Int32.Parse(formCollection["TAR_ID"]);

            tarefaNoBanco = tarefaData.TarefaTable.Find(idTarefaDeletada);
            tarefaData.TarefaTable.Remove(tarefaNoBanco);
            tarefaData.SaveChanges();
            return RedirectToAction("Tarefa");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return View("Login");
        }

        public ActionResult Usuario()
        {
            string usuarioLogado = Session["usuario"].ToString();
            Usuario usuarioInfo = usuarioData.UsuarioTable.Where(u => u.USU_NOME == usuarioLogado).FirstOrDefault();
            if(usuarioInfo.USU_PERMISSAO != "admin")
            {
                return View("Connected");
            }
            return View("Usuario");
        }
        public ActionResult Tarefa()
        {

            string usuarioLogado = Session["usuario"].ToString();
            if (UsuarioController.isRedirected)
            {
                ViewBag.Message = "campoEmBranco";
            }

            UsuarioController.isRedirected = false;
            Usuario usuarioInfo = usuarioData.UsuarioTable.Where(u => u.USU_NOME == usuarioLogado).FirstOrDefault();
            return View(tarefaData.TarefaTable.Where(t => t.TAR_USUID == usuarioInfo.USU_ID).ToList());
        }

        [HttpPost]
        public ActionResult Verify(Usuario usuario)
        {

            Usuario credenciais = usuarioData.UsuarioTable.Where(u => u.USU_NOME == usuario.USU_NOME && u.USU_SENHA == usuario.USU_SENHA).FirstOrDefault();
            if (credenciais != null)
            {
                Session["usuario"] = credenciais.USU_NOME;
                Session["permissao"] = credenciais.USU_PERMISSAO;
                return View("Connected");
            }
            else
            {
                return View("Error");
            }
        }
    }
}