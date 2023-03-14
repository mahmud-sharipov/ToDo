global using ToDo.Shared;
using System.Net.Http.Json;

namespace ToDo.Web.Client;

public class TodoClient
{
    private readonly HttpClient _client;
    public TodoClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<TodoItemResponseDto?> AddTodoAsync(string? title, DateTime? dueDate = null)
    {
        if (string.IsNullOrEmpty(title))
        {
            return null;
        }

        TodoItemResponseDto? createdTodo = null;

        var response = await _client.PostAsJsonAsync("todos", new TodoItemRequestDto { Title = title, DueDate = dueDate });

        if (response.IsSuccessStatusCode)
        {
            createdTodo = await response.Content.ReadFromJsonAsync<TodoItemResponseDto>();
        }

        return createdTodo;
    }

    public async Task<bool> UpdateTodoAsync(TodoItemResponseDto todoItemDto)
    {
        var response = await _client.PutAsJsonAsync("todos/" + todoItemDto.Id.ToString(), new TodoItemRequestDto()
        {
            DueDate = todoItemDto.DueDate,
            IsComplete = todoItemDto.IsComplete,
            Title = todoItemDto.Title
        });

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTodoAsync(Guid id)
    {
        var response = await _client.DeleteAsync($"todos/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<List<TodoItemResponseDto>?> GetTodosAsync(bool? isCompleted, bool? hasDueDate, string orderBy = "DueDate", bool orderByDescending = false)
    {
        var uri = $"todos?orderBy={orderBy}&orderByDescending={orderByDescending}";
        if (isCompleted is not null)
            uri += $"&isCompleted=" + isCompleted.Value;
        if (hasDueDate is not null)
            uri += $"&hasDueDate=" + hasDueDate.Value;

        var response = await _client.GetAsync(uri);
        var statusCode = response.StatusCode;
        List<TodoItemResponseDto>? todos = null;

        if (response.IsSuccessStatusCode)
        {
            todos = await response.Content.ReadFromJsonAsync<List<TodoItemResponseDto>>();
        }

        return todos;
    }
}