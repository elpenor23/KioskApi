// namespace KioskApi;

// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using KioskApi.Models;
// using Microsoft.Azure.Cosmos;
// using Microsoft.Azure.Cosmos.Fluent;
// using Microsoft.Extensions.Configuration;

// public class CosmosDbService : ICosmosDbService
// {
//     private Container _container;

//     public CosmosDbService(
//         CosmosClient dbClient,
//         string databaseName,
//         string containerName)
//     {
//         this._container = dbClient.GetContainer(databaseName, containerName);
//     }
    
//     // public async Task AddItemAsync(WeatherItem item)
//     // {
//     //     await this._container.CreateItemAsync<WeatherItem>(item, new PartitionKey(item.Id));
//     // }

//     // public async Task DeleteItemAsync(string id)
//     // {
//     //     await this._container.DeleteItemAsync<WeatherItem>(id, new PartitionKey(id));
//     // }

//     public async Task<WeatherItem?> GetItemAsync(string id)
//     {
//         try
//         {
//             ItemResponse<WeatherItem> response = await this._container.ReadItemAsync<WeatherItem>(id, new PartitionKey(id));
//             return response.Resource;
//         }
//         catch(CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
//         { 
//             return null;
//         }

//     }

//     public async Task<IEnumerable<WeatherItem?>> GetItemsAsync(string queryString)
//     {
//         var query = this._container.GetItemQueryIterator<WeatherItem>(new QueryDefinition(queryString));
//         List<WeatherItem> results = new List<WeatherItem>();
//         while (query.HasMoreResults)
//         {
//             var response = await query.ReadNextAsync();
            
//             results.AddRange(response.ToList());
//         }

//         return results;
//     }

//     // public async Task UpdateItemAsync(string id, WeatherItem item)
//     // {
//     //     await this._container.UpsertItemAsync<WeatherItem>(item, new PartitionKey(id));
//     // }
// }