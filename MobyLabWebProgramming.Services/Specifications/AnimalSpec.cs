using Ardalis.Specification;
using MobyLabWebProgramming.Database.Repository.Entities;

namespace MobyLabWebProgramming.Services.Specifications;

public class AnimalSpec : Specification<Animal>
{
    public AnimalSpec(Guid id) => Query.Where(e => e.Id == id);
}