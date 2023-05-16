namespace LibraryApp;

using ZXing.Net.Maui.Controls;
using ZXing.Net.Maui;
using System.Diagnostics.CodeAnalysis;
using LibraryApp.Services;
using LibraryApp.Models;

/*            <Grid.RowDefinitions>
                <RowDefinition />
                <RowDefinition />
                <RowDefinition />
                <RowDefinition />
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
                <ColumnDefinition />
                <ColumnDefinition />
            </Grid.ColumnDefinitions>
*/

public partial class ISBNScanPage
	: ContentPage
{
	BookInformationRetrievalService _bookInformationService = null;

	public ISBNScanPage()
	{
		InitializeComponent();
		_bookInformationService = new BookInformationRetrievalService();
	}

	/**
	 * Handles the event that a barcode has been detected. The passed event contains the 
	 * read barcode values
	 * 
	 * @param detectedBarcodeEvent Event with String arrays containing the detected barcodes
	 */
    private void OnBarcodeDetected(object sender, BarcodeDetectionEventArgs detectedBarcodeEvent)
	{
		Book bookInformation = null;
		string detectedISBN = detectedBarcodeEvent.Results[0].Value;

        Console.WriteLine("Read ISBN: " + detectedISBN);

        ISBNBarcodeReader.Opacity = 50;

        // now call API to resolve the ISBN into title, authour, book image, ... from API.
        // This uses the Google books API
        bookInformation = _bookInformationService.RetrieveBookInformationForISBN(detectedISBN);

        /*
        detectedBarCode.Text = detectedISBN;
        if (bookInformation != null)
        {
            bookTitle.Text = "X" + bookInformation.Title + "Z";
            bookAuthor.Text = bookInformation.Author;
            bookISBN.Text = bookInformation.ISBN;
            bookDescription.Text = bookInformation.Description;
            bookImageUrl.Source = bookInformation.ImageLink;
        }

        SemanticScreenReader.Announce(detectedBarCode.Text);
        SemanticScreenReader.Announce(bookTitle.Text);
        SemanticScreenReader.Announce(bookAuthor.Text);
        SemanticScreenReader.Announce(bookDescription.Text);
        SemanticScreenReader.Announce(bookImageUrl.Source);
*/
        Dispatcher.Dispatch(() =>
		{
            Console.WriteLine("Dispatch: Read ISBN: " + detectedISBN);

            // The first barcode in the array is the most likely one
            //detectedBarCode.Text = detectedBarcodeEvent.Results[0].Value;
            detectedBarCode.Text = detectedISBN;

            Console.WriteLine("Dispatch_2: Read ISBN: " + detectedISBN);


            if (bookInformation != null)
			{
                // correct the image link for Android. Only https calls are allowed
                if (bookInformation.ImageLink != null && bookInformation.ImageLink.ToLower().StartsWith("http:"))
                {
                    // needs to be more robust, since a) http could be in the middle of the URL and b) it may not be
                    // all lower case.
                    bookInformation.ImageLink = bookInformation.ImageLink.Replace("http:", "https:");
                }

                bookTitle.Text = bookInformation.Title;
                bookAuthor.Text = bookInformation.Author;
                bookISBN.Text = bookInformation.ISBN;
                bookDescription.Text = bookInformation.Description;
                bookImageUrl.Source = ImageSource.FromUri(new Uri(bookInformation.ImageLink));
                //bookImageUrl.Source = ImageSource.FromUri(new Uri("http://books.google.com/books/content?id=SXvxrQEACAAJ&printsec=frontcover&img=1&zoom=5&source=gbs_api"));
                //bookImageUrlLabel.Text = bookInformation.ImageLink;
			}


            // Note: At the moment the barcode scanner only works in landscape mode.
            // Maybe set a big green tick mark on top of the scanner to show, the ISBN was detected.

        });

    }
		

}