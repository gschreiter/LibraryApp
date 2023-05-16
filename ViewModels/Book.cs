namespace LibraryApp.Models;

/**
 * Represents a book with a title, author, and ISBN number.
 */
public class Book
{
    /** The title of the book. */
    public string Title { get; set; }

    /** The author of the book. */
    public string Author { get; set; }

    /** The ISBN number of the book. */
    public string ISBN { get; set; }

    // http link for the thumbnail image
    public string ImageLink { get; set; }

    // description of book
    public string Description { get; set; }

    /**
     * Initializes a new instance of the Book class with the specified title, author, and ISBN number.
     * 
     * @param title The title of the book.
     * @param author The author of the book.
     * @param isbn The ISBN number of the book.
     */
    public Book(string title, string author, string isbn, string imageLink, string description)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        ImageLink = imageLink;
        Description = description;
    }

    /**
     * Returns a string representation of the book.
     *         
     * @return A string representation of the book.
     */
    public String toString ()
    {
        return "Title: " + Title + ", Author: " + Author + ", ISBN: " + ISBN 
                + ", Description: " + Description + ", Image link: " + ImageLink;

    }
}