using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;

namespace ReptileTracker.Animal.Service;

public interface IReptileService
{
    Result<Reptile> CreateReptile(string name, string species, DateTime birthdate, ReptileType type, int accountId);
    Result<Reptile> UpdateReptile(Reptile entity);
    Result<Reptile> GetReptileById(int id);
    Result<Reptile> DeleteReptile(int id);
    Task<Result<List<Reptile>>> GetReptilesByAccount(int id);
}