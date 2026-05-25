namespace QuickGridDemo.Models;

public class Book
{
    public string[] Author_Name { get; set; }
    public int First_Publish_Year { get; set; }
    public string[] Language { get; set; }
    public string Title { get; set; }
    public string LanguageList { get { return string.Join(',', Language ?? []); } }
    public string AuthorsList { get { return string.Join(',', Author_Name ?? []); } }
}

public class BookQueryResult : IDisposable
{
    public int NumFound { get; set; }
    public List<Book> Docs { get; set; }

    public void Dispose()
    {

    }
}