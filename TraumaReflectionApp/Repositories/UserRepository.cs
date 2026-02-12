using System.Collections.Generic;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using TraumaReflectionApp;
using TraumaReflectionApp.Models;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _connection;

    public UserRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public User GetUserByUsername(string username)
    {
        return _connection.QuerySingleOrDefault<User>(
            "SELECT * FROM Users WHERE Username = @username;",
            new { username });
    }

    public User GetById(int id)
    {
        return _connection.QuerySingleOrDefault<User>(
            "SELECT * FROM Users WHERE UserID = @id;",
            new { id }
        );
    }


    // User GetUserByUsername(string username)
    // {
    //     return GetUserByUsername(username);
    // }
    


    IEnumerable<User> IUserRepository.GetAllUsers()
    {
        return GetAllUsers();
    }

    // void IUserRepository.AddUser(User user)
    // {
    //     AddUser(user);
    // }
    //
    // void IUserRepository.UpdateUser(User user)
    // {
    //     UpdateUser(user);
    // }

    // User IUserRepository.GetUserById(int id)
    // {
    //     return GetUserById(id);
    // }

    public User GetUserByEmail(string email)
    {
        return _connection.QuerySingleOrDefault<User>(
            "SELECT * FROM Users WHERE Email = @email;",
            new { email });
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _connection.Query<User>("SELECT * FROM Users;");
    }

    public void AddUser(User user)
    {
        _connection.Execute(
            @"INSERT INTO Users (Username, Email, PasswordHash)
              VALUES (@Username, @Email, @PasswordHash);",
            user);
    }

    public void UpdateUser(User user)
    {
        _connection.Execute(
            @"UPDATE Users
              SET Username = @Username,
                  Email = @Email,
                  PasswordHash = @PasswordHash
              WHERE UserID = @UserID;",
            user);
    }

    public void DeleteUser(int id)
    {
        _connection.Execute(
            "DELETE FROM Users WHERE UserID = @id;",
            new { id });
    }
}