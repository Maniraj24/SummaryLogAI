# AI Learning Roadmap: Log RAG

## Goal

Add a local Retrieval-Augmented Generation (RAG) feature to the log summarizer. Users should be able to ask targeted questions about a log and receive answers grounded in retrieved log entries.

## Pathway

1. **Define retrieval goals (current step)**
   - Pick a small set of questions, such as “What recurring database errors occurred?” and “Which service had the most timeouts?”
   - For each question, note the expected log entries and useful metadata filters.
   - Use this as a known-answer test set.
2. **Prepare log entries**
   - Parse each record into a normalized entry.
   - Preserve timestamp, severity, service, message, and a stable entry ID.
   - Redact sensitive values before indexing or sending content to a model.
3. **Create embeddings and an index**
   - Generate embeddings locally for log messages or appropriately sized groups of entries.
   - Store vectors alongside original text and metadata in a local vector store.
4. **Retrieve relevant evidence**
   - Embed the user's question, retrieve similar entries, and apply metadata filters where useful.
   - Inspect the results and tune retrieval rather than assuming semantic similarity found everything.
5. **Generate grounded answers**
   - Send retrieved entries to Ollama with instructions to answer only from the evidence.
   - Include entry IDs or timestamps as citations, and say when the retrieved evidence is insufficient.
6. **Evaluate and improve**
   - Check whether expected entries appear in retrieved results and whether answers are supported.
   - Measure latency and retrieved-context size; compare against simple filters and aggregation.
7. **Update resume/profile**
   - After implementation and evaluation, describe only the features and measured results actually achieved.

## Start here

Create a small evaluation set before choosing a vector database or writing retrieval code. For each of 3–5 realistic questions, record the log entry IDs that should be retrieved and any filters (for example, severity or service). This makes it possible to tell whether RAG improves retrieval instead of merely producing plausible answers.

## Resume notes (draft for after the work is complete)

- Built a local log-analysis assistant using .NET, Ollama, embeddings, and vector search to retrieve evidence for natural-language questions.
- Evaluated retrieval relevance and answer grounding against a known-answer test set; report measured results once available.

These are draft targets, not claims of completed work. No resume or profile document was present in the project folder to update.
