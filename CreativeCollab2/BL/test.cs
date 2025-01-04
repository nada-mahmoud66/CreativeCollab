using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace CreativeCollab2.BL
{
	public class test
	{
	}


	class Program
	{
		static async Task Main(string[] args)
		{
			// Path to the image file you want to send
			string imagePath = "path_to_your_image.jpg";

			// URL of your Flask API endpoint
			string apiUrl = "http://localhost:5000/predict"; // Adjust this URL according to your setup

			using (var httpClient = new HttpClient())
			{
				// Read the image file as bytes
				byte[] imageData = System.IO.File.ReadAllBytes(imagePath);

				// Create multipart form content
				var formData = new MultipartFormDataContent();
				formData.Add(new ByteArrayContent(imageData), "image", "image.jpg");

				// Send POST request to the Flask API
				HttpResponseMessage response = await httpClient.PostAsync(apiUrl, formData);

				// Check if the request was successful
				if (response.IsSuccessStatusCode)
				{
					// Read the response content
					string responseContent = await response.Content.ReadAsStringAsync();

					// Display the prediction received from the Flask API
					Console.WriteLine("Prediction: " + responseContent);
				}
				else
				{
					Console.WriteLine("Error: " + response.StatusCode);
				}
			}
		}
	}
}
