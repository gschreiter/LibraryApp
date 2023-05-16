using LibraryApp.Models;
using LibraryApp.Services;
//using Org.Apache.Http;
using System.Net.Http;

namespace LibraryApp;

public partial class MainPage : ContentPage
{
	int count = 0;
    private static readonly HttpClient _bookInformationRetrievalHttpClient = new HttpClient();

	public MainPage()
	{
		InitializeComponent();
        //_bookInformationRetrievalHttpClient = new HttpClient();
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

    private async void OnTestHttpClientClicked(object sender, EventArgs e)
    {
        Book bookInformation = null;
		BookInformationRetrievalService bookInformationService = null;
        System.Threading.Tasks.Task<Book> bookInformationTask = null;
        string detectedISBN = "9781617292484";

        // get the book information


        bookInformationService = new BookInformationRetrievalService();

        //System.Runtime.CompilerServices.ConfiguredTaskAwaitable<Book> configuredTaskAwaitable = bookInformationService.RetrieveBookInformationForISBN(detectedISBN).ConfigureAwait(false);
        //bookInformation = configuredTaskAwaitable.GetAwaiter().GetResult();

        //HttpResponseMessage httpResponse = await bookInformationService.RetrieveBookInformationForISBNAsync(detectedISBN).ConfigureAwait(false);

        //bookInformation = bookInformationService.RetrieveBookInformationForISBN(detectedISBN);

        HttpClient bookInformationRetrievalHttpClient = null;
        HttpResponseMessage httpResponse = null;

        //bookInformationRetrievalHttpClient = _bookInformationRetrievalHttpClient;

        //bookInformationRetrievalHttpClient = new HttpClient();
        //bookInformationRetrievalHttpClient.BaseAddress = new Uri("https://www.googleapis.com/books/v1/volumes?q=isbn:9781617292484");
        string bookRetrievalUrl = "https://www.googleapis.com/books/v1/volumes?q=isbn:9781617292484";
        Uri uri = new Uri(bookRetrievalUrl);
        //httpResponse = bookInformationRetrievalHttpClient.GetAsync(uri).Result;
        try
        {
            Console.WriteLine("Before call to HttpClient");
            //using HttpResponseMessage httpResponseNew = await bookInformationRetrievalHttpClient.GetAsync(uri);
            //using HttpResponseMessage httpResponseNew = await bookInformationRetrievalHttpClient.GetAsync(uri).ConfigureAwait(false);
            //httpResponse = bookInformationRetrievalHttpClient.GetAsync(uri).Result;
            //Console.WriteLine(httpResponseNew.Content.ReadAsStringAsync().Result);

            //string bookInformationString = httpResponseNew.Content.ReadAsStringAsync().Result;
            //string bookInformationString = await _bookInformationRetrievalHttpClient.GetStringAsync(uri).ConfigureAwait(false);
            string bookInformationString = await _bookInformationRetrievalHttpClient.GetStringAsync(uri);
            //string bookInformationString = _bookInformationRetrievalHttpClient.GetStringAsync(uri).Result;

            Console.WriteLine("After call to HttpClient");
            Console.WriteLine("Content from API Call: " + bookInformationString);

            if (bookInformation != null)
            {
                CounterBtn.Text = "Call Successful";
            }
            SemanticScreenReader.Announce(CounterBtn.Text);
        }
        catch (HttpRequestException exceptionObject)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", exceptionObject.Message);
        }
    }
}

