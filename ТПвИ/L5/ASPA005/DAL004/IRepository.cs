using System;

namespace DAL004
{
    public interface IRepository : IDisposable
    {
        string BasePath { get; }
        Celebrity[] getAllCelebrities();
        Celebrity? getCelebrityById(int id);
        Celebrity[] getCelebritiesBySurname(string Surname);
        string? getPhotoPathById(int id);

        int? addCelebrity(Celebrity celebrity);
        bool delCelebrityById(int id);
        bool updCelebrityById(int id, Celebrity celebrity);
        int SaveChanges();
    }

    public record Celebrity(int id, string Firstname, string Surname, string PhotoPath);
}
