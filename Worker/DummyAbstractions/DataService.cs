namespace Worker.DummyAbstractions;

public class DataService : IDataService
{
    public Task<string> FetchData()
    {
        return Task.FromResult("Ponchiki");
    }
}