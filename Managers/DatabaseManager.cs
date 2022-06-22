using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace KioskApi.Managers;
public class DatabaseManager
{    
    #region "Database Access"
    private readonly IDocumentClient _documentClient;
    readonly string databaseId;
    readonly string _collectionId;
    public IConfiguration Configuration{get;}
    public DatabaseManager(IDocumentClient documentClient, IConfiguration configuration, string collectionId){
        _documentClient = documentClient;
        Configuration = configuration;
        databaseId = Configuration["DatabaseId"];
        _collectionId = collectionId;

        BuildCollection().Wait();
    }

    public IQueryable<T> GetData<T>(int maxResults) where T : Models.IModel, new()
    {
        return _documentClient.CreateDocumentQuery<T>(
            UriFactory.CreateDocumentCollectionUri(databaseId, _collectionId), 
            new FeedOptions { MaxItemCount = maxResults});
    }

    public async void UpdateData<T>(T item) where T : Models.IModel, new()
    {
        await _documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseId, _collectionId, item.Id), item);
        
    }
    public async void AddData<T>(T item) where T : Models.IModel, new()
    {
        await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, _collectionId), item);
    }

    private async Task BuildCollection(){
        await _documentClient.CreateDatabaseIfNotExistsAsync(new Database {Id = databaseId});
        await _documentClient.CreateDocumentCollectionIfNotExistsAsync(
            UriFactory.CreateDatabaseUri(databaseId), 
            new DocumentCollection {Id = _collectionId});
    }
    #endregion
}