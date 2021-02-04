using System.Collections.Generic;
using System.Text.Json;
using GraphQL.SystemTextJson;

namespace GraphQL.Tests
{
    public static class JsonStringExtensions
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new ObjectDictionaryConverter(),
                new JsonConverterBigInteger(),
            }
        };
        /// <summary>
        /// Creates an <see cref="ExecutionResult"/> with it's <see cref="ExecutionResult.Data" />
        /// property set to the strongly-typed representation of <paramref name="json"/>.
        /// </summary>
        /// <param name="json">A json representation of the <see cref="ExecutionResult.Data"/> to be set.</param>
        /// <param name="errors">Any errors.</param>
        /// <returns>ExecutionResult.</returns>
        public static ExecutionResult ToExecutionResult(this string json, ExecutionErrors errors = null)
            => new()
            {
                Data = string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<Dictionary<string, object>>(json, _jsonOptions),
                Errors = errors
            };
    }
}
