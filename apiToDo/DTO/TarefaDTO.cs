using System.ComponentModel.DataAnnotations; // Para validações

namespace apiToDo.DTO
{
    public class TarefaDTO
    {
        // O ID é útil para identificar a tarefa em operações de GET, PUT, DELETE.
        // Para POST (criação), ele geralmente é ignorado ou não enviado pelo cliente.
        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição da tarefa é obrigatória.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "A descrição deve ter entre 3 e 200 caracteres.")]
        public string? Descricao { get; set; }

        // Adicionado para permitir marcar tarefas como concluídas (útil para Update/Get)
        public bool Concluida { get; set; } = false; // Valor padrão
    }
}
