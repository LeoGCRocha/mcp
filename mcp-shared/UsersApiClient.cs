using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using mcp_shared.Dtos.Users;

namespace mcp_shared;

public class UsersApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public UsersApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<UserResponse>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _httpClient.GetFromJsonAsync<List<UserResponse>>("/users", JsonOptions, cancellationToken);
        return users ?? [];
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync($"/users/{userId}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<UserResponse>(JsonOptions, cancellationToken);
    }

    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync("/users", request, JsonOptions, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        var user = await response.Content.ReadFromJsonAsync<UserResponse>(JsonOptions, cancellationToken);
        return user ?? throw new InvalidOperationException("The users API returned an empty response when creating a user.");
    }

    public async Task<UserResponse?> UpdateUserAsync(Guid userId, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsJsonAsync($"/users/{userId}", request, JsonOptions, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<UserResponse>(JsonOptions, cancellationToken);
    }

    public async Task<bool> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.DeleteAsync($"/users/{userId}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        await EnsureSuccessAsync(response, cancellationToken);
        return true;
    }

    public async Task<UserNoteResponse?> AddNoteAsync(Guid userId, AddUserNoteRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync($"/users/{userId}/notes", request, JsonOptions, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await EnsureSuccessAsync(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<UserNoteResponse>(JsonOptions, cancellationToken);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new HttpRequestException(
            $"Users API request failed with status code {(int)response.StatusCode}: {body}",
            null,
            response.StatusCode);
    }
}
