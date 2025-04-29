using System; // Para DateTime

namespace apiToDo.Models
{
    // Renomeado de Tarefas para Tarefa
    // Agora é um POCO (Plain Old C# Object) representando a entidade
    public class Tarefa
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public bool Concluida { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataConclusao { get; set; } // Nulável, preenchido quando Concluida = true
    }
}
