# Implementation Plan: Count Only Mode

## Phase 1: Preparation
- [x] Define the `-C` flag variable in `Program.cs`.
- [x] Parse arguments to detect `-C` or `-c`.

## Phase 2: Implementation
- [x] Wrap the indexing logic in a conditional check (only run if NOT in count mode).
- [x] Update the main display loop to handle count-only output:
    - If `countOnly`: Show a simple table or list with collection names and document counts.
    - Else: Show the existing detailed tables.

## Phase 3: Verification
- [x] Run `dotnet build` to ensure no errors.
- [x] Test with `dotnet run -- Databases -C`.
- [x] Test with `dotnet run -- Databases` (ensure no regressions).
