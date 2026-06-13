# Specification: Help and Usage Display

## Problem
Currently, if no parameters are provided, the application shows a simple error message. It doesn't explain how to use the tool or what flags are available.

## Proposed Change
When the application is run without any arguments:
1. Display a clean "Usage" guide using Spectre.Console.
2. List all available arguments and flags:
   - `<path>`: Folder path to scan for LiteDB databases.
   - `-C`: Count-only mode (summarizes collections).
   - `-M<n>`: Max rows per collection (default: 20).

## Acceptance Criteria
- Running `LiteDBViewer` with no arguments shows the help screen.
- The help screen is visually consistent with the rest of the app (using Spectre.Console).
- The application exits gracefully after showing help.
