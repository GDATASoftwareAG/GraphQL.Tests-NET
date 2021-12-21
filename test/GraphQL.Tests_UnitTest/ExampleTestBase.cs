using GraphQL.Execution;
using GraphQL.Tests;

namespace GraphQL.Tests_UnitTest;

public class ExampleTestBase<T> : QueryTestBase<ExampleSchema, GraphQLDocumentBuilder> where T : IExampleAccess, new()
{
    public ExampleTestBase()
    {
        Services.Singleton<IExampleAccess>(new T());
        Services.Register<ExampleQuery>();
        Services.Register<BookType>();
        Services.Register<AuthorType>();
        Services.Singleton(new ExampleSchema(new FuncServiceProvider(type => Services.Get(type))));
    }
}
