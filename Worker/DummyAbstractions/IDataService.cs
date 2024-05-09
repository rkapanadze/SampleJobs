namespace Worker.DummyAbstractions;

public interface IDataService
{
    Task<string> FetchData();
}