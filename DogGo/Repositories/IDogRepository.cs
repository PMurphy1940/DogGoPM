using DogGo.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IDogRepository
    {
        void AddDog(Dog dog);
        void DeleteDog(int DogId);
        List<Dog> GetAllDogs();
        List<Dog> GetDogByBreed(string breed);
        Dog GetDogById(int id);
        void UpdateDog(Dog dog);
        List<Dog> GetDogsByOwnerId(int ownerId);
    }
}