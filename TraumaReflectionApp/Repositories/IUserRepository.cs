using System.Collections.Generic;
using TraumaReflectionApp;
using TraumaReflectionApp.Models;

public interface IUserRepository
{
    User GetUserByUsername(string username);
    User GetUserByEmail(string email);
    User GetById(int id);
    IEnumerable<User> GetAllUsers();
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
}