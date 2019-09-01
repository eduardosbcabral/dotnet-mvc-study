using Alura.ListaLeitura.App.html;
using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alura.ListaLeitura.App.Logica
{
    public class LivrosController : Controller
    {
        public string Detalhes(int id)
        {
            var repo = new LivroRepositorioCSV();
            var livro = repo.Todos.FirstOrDefault(x => x.Id == id);
            
            return livro.Detalhes();
        }

        public IActionResult ParaLer()
        {
            var repo = new LivroRepositorioCSV();
            ViewBag.Livros = repo.Todos;

            return View("lista");
        }

        public IActionResult Lendo()
        {
            var repo = new LivroRepositorioCSV();

            ViewBag.Livros = repo.Lendo.Livros;

            return View("lista");
        }

        public IActionResult Lidos()
        {
            var repo = new LivroRepositorioCSV();

            ViewBag.Livros = repo.Lidos.Livros;

            return View("lista");
        }

        public string Teste()
        {
            return "nova funcionalidade foi implementada";
        }

    }
}
