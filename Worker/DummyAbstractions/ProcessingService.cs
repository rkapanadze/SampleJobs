namespace Worker.DummyAbstractions;

public class ProcessingService : IProcessingService
{
    public Task<string> ProcessData(string data)
    {
        return Task.FromResult(data);
    }
}