using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibraryApp.Models;

namespace LibraryApp.Services;

/** This class is responsible for persisting book data to a database. 
 *  The book information will be delivered to this class from the calling classes and created, updated, or deleted in the database.
 *  
 *  @author: GSC
 */
class BookPersistenceService
{

    /** 
     * This method will create a new book in the database.
     * 
     * @param: book - the book to be created
     * @return: true if the book was created successfully, false otherwise
     * 
     */
    public bool CreateBook(Book book)
    {
        if (book == null) 
        {
            return false;
        }

        // TODO: Implement this method

        return false;

    }

}
