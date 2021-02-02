using System.Collections.Generic;

namespace GraphQL.Tests_UnitTest
{
    public class ExampleAccess : IExampleAccess
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
