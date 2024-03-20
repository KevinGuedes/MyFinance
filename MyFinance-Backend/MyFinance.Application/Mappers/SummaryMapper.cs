using MyFinance.Contracts.Summary.Responses;

namespace MyFinance.Application.Mappers;

public static class SummaryMapper
{
    public static class DTR
    {
        public static SummaryResponse Map(Tuple<string, byte[]> summaryData)
        {
            var (fileName, fileContent) = summaryData;
            return new SummaryResponse
            {
                FileName = fileName,
                FileContent = fileContent
            };
        }
    }
}