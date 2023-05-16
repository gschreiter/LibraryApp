using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using LibraryApp.Models;
using LibraryApp.Services.ApiModels;
using Newtonsoft.Json;

namespace LibraryApp.Services;


/**
 * Service currently uses Google Books API to retrieve book information based on an ISBN.
 * 
 * Idea is, that this is a base class and implementations of the service should be a detailed implementation of a sepcific book information 
 * retrieval service.
 */
public class BookInformationRetrievalService
{
    // Timespan how long we will wait for the book API to respond before we give up
    // Currently we use it twice: For the API call itself and for reading the actual content. So worst case is, that we wait twice as long as this value
    private readonly int _timeoutInMilliSeconds = 10000;

    // Base Google API for retrieving book information
    private readonly String _bookInformationRetrievalBaseUrl = "https://www.googleapis.com/books/v1/volumes?q=isbn:";

    // http client to access the book API
    static readonly HttpClient _bookInformationRetrievalHttpClient = new HttpClient ();
    private readonly TimeSpan _timeoutForApiCall;


    // Authentication information if used for authenticated services. Not used at the moment.
    readonly String _authenticationHeaderKey = "x-api-key";
    readonly String _authenticationHeaderValue = "";


    
    /**
     * Consructor for class to retrieve book information from the Google Books API based on an ISBN.
     * 
     * Initiates a HttpClient.
     */
    public BookInformationRetrievalService()
    {
       // _bookInformationRetrievalHttpClient = new HttpClient();
        _timeoutForApiCall = TimeSpan.FromMilliseconds(_timeoutInMilliSeconds);

        // we do not need authentication for the book API
        //bookInformationRetrievalHttpClient.DefaultRequestHeaders.Add(authenticationHeaderKey, authenticationHeaderValue);
    }

    /**
     * Retrieves book information from the Google Books API based on an ISBN.
     * This is an asynchronous method.
     * 
     * @param isbn ISBN to retrieve book information for
     * @return Book information retrieved from the API
     */
    public async Task<LibraryApp.Models.Book> RetrieveBookInformationForISBNAsync(String isbn)
    {
        APIBookResponse bookResponse = null;
        string responseRawContent = null;
        string bookRetrievalUrl = null;
        HttpResponseMessage httpResponse = null;
        
        LibraryApp.Models.Book retrievedBookInformation = null;

        if (isbn == null)
        {
            return null;
        }

        // Access the Google API
        bookRetrievalUrl = _bookInformationRetrievalBaseUrl + isbn;
        httpResponse = await _bookInformationRetrievalHttpClient.GetAsync(bookRetrievalUrl).ConfigureAwait(false);

        Console.WriteLine("Response status: " + httpResponse.StatusCode);
        if (httpResponse.IsSuccessStatusCode)
        {

            // we read the content
            responseRawContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            Console.WriteLine("Response content: " + responseRawContent);
            bookResponse = JsonConvert.DeserializeObject<APIBookResponse>(responseRawContent);

            if (bookResponse != null && bookResponse.Items.Count > 0)
            {
                //return bookResponse.Items[0];
                retrievedBookInformation = MapHttpResponseModelToInternalModel(bookResponse.Items[0]);
            }
        }

        return retrievedBookInformation;
    }

    /**
     * Retrieves book information from the Google Books API based on an ISBN.
     * Method converts asynchronous call to synchronous method.
     * 
     * @param isbn ISBN to retrieve book information for
     * @return Book information retrieved from the API
     */
    public LibraryApp.Models.Book RetrieveBookInformationForISBN (String isbn)
    {
        APIBookResponse bookResponse = null;
        string responseRawContent = null;
        string bookRetrievalUrl = null;
        HttpResponseMessage httpResponse = null;
        Task<HttpResponseMessage> httpResponseAsync = null;
        Task<string> responseRawContentAsync = null;
        LibraryApp.Models.Book retrievedBookInformation = null;

        if (isbn == null)
        {
            return null;
        }

        // Access the Google API
        bookRetrievalUrl = _bookInformationRetrievalBaseUrl + isbn;
        httpResponseAsync = _bookInformationRetrievalHttpClient.GetAsync(bookRetrievalUrl);
        httpResponseAsync.ConfigureAwait(false);
        httpResponseAsync.Wait(_timeoutForApiCall);

        // Check if the response timed out
        if (!httpResponseAsync.IsCompleted)
        {
            Console.WriteLine("Response timed out");
            return null;
        }

        // API call returned. No we check the response status and read the content.
        httpResponse = httpResponseAsync.Result;

        Console.WriteLine("Response status: " + httpResponse.StatusCode);
        if (httpResponse.IsSuccessStatusCode)
        {

            // we read the content
            responseRawContentAsync = httpResponse.Content.ReadAsStringAsync();

            responseRawContentAsync.Wait(_timeoutForApiCall);

            // Check if reading the content timed out
            if (!responseRawContentAsync.IsCompleted)
            {
                Console.WriteLine("Reading http content timed out");
                return null;
            }

            responseRawContent = responseRawContentAsync.Result;

            Console.WriteLine("Response content: " + responseRawContent);
            bookResponse = JsonConvert.DeserializeObject<APIBookResponse>(responseRawContent);

            if (bookResponse != null && bookResponse.TotalItems > 0 
                && bookResponse.Items != null && bookResponse.Items.Count > 0)
            {
                //return bookResponse.Items[0];
                retrievedBookInformation = MapHttpResponseModelToInternalModel(bookResponse.Items[0]);
            }
            else
            {
                Console.WriteLine("No book information found for ISBN " + isbn);
            } 

        }

        return retrievedBookInformation;
    }

    /**
     * Maps the response model from the API to the internal model.
     *
     * @param apiBookInformation model from API JSON response to be mapped to the internal model
     * @return populated internal model
     */
    public LibraryApp.Models.Book MapHttpResponseModelToInternalModel (LibraryApp.Services.ApiModels.Book apiBookInformation)
    {
        LibraryApp.Models.Book internalModel = null;
        string authors = null;
        int numberOfAuthors = 0;
        string isbnString = null;
        int numberOfISBNs = 0;
        ISBN isbnRecord = null;
        Boolean isbnRecordFound = false;


        // concatenate the authors in one string for now. We can change this later.
        for (numberOfAuthors = 0; numberOfAuthors< apiBookInformation.VolumeInfo.Authors.Count; numberOfAuthors++)
        {
            if (numberOfAuthors == 0)
            {
                authors = apiBookInformation.VolumeInfo.Authors[numberOfAuthors];
            }
            else
            {
                authors = authors + "; " + apiBookInformation.VolumeInfo.Authors[numberOfAuthors];
            }                
        }

        // find the ISBN. There are different "industry identifiers" which are not clear, what they are.
        isbnRecordFound = false;
        for (numberOfISBNs = 0; numberOfISBNs < apiBookInformation.VolumeInfo.ISBNList.Count && (!isbnRecordFound); numberOfISBNs++)
        {
            isbnRecord = apiBookInformation.VolumeInfo.ISBNList[numberOfISBNs];
            if (isbnRecord.IsPublicISBN())
            {
                isbnString = isbnRecord.ISBNNumber;
            }

            isbnRecordFound = true;
        }

        if (apiBookInformation != null) 
        {
                internalModel = new LibraryApp.Models.Book( apiBookInformation.VolumeInfo.Title,
                                                            authors,
                                                            isbnString,
                                                            apiBookInformation.VolumeInfo.ImageLinks.Thumbnail,
                                                            apiBookInformation.VolumeInfo.Description);
        }

        return internalModel;
    }
}