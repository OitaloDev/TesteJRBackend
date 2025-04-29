using apiToDo.Models;
using apiToDo.DTO;
using System.Collections.Generic;

namespace apiToDo.Services
{
    public interface ITarefaService
    {
        List<Tarefa> ListarTarefas();
        Tarefa? ObterTarefaPorId(int id);
        List<Tarefa> InserirTarefa(TarefaDTO novaTarefaDto);
        List<Tarefa> DeletarTarefa(int id);
        List<Tarefa> AtualizarTarefa(int id, TarefaDTO tarefaAtualizadaDto);
    }
}
