namespace BuildingBlocks.Services.CurrentUser;

public interface ICurrentUser
{
    long Id { get; }
    string UserName { get; }
    string Email { get; }
}

public sealed class CurrentUser : ICurrentUser
{
    public long Id { get; }
    public string UserName { get; }
    public string Email { get; }
}
