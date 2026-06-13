# Implementation Plan: Help and Usage Display

## Phase 1: Preparation
- [x] Define the help content and layout.

## Phase 2: Implementation
- [x] Modify `Program.cs` to check `args.Length == 0`.
- [x] Implement the `ShowHelp()` logic using Spectre.Console (Panels/Tables).

## Phase 3: Verification
- [x] Run `dotnet run` (no args) to verify help display.
- [x] Run `dotnet run -- Databases` to ensure normal operation still works.
