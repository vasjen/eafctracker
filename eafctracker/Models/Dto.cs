namespace Eafctracker.Models
{
    public record CardCreateRequest(string Name, string Description);
    public record CardUpdateRequest(int Id, string Name, string Description);
}