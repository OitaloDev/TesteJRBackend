using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using apiToDo.Services;

namespace apiToDo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "apiToDo", Version = "v1" });
            });

            // Registrar o serviço de Tarefas (Injeção de Dependência)
            // AddScoped: Cria uma instância por requisição HTTP.
            // AddSingleton: Cria uma única instância para toda a aplicação (adequado para lista estática em memória).
            // AddTransient: Cria uma nova instância cada vez que é solicitado.
            services.AddSingleton<ITarefaService, TarefaService>(); // Usando Singleton devido à lista estática
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "apiToDo v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Removido UseAuthorization por enquanto, pois o endpoint lstTarefas original tinha [Authorize]
            // e não há configuração de autenticação no projeto base.
            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
