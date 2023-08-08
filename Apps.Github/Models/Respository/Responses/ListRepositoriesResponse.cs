using Apps.Github.Dtos;

namespace Apps.Github.Models.Respository.Responses;

public record ListRepositoriesResponse(RepositoryDto[] Repositories);