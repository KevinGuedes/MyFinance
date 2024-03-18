namespace MyFinance.Contracts.Summary.Responses;

public sealed class SummaryResponse
{
    public required string FileName { get; set; }
    public required byte[] FileContent{ get; set; }

    public void Deconstruct(out string fileName, out byte[] fileContent)
    {
        fileName = FileName;
        fileContent = FileContent;
    }
}
