using Xunit;

namespace GraphQL.Tests_UnitTest
{
    public class GrIDBasicQueryTests : ExampleTestBase<ExampleAccess>
    {
        [Fact]
        public void GetBooks_QueryWithTitle_BooksWithTitle()
        {
            var query = "query {books{title}}";

            var expected = "{\"books\": [{\"title\": \"test\"}]}";

            AssertQuerySuccess(query, expected);
        }
    }
}
