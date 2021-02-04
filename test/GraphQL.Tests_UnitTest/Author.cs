using GraphQL.Types;

namespace GraphQL.Tests_UnitTest
{
    public record Author
    {
    }

    public class AuthorType : ObjectGraphType<Author>
    {
    }
}
