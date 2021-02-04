using System.Threading.Tasks;
using Xunit;

namespace GraphQL.Tests_UnitTest
{
    public class GrIDBasicQueryTests : ExampleTestBase<ExampleAccess>
    {
        [Fact]
        public async Task GetBooks_QueryWithTitle_BooksWithTitle()
        {
            var query = "query {books{title, releaseDate}}";

            var expected = "{\"books\": [{\"title\": \"test\",\"releaseDate\":\"0001-01-01T00:00:00+00:00\"}]}";

            await AssertQuerySuccessAsync(query, expected);
        }
    }
}
