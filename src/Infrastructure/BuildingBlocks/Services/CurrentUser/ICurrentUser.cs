namespace BuildingBlocks.Services.CurrentUser;

public interface ICurrentUser
{
    long Id { get; }
    string UserName { get; }
    string Email { get; }
}
