using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.Mappers;

public static class SummaryMapper
{
    public static class DTR
    {
        public static SummaryResponse Map((string FileName, byte[] FileContent) summaryData)
        {
            return new SummaryResponse
            {
                FileName = summaryData.FileName,
                FileContent = summaryData.FileContent
            };
        }
    }
}