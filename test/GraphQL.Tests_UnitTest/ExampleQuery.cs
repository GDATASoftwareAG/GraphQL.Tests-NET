using GraphQL.Types;

namespace GraphQL.Tests_UnitTest;

public class ExampleQuery : ObjectGraphType<object>
{
    public ExampleQuery(IExampleAccess data)
    {
        Name = "Query";

        Field<ListGraphType<BookType>>("books", resolve: context => data.GetBooks());
    }
}
