using System;
using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.Tests_UnitTest;

public class ExampleSchema : Schema
{
    public ExampleSchema(IServiceProvider resolver) : base(resolver)
    {
        Query = resolver.GetRequiredService<ExampleQuery>();
        Initialize();
    }
}
