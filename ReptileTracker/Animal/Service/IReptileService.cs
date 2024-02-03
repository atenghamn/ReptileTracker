using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReptileTracker.Animal.Model;
using ReptileTracker.Commons;

namespace ReptileTracker.Animal.Service;

public interface IReptileService
{
    Task<Result<Reptile>> CreateReptile(string name, string species, DateTime birthdate, ReptileType type, string username, CancellationToken ct);
    Task<Result<Reptile>> UpdateReptile(Reptile entity, CancellationToken ct);
    Task<Result<Reptile>> GetReptileById(int id, CancellationToken ct);
    Task<Result<Reptile>> DeleteReptile(int id, CancellationToken ct);
    Task<Result<List<Reptile?>>> GetReptilesByAccount(string username, CancellationToken ct);
}