using System.Collections.Generic;

namespace GraphQL.Tests_UnitTest;

public interface IExampleAccess
{
    IEnumerable<Book> GetBooks();
}
