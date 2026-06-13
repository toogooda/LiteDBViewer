# Specification: Count Only Mode

## Problem
Currently, LiteDBViewer always lists the content of every collection in every database. For large databases, this can be slow and overwhelming if the user only wants to see how many items are in each collection.

## Proposed Change
Add a `-C` or `-c` flag to the CLI arguments.
When this flag is present:
1. Skip the "Indexing and resolving entities" phase (which is expensive and unnecessary for counts).
2. For each collection in each database, display only the collection name and the total count of documents.
3. Use a simplified output format (perhaps a summary table or simple lines).

## Acceptance Criteria
- Running `LiteDBViewer <path> -C` displays counts instead of item tables.
- Running `LiteDBViewer <path> -c` (lowercase) also works.
- Normal operation (without `-C`) is unaffected.
- The indexing progress bar should NOT appear in count mode.
