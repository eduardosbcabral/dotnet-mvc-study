using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Alura.ListaLeitura.App.Negocio;
using Alura.ListaLeitura.App.Repositorio;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Alura.ListaLeitura.App
{
    public class Startup : IStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            var builder = new RouteBuilder(app);
            var rotas = builder.MapRoute("Livros/ParaLer", LivrosParaLer)
                .MapRoute("Livros/Lendo", LivrosLendo)
                .MapRoute("Livros/Lidos", LivrosLidos)
                .MapRoute("Cadastro/NovoLivro/{nome}/{autor}", NovoLivroParaLer)
                .MapRoute("Livros/Detalhes/{id:int}", ExibeDetalhes)
                .Build();

            app.UseRouter(rotas);
        }

        private Task ExibeDetalhes(HttpContext context)
        {
            int id = Convert.ToInt32(context.GetRouteValue("id"));

            var repo = new LivroRepositorioCSV();
            var livro = repo.Todos.FirstOrDefault(x => x.Id == id);

            return context.Response.WriteAsync(livro.Detalhes());
        }

        public Task NovoLivroParaLer(HttpContext context)
        {
            string titulo = Convert.ToString(context.GetRouteValue("nome"));
            string autor = Convert.ToString(context.GetRouteValue("autor"));
            var livro = new Livro(titulo, autor);
            var repo = new LivroRepositorioCSV();
            repo.Incluir(livro);
            return context.Response.WriteAsync("O livro foi adicionado com sucesso.");
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            return services.BuildServiceProvider();
        }

        public Task Roteamento(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();

            IDictionary<string, RequestDelegate> caminhosAtendidos = new Dictionary<string, RequestDelegate>
            {
                { "/Livros/ParaLer", LivrosParaLer },
                { "/Livros/Lendo", LivrosLendo },
                { "/Livros/Lidos", LivrosLidos }
            };

            string path = context.Request.Path;

            if (caminhosAtendidos.ContainsKey(path))
            {
                RequestDelegate metodo = caminhosAtendidos[path];
                return metodo.Invoke(context);
            }

            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return context.Response.WriteAsync("Caminho inexistente");
        }

        public Task LivrosParaLer(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();

            return context.Response.WriteAsync(_repo.ParaLer.ToString());
        }

        public Task LivrosLendo(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();

            return context.Response.WriteAsync(_repo.Lendo.ToString());
        }

        public Task LivrosLidos(HttpContext context)
        {
            var _repo = new LivroRepositorioCSV();

            return context.Response.WriteAsync(_repo.Lidos.ToString());
        }
    }
}