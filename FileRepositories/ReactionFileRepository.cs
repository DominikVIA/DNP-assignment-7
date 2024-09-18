using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class ReactionFileRepository : IReactionRepository
{
    private readonly string filePath = "reactions.json";

    public ReactionFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    private async Task DummyData()
    {
        await AddAsync(new Reaction(1, 2, false, DateTime.Now));
        await AddAsync(new Reaction(2, 1, true, DateTime.Now));
        await AddAsync(new Reaction(3, 3, false, DateTime.Now));
        await AddAsync(new Reaction(4, 4, true, DateTime.Now));
    }
    
    public async Task<Reaction> AddAsync(Reaction reaction)
    {
        string reactionsAsJson = await File.ReadAllTextAsync(filePath);
        List<Reaction> reactions = JsonSerializer.Deserialize<List<Reaction>>(reactionsAsJson)!;
        int maxId = reactions.Count > 0 ? reactions.Max(x => x.Id) : 0;
        reaction.Id = maxId + 1;
        reactions.Add(reaction);
        reactionsAsJson = JsonSerializer.Serialize(reactions);
        await File.WriteAllTextAsync(filePath, reactionsAsJson);
        return reaction;
    }

    public async Task UpdateAsync(Reaction reaction)
    {
        string reactionsAsJson = await File.ReadAllTextAsync(filePath);
        List<Reaction> reactions = JsonSerializer.Deserialize<List<Reaction>>(reactionsAsJson)!;
        var temp = reactions.First(u => u.Id == reaction.Id);
        reactions.Remove(temp);
        reactions.Add(reaction);
        reactionsAsJson = JsonSerializer.Serialize(reactions);
        await File.WriteAllTextAsync(filePath, reactionsAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string reactionsAsJson = await File.ReadAllTextAsync(filePath);
        List<Reaction> reactions = JsonSerializer.Deserialize<List<Reaction>>(reactionsAsJson)!;
        var temp = reactions.First(u => u.Id == id);
        reactions.Remove(temp);
        reactionsAsJson = JsonSerializer.Serialize(reactions);
        await File.WriteAllTextAsync(filePath, reactionsAsJson);
    }

    public async Task<Reaction> GetSingleAsync(int id)
    {
        string reactionsAsJson = await File.ReadAllTextAsync(filePath);
        List<Reaction> reactions = JsonSerializer.Deserialize<List<Reaction>>(reactionsAsJson)!;
        var temp = reactions.First(u => u.Id == id);
        return temp;
    }

    public IQueryable<Reaction> GetMany()
    {
        string reactionsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Reaction> reactions = JsonSerializer.Deserialize<List<Reaction>>(reactionsAsJson)!;
        return reactions.AsQueryable();
    }
}