using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CreativeCollab2.BL
{
	public class MLService
	{
		private readonly HttpClient _client;

		public MLService()
		{
			_client = new HttpClient();
			_client.BaseAddress = new Uri("http://127.0.0.1:5000"); // Replace with your Flask API address
		}

		[HttpPost]
		public async Task<string> GetPredictionsAsync(string imagePath)
		{
			var requestData = new { image_path = imagePath };
			var response = await _client.PostAsJsonAsync("/predict", requestData);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}

		[HttpPost]
		public async Task<string> SearchSimilarImagesAsync(string imagePath)
		{
			var requestData = new { image_path2 = imagePath };
			var response = await _client.PostAsJsonAsync("/search-similar", requestData);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}

		[HttpPost]
		public async Task<string> GetRecommendationsAsync(int userId)
		{
			var requestData = new { UserID = userId };
			var response = await _client.PostAsJsonAsync("/receive-data", requestData);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}

		[HttpPost]
		public async Task<bool> CheckProfanityAsync(string sentence)
		{
			var requestData = new { sentence = sentence };
			var response = await _client.PostAsJsonAsync("/check-profanity", requestData);
			response.EnsureSuccessStatusCode();
			return bool.Parse(await response.Content.ReadAsStringAsync());
		}
	}

	public class PredictionHandler
	{
		private readonly MLService _mlService;

		public PredictionHandler()
		{
			_mlService = new MLService();
		}

		[HttpPost]
		public async Task<string> GetPredictionsForImageAsync(string imagePath)
		{
			var predictionsJson = await _mlService.GetPredictionsAsync(imagePath);
			return predictionsJson;
		}

		[HttpPost]
		public async Task<string> GetSimilarImagesForImageAsync(string imagePath)
		{
			var similarImagesJson = await _mlService.SearchSimilarImagesAsync(imagePath);
			return similarImagesJson;
		}

		[HttpPost]
		public async Task<bool> CheckSentenceForProfanityAsync(string sentence)
		{
			var isProfane = await _mlService.CheckProfanityAsync(sentence);
			return isProfane;
		}

	}
}
