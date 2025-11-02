using DocumentPipeline.Domain.Interfaces;
using DocumentPipeline.Domain.Models;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace DocumentPipeline.Infrastructure.Processors;

public class TxtProcessor : IDocumentProcessor
{
    private const int MaxLineLength = 80;

    public bool CanProcess(DocumentType type) => type == DocumentType.Txt;

    public DocumentContent Process(DocumentContent content)
    {
        string[] lines = content.ProcessedContent.Split('\n');
        var result = new StringBuilder();
        string? previousLine = null;

        foreach (var line in lines)
        {
            string trimmedLine = line.TrimEnd();

            if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                if (!string.IsNullOrWhiteSpace(previousLine))
                {
                    result.AppendLine();
                }
                previousLine = trimmedLine;
                continue;
            }

            if (trimmedLine.Length <= MaxLineLength)
            {
                result.AppendLine(trimmedLine);
            }
            else
            {
                int pos = 0;
                while (pos < trimmedLine.Length)
                {
                    int length = Math.Min(MaxLineLength, trimmedLine.Length - pos);
                    result.AppendLine(trimmedLine.Substring(pos, length));
                    pos += length;
                }
            }

            previousLine = trimmedLine;
        }

        content.ProcessedContent = result.ToString().TrimEnd();
        content.Metadata["FormattedLineCount"] = content.ProcessedContent.Split('\n').Length.ToString();
        return content;
    }
}
