using System;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;

namespace ReptileTracker.Animal.Service;

public interface IReptileService
{
    Result<Reptile> CreateReptile(string name, string species, DateTime birthdate, ReptileType type);
    Result<Reptile> UpdateReptile(Reptile entity);
    Result<Reptile> GetReptileById(int id);
    Result<Reptile> DeleteReptile(int id);
}