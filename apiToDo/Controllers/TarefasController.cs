using apiToDo.DTO;
using apiToDo.Models;
using apiToDo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic; // Para ActionResult<T>
using System.Linq; // Para usar FirstOrDefault e outros métodos LINQ

namespace apiToDo.Controllers
{
    [ApiController]
    [Route("api/tarefas")] // Rota base mais descritiva (padrão REST)
    public class TarefasController : ControllerBase
    {
        private readonly ITarefaService _tarefaService; // Injetar o serviço

        // Construtor para receber a injeção de dependência
        public TarefasController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        // GET api/tarefas
        // [Authorize] // Removido pois não há setup de autenticação no projeto base
        [HttpGet] // Verbo HTTP correto para listar recursos
        [ProducesResponseType(StatusCodes.Status200OK)] // Documenta o tipo de retorno para o Swagger
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Tarefa>> ListarTarefas() // Usar ActionResult<T> para melhor documentação e type safety
        {
            try
            {
                var tarefas = _tarefaService.ListarTarefas();
                // Retorna 200 OK com a lista de tarefas no corpo da resposta
                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                // Em caso de erro inesperado no servidor logar o erro 
                return StatusCode(StatusCodes.Status500InternalServerError, new { msg = $"Ocorreu um erro interno no servidor: {ex.Message}" });
            }
        }

        // POST api/tarefas
        [HttpPost] // Verbo HTTP correto para criar um novo recurso
        [ProducesResponseType(StatusCodes.Status200OK)] // Conforme solicitado (retornar lista e 200)
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Para erros de validação do DTO
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Tarefa>> InserirTarefa([FromBody] TarefaDTO request)
        {
            // A validação do [FromBody] e [ApiController] já retorna 400 Bad Request
            // se o DTO não for válido (ex: campo 'Descricao' faltando).

            try
            {
                var listaAtualizada = _tarefaService.InserirTarefa(request);

                // Retorna 200 OK com a lista atualizada, conforme instruído.
                return Ok(listaAtualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msg = $"Ocorreu um erro interno ao inserir a tarefa: {ex.Message}" });
            }
        }

        // DELETE api/tarefas/{id}
        // [HttpGet("DeletarTarefa")] // Alterado de GET com Query Param para DELETE com Route Param (padrão REST)
        [HttpDelete("{id}")] // Verbo HTTP correto e parâmetro na rota
        [ProducesResponseType(StatusCodes.Status200OK)] // Conforme solicitado (retornar lista e 200)
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Se a tarefa não for encontrada
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // Renomeado o parâmetro para 'id' para corresponder à rota "{id}"
        public ActionResult<List<Tarefa>> DeletarTarefa([FromRoute] int id) // [FromRoute] para pegar da URL
        {
            // Início do bloco try-catch para tratamento de erros gerais.
            try
            {
                // Tenta encontrar a tarefa com o ID fornecido usando o serviço.
                // Isso verifica se a tarefa existe antes de tentar deletar.
                var tarefaExistente = _tarefaService.ObterTarefaPorId(id);

                // Verifica se a tarefa com o ID especificado foi encontrada.
                if (tarefaExistente == null)
                {
                    // Se a tarefa não foi encontrada (tarefaExistente é null), retorna um status 404 Not Found.
                    // Inclui uma mensagem indicando que a tarefa com aquele ID não existe.
                    // Este é o tratamento de erro solicitado para o caso de tentar deletar um ID inexistente (como 1458, se ele não existir).
                    return NotFound(new { msg = $"Tarefa com ID {id} não encontrada." });
                }

                // Se a tarefa foi encontrada, chama o método do serviço para deletá-la.
                // O método DeletarTarefa do serviço remove a tarefa da lista interna.
                var listaAtualizada = _tarefaService.DeletarTarefa(id);

                // Retorna um status 200 OK com a lista de tarefas atualizada no corpo da resposta,
                // conforme especificado nas instruções do teste.
                return Ok(listaAtualizada);
            }
            // Captura qualquer exceção inesperada que possa ocorrer durante o processo.
            catch (Exception ex)
            {
                // Em caso de erro inesperado no servidor (ex: problema na lógica do serviço),
                // retorna um status 500 Internal Server Error.
                return StatusCode(StatusCodes.Status500InternalServerError, new { msg = $"Ocorreu um erro interno ao deletar a tarefa: {ex.Message}" });
            }
        }

        // PUT api/tarefas/{id}
        [HttpPut("{id}")] // Verbo HTTP correto para atualização completa (ou PATCH para parcial)
        [ProducesResponseType(StatusCodes.Status200OK)] // Conforme solicitado (retornar lista e 200)
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Se a tarefa não for encontrada
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Para erros de validação do DTO
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Tarefa>> AtualizarTarefa([FromRoute] int id, [FromBody] TarefaDTO request)
        {
            try
            {
                // Verifica primeiro se a tarefa existe
                var tarefaExistente = _tarefaService.ObterTarefaPorId(id);
                if (tarefaExistente == null)
                {
                    return NotFound(new { msg = $"Tarefa com ID {id} não encontrada para atualização." });
                }

                // Se existe, atualiza
                var listaAtualizada = _tarefaService.AtualizarTarefa(id, request);

                // Retorna 200 OK com a lista atualizada, conforme instruído.
                return Ok(listaAtualizada);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msg = $"Ocorreu um erro interno ao atualizar a tarefa: {ex.Message}" });
            }
        }

        // GET api/tarefas/{id}
        [HttpGet("{id}")] // Rota específica para buscar um item pelo ID
        [ProducesResponseType(StatusCodes.Status200OK)] // Retorna o objeto encontrado
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Se não encontrar
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Tarefa> ObterTarefaPorId([FromRoute] int id)
        {
            try
            {
                var tarefa = _tarefaService.ObterTarefaPorId(id);

                if (tarefa == null)
                {
                    // Retorna 404 Not Found se a tarefa não existir
                    return NotFound(new { msg = $"Tarefa com ID {id} não encontrada." });
                }

                // Retorna 200 OK com o objeto Tarefa encontrado
                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msg = $"Ocorreu um erro interno ao buscar a tarefa: {ex.Message}" });
            }
        }
    }
}
