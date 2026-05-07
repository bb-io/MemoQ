using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Apps.MemoQ.Utils.Csv;

public static class CsvHelper
{
    public static Dictionary<string, Dictionary<string, decimal>> MemoQCsvToDictionary(string csvContent)
    {
        var rawMetrics = new Dictionary<string, Dictionary<string, decimal>>(StringComparer.OrdinalIgnoreCase);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = false,
            BadDataFound = null,
            MissingFieldFound = null
        };

        using var stringReader = new StringReader(csvContent);
        using var csv = new CsvReader(stringReader, config);

        if (!csv.Read()) 
            return rawMetrics;
        var headers = csv.Parser.Record;

        if (!csv.Read()) 
            return rawMetrics;
        var subHeaders = csv.Parser.Record;

        if (headers == null || subHeaders == null) 
            return rawMetrics;

        var columnMap = new Dictionary<string, Dictionary<string, int>>(StringComparer.OrdinalIgnoreCase);

        for (int i = 2; i < headers.Length; i++) 
        {
            var categoryName = headers[i].Trim();
            if (string.IsNullOrEmpty(categoryName)) 
                continue;
            
            columnMap[categoryName] = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            rawMetrics[categoryName] = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

            for (int j = i; j < i + 8 && j < subHeaders.Length; j++)
            {
                var subMetricName = subHeaders[j].Trim();

                if (string.IsNullOrEmpty(subMetricName)) 
                    continue;
                
                columnMap[categoryName][subMetricName] = j;
                rawMetrics[categoryName][subMetricName] = 0m;
            }
        }

        while (csv.Read())
        {
            var row = csv.Parser.Record;
            if (row == null || row.Length < 2) 
                continue;

            foreach (var (category, value) in columnMap)
            {
                foreach (var (subMetricName, colIndex) in value)
                {
                    if (colIndex < row.Length && decimal.TryParse(row[colIndex], out var count))
                        rawMetrics[category][subMetricName] += count;
                }
            }
        }

        return rawMetrics;
    }
}