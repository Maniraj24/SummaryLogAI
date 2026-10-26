# LogSummaryAI

LogSummaryAI is a .NET 8 console application that creates error-focused summaries of application logs using Ollama. Configure one or more application instances in `appsettings.json`; the app processes each instance and writes a separate summary named after the application.

## Requirements

- .NET 8 SDK
- [Ollama](https://ollama.com/download), running and reachable at the configured `Ollama.BaseUrl`
- The configured summary and embedding models pulled into Ollama

For the models in the example configuration:

```bash
ollama pull phi4-mini:latest
ollama pull nomic-embed-text
```

## Configure

Add an object to the `Instances` array for every application whose log you want summarized:

```json
{
  "Instances": [
    {
      "ApplicationName": "Orders",
      "InputLogPath": "Input/orders.log",
      "OutputSummaryPath": "Output"
    },
    {
      "ApplicationName": "Billing",
      "InputLogPath": "Input/billing.log",
      "OutputSummaryPath": "Output"
    }
  ],
  "MaxChunkCharacters": 12000,
  "SemanticSearch": {
    "EmbeddingModel": "nomic-embed-text",
    "Query": "Application errors, exceptions, failures, their causes, affected services, timestamps, and impact.",
    "MaxEmbeddingCharacters": 4000,
    "MaxResults": 12
  },
  "Ollama": {
    "BaseUrl": "http://localhost:11434",
    "Model": "phi4-mini:latest",
    "TimeoutSeconds": 300,
    "Temperature": 0.2,
    "MaxOutputTokens": 2048
  }
}
```

Paths can be absolute or relative to the current working directory. Each `ApplicationName` must be a valid file name. The app creates the configured output directory if needed and writes `<ApplicationName>.txt` there. In the example above, the output files are `Output/Orders.txt` and `Output/Billing.txt`.

The shared `SemanticSearch` and `Ollama` settings apply to every instance:

- `MaxChunkCharacters`: initial candidate chunk size.
- `EmbeddingModel`: Ollama model that converts the query and log text into embeddings for semantic retrieval.
- `Query`: the text used to find relevant log chunks.
- `MaxEmbeddingCharacters`: maximum character length of each text piece embedded during retrieval.
- `MaxResults`: maximum number of top-ranked text pieces passed on for summarization.
- `BaseUrl`: Ollama server URL.
- `Model`: Ollama text-generation model used to summarize selected log text.
- `TimeoutSeconds`: HTTP timeout for Ollama requests.
- `Temperature`: generation variability; lower values generally produce more consistent output.
- `MaxOutputTokens`: generation limit per Ollama request.

## Run

From the project directory:

```bash
dotnet run
```

The app looks for `appsettings.json` in the current working directory, then in the application directory. It processes instances in order, reports each summary path, and continues to later instances if one fails. The process exits unsuccessfully if any instance failed.

## Processing overview

For each configured instance, the app:

1. Reads the input log.
2. Locally keeps entries matching error-related indicators (or matching text lines and their continuation lines).
3. Splits the filtered text into candidate chunks.
4. Uses the configured embedding model through Ollama to embed the search query and candidate text; the app computes cosine similarity and selects up to `MaxResults`.
5. Groups every locally filtered log record by exact exception/error type and message and counts occurrences in code. Semantic search selects excerpts for context, and Ollama writes the brief overall summary. Counts do not depend on the retrieved excerpts or the model's ability to count.
6. Saves the result as `<ApplicationName>.txt`.

The local filter is error-focused, so the issue counts cover matching filtered records, not necessarily every entry in the input log. Repeated issues are grouped when their exception/error type and message match (case-insensitively, with whitespace normalized); different messages remain separate. Semantic retrieval narrows contextual excerpts for the overall summary, but does not reduce the counted issue inventory. JSON input is expected to be a single valid JSON object or array; plain-text logs are also supported.

## Privacy

Log text selected by the filter is sent to the Ollama server at `Ollama.BaseUrl` for embedding and summarization. With the default localhost URL, requests go to the Ollama service on the same machine. If you configure a remote server, review its access and data-handling policies before sending logs.
