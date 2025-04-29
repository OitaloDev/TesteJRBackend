using apiToDo.Models;
using apiToDo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace apiToDo.Services
{
    public class TarefaService : ITarefaService
    {
        // Lista estática para simular um "banco de dados" em memória.
        private static List<Tarefa> _tarefas = new List<Tarefa>();

        // Retorna uma cópia da lista para evitar modificações externas indesejadas
        public List<Tarefa> ListarTarefas()
        {
            return _tarefas.ToList();
        }

        // Busca a tarefa pelo ID na lista
        public Tarefa? ObterTarefaPorId(int id)
        {
            return _tarefas.FirstOrDefault(t => t.Id == id);
        }

        // Cria um novo objeto Tarefa a partir do DTO
        public List<Tarefa> InserirTarefa(TarefaDTO novaTarefaDto)
        {
            var novaTarefa = new Tarefa
            {
                Id = novaTarefaDto.Id, // Atribui o próximo ID e incrementa o contador
                Descricao = novaTarefaDto.Descricao,
                Concluida = novaTarefaDto.Concluida,
                DataCriacao = DateTime.UtcNow // Define a data de criação
            };

            // Adiciona a nova tarefa à lista
            _tarefas.Add(novaTarefa);

            // Retorna a lista completa atualizada, conforme solicitado
            return _tarefas.ToList();
        }

        // Busca a tarefa a ser deletada pelo ID
        public List<Tarefa> DeletarTarefa(int id)
        {
            var tarefaParaDeletar = _tarefas.FirstOrDefault(t => t.Id == id);

            // Se a tarefa existir, remove-a da lista
            if (tarefaParaDeletar != null)
            {
                _tarefas.Remove(tarefaParaDeletar);
            }
            // Retorna a lista completa atualizada, conforme solicitado
            return _tarefas.ToList();
        }

        // Busca a tarefa a ser atualizada pelo ID
        public List<Tarefa> AtualizarTarefa(int id, TarefaDTO tarefaAtualizadaDto)
        {
            var tarefaExistente = _tarefas.FirstOrDefault(t => t.Id == id);

            // Se a tarefa existir, atualiza suas propriedades
            if (tarefaExistente != null)
            {
                tarefaExistente.Descricao = tarefaAtualizadaDto.Descricao;

                // Se o status de conclusão mudou para 'true', registra a data/hora.
                // Se mudou para 'false', limpa a data de conclusão.
                if (tarefaAtualizadaDto.Concluida && !tarefaExistente.Concluida)
                {
                    tarefaExistente.DataConclusao = DateTime.UtcNow;
                }
                else if (!tarefaAtualizadaDto.Concluida)
                {
                    tarefaExistente.DataConclusao = null;
                }
                tarefaExistente.Concluida = tarefaAtualizadaDto.Concluida;
            }

            // Retorna a lista completa atualizada, conforme solicitado
            return _tarefas.ToList();
        }
    }
}
