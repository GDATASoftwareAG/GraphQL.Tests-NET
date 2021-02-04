using System;
using GraphQL.Types;

namespace GraphQL.Tests_UnitTest
{
    public record Book
    {
        public string Title { get; }
        public DateTimeOffset ReleaseDate { get; } = DateTimeOffset.MinValue;

        public Book(string title)
        {
            Title = title;
        }
    }

    public sealed class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Name = "Book";
            Field(h => h.Title).Description("title");
            Field(h => h.ReleaseDate).Description("releaseDate");
        }
    }
}
