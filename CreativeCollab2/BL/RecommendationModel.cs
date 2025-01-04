using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CreativeCollab2.BL
{


	class RecommendationModel
	{
		[HttpPost]
		public async Task<List<int>> recommend(string userId)
		{
			string url = "http://127.0.0.1:5000/receive-data"; // Replace with your Flask server URL

			// Example data
			var data = new
			{
				UserID = userId,

				// Add more data fields as needed
			};

			// Convert data to JSON string
			var json = JsonConvert.SerializeObject(data);

			// Create HttpClient
			using (var httpClient = new HttpClient())
			{
				// Create StringContent with JSON data
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				try
				{
					// Send POST request
					var response = await httpClient.PostAsync(url, content);

					// Check if response is successful
					if (response.IsSuccessStatusCode)
					{
						// Read response content
						var responseContent = await response.Content.ReadAsStringAsync();

						// Deserialize response JSON to get the list of post IDs as integers
						var postIds = JsonConvert.DeserializeObject<List<int>>(responseContent);

						// Return the list of post IDs
						return postIds;
					}
					else
					{
						Console.WriteLine("Error: " + response.StatusCode);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
			}

			// If an error occurs or if the response is not successful, return an empty list
			return new List<int>();
		}
	}

}