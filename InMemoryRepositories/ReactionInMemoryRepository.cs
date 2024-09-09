﻿using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ReactionInMemoryRepository(List<Reaction> reactions) : IReactionRepository
{
    private List<Reaction> reactions = reactions;

    public Task<Reaction> AddAsync(Reaction reaction)
    {
        reaction.Id = reactions.Any() ? reactions.Max(r => r.Id) + 1 : 1;
        reactions.Add(reaction);
        return Task.FromResult(reaction);
    }

    public Task UpdateAsync(Reaction reaction)
    {
        Reaction? existingReaction = reactions.SingleOrDefault(r => r.Id == reaction.Id);
        if (existingReaction is null)
        {
            throw new InvalidOperationException(
                $"Reaction with ID '{reaction.Id}' does not exist");
        }
        reactions.Remove(existingReaction);
        reactions.Add(reaction);
        
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Reaction? existingReaction = reactions.SingleOrDefault(r => r.Id == id);
        if (existingReaction is null)
        {
            throw new InvalidOperationException(
                $"Reaction with ID '{id}' does not exist");
        }
        reactions.Remove(existingReaction);
        
        return Task.CompletedTask;
    }

    public Task<Reaction> GetSingleAsync(int id)
    {
        Reaction? existingReaction = reactions.SingleOrDefault(r => r.Id == id);
        if (existingReaction is null)
        {
            throw new InvalidOperationException(
                $"Reaction with ID '{id}' does not exist");
        }
        
        return Task.FromResult(existingReaction);
    }

    public IQueryable<Reaction> GetMany()
    {
        return reactions.AsQueryable();
    }
}