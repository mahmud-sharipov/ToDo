using ToDo.Shared;

namespace Todo.WebUI.Services;

public class ToDoService
{
    private readonly HttpClient _httpClient;

    public ToDoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TodoItem[]> Get(bool? completed)
    {
        if (completed is null)
            return await _httpClient.GetFromJsonAsync<TodoItem[]>("todos");
        else
            return await _httpClient.GetFromJsonAsync<TodoItem[]>("todos?completed=" + completed.Value);
    }

    public async Task Create(TodoItem todoItem)
    {
        var responce = await _httpClient.PostAsJsonAsync("todos", todoItem);
        responce.EnsureSuccessStatusCode();
    }

    public async Task TagleTodoStatus(TodoItem todoItem)
    {
        todoItem.IsComplete = !todoItem.IsComplete;
        var responce = await _httpClient.PutAsJsonAsync("todos/" + todoItem.Id.ToString(), todoItem);
        responce.EnsureSuccessStatusCode();
    }

    public async Task Delete(TodoItem todoItem)
    {
        var responce = await _httpClient.DeleteAsync("todos/" + todoItem.Id.ToString());
        responce.EnsureSuccessStatusCode();
    }
}