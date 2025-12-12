using TransactEase.Models;

namespace TransactEase.DataLayer;

public class AuthDAL
{
    public async Task<UserModel> GetUserByUsernameAsync(string username)
    {
        // This is a placeholder. In a real application, you would query the database.
        // to get the user by their username.
        await Task.CompletedTask;
        return new UserModel { Username = "admin", Password = "password" };
    }
}

public class UserModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
