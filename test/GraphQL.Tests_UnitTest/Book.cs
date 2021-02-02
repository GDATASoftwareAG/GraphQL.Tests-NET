using GraphQL.Types;

namespace GraphQL.Tests_UnitTest
{
    public class Book
    {
        public string Title { get; }

        public Book(string title)
        {
            Title = title;
        }
    }

    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Name = "Book";
            Field(h => h.Title).Description("title");
        }
    }
}
