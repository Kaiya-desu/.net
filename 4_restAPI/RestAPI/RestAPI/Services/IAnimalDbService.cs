using System.Collections.Generic;
using RestAPI.Models;

namespace RestAPI.Services
{
    public interface IAnimalDbService
    {
        public List<Animal> GetAnimalsFromDb(string orderBy);
        public void AddAnimalToDb(Animal animal);
        public void ModifyAnimalInDb(string sIdAnimal, Animal animal);
        public void DeleteAnimalFromDb(string idAnimal);
        public bool ValidateData(Animal animal);
        public bool ValidateId(string sIdAnimal);
        
    }
}