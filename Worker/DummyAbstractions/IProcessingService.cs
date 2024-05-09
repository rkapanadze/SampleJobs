namespace Worker.DummyAbstractions;

public interface IProcessingService
{
    Task<string> ProcessData(string data);
}