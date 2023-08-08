using Octokit;

namespace Apps.Github.Utils;

public static class Pagination
{
    public static async Task<List<T>> Paginate<T>(Func<ApiOptions, Task<IReadOnlyList<T>>> request)
    {
        var result = new List<T>();
        IReadOnlyList<T> response;
        var options = new ApiOptions
        {
            StartPage = 1
        };

        do
        {
            response = await request(options);
            options.StartPage++;

            result.AddRange(response);
        } while (response.Any());

        return result;
    }
}