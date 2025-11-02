using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace DocumentPipeline.Infrastructure.Processors;
    
public class CsvProcessor : IDocumentProcessor
{
    public bool CanProcess(DocumentType type) => type == DocumentType.Csv;

    public DocumentContent Process(DocumentContent content)
    {
        string[] lines = content.ProcessedContent.Split('\n')
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();

        if (lines.Length == 0)
        {
            content.ProcessedContent = string.Empty;
            return content;
        }

        int maxColumns = lines
            .Select(line => line.Split(',').Length)
            .Max();

        var result = new StringBuilder();
        foreach (var line in lines)
        {
            string[] columns = line.Split(',');
            string[] normalizedColumns = new string[maxColumns];

            for (int i = 0; i < maxColumns; i++)
            {
                normalizedColumns[i] = i < columns.Length 
                    ? columns[i].Trim() 
                    : string.Empty;
            }

            result.AppendLine(string.Join(",", normalizedColumns));
        }

        content.ProcessedContent = result.ToString().TrimEnd();
        content.Metadata["NormalizedColumns"] = maxColumns.ToString();
        content.Metadata["RowsAfterFormat"] = lines.Length.ToString();
        
        return content;
    }
}
