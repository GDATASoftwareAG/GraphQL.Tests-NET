using System.Collections.Generic;

namespace GraphQL.Tests_UnitTest
{
    public record ExampleAccess : IExampleAccess
    {
        public IEnumerable<Book> GetBooks()
        {
            return new[]
            {
                new Book("test"),
            };
        }
    }
}
