# SOLID

This repo has a proof of concept example for document processing demonstrating application of SOLID principles:

- SRP (Single Responsibility): each class should do one job only.
- OCP (Open/Closed): code should be easy to extend without changing existing working code.
- LSP (Liskov Substitution): you should be able to replace a base type with a subtype without breaking things.
- ISP (Interface Segregation): prefer small focused interfaces instead of big ones.
- DIP (Dependency Inversion): high-level code should depend on interfaces/abstractions, not concrete classes.

## Implementation

I focus on 3 principles here: SRP, OCP, DIP.

1. SRP — single responsibility

Each processor class has only one job: process one document type. Example: `MarkdownProcessor`.

```csharp
public class MarkdownProcessor : IDocumentProcessor
{
	public bool CanProcess(DocumentType type) => type == DocumentType.Markdown;

	public void Process(Document document)
	{
		Console.WriteLine($"Processing Markdown: {document.Name}");
	}
}
```

2. OCP — open for extension, closed for modification

The pipeline doesn't need to be changed when we add new processors. It takes a list of `IDocumentProcessor` and asks each processor if it can handle a document.

```csharp
public class DocumentPipeline
{
	private readonly IEnumerable<IDocumentProcessor> _processors;
	public DocumentPipeline(IEnumerable<IDocumentProcessor> processors)
	{
		_processors = processors;
	}

	public void ProcessDocument(Document document)
	{
		var processor = _processors.FirstOrDefault(p => p.CanProcess(document.Type));
		if (processor == null) { Console.WriteLine("No processor"); return; }
		processor.Process(document);
	}
}
```

To add support for a new type, just add another `IDocumentProcessor` implementation — no changes to `DocumentPipeline`.

3. DIP — depend on abstractions

The project uses an interface `IDocumentProcessor` that processors implement. The pipeline depends on that interface, not on concrete processors.

```csharp
public interface IDocumentProcessor
{
	bool CanProcess(DocumentType type);
	void Process(Document document);
}
```

Constructor shows the dependency on the abstraction:

```csharp
private readonly IEnumerable<IDocumentProcessor> _processors;
public DocumentPipeline(IEnumerable<IDocumentProcessor> processors)
{
	_processors = processors;
}
```

This makes testing and swapping processors easier.


## Conclusion

The DocumentPipeline project shows how simple SOLID ideas make code easier to extend and maintain. Models hold data (SRP), the pipeline works with abstractions (DIP) and can be extended by adding processors (OCP).
