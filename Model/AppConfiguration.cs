namespace LogSummaryAI.Model;

public sealed class AppConfiguration
{
    public string InputLogPath { get; set; } = "";
    public string OutputSummaryPath { get; set; } = "";
    public int MaxChunkCharacters { get; set; } = 12000;
    public OllamaConfiguration Ollama { get; set; } = new();
    public SemanticSearchConfiguration SemanticSearch { get; set; } = new();
}

public sealed class OllamaConfiguration
{
    public string BaseUrl { get; set; } = "http://localhost:11434";
    public string Model { get; set; } = "phi4-mini:latest";
    public int TimeoutSeconds { get; set; } = 300;
    public double Temperature { get; set; } = 0.2;
    public int MaxOutputTokens { get; set; } = 2048;
}

public sealed class SemanticSearchConfiguration
{
    public string EmbeddingModel { get; set; } = "nomic-embed-text";
    public string Query { get; set; } =
        "Application errors, exceptions, failures, their causes, affected services, timestamps, and impact.";
    public int MaxEmbeddingCharacters { get; set; } = 4000;
    public int MaxResults { get; set; } = 12;
}
