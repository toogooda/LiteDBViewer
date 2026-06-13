# Specification: Initial Implementation

## Requirements
- Create a .NET Console Application.
- Add `LiteDB` and `Spectre.Console` NuGet packages.
- Implement logic to:
  1. Parse the provided folder path.
  2. Locate all `*.db` and `*.litedb` files.
  3. For each file, iterate through all collections.
  4. Print each collection's documents in a table.

## Constraints
- Handle potential access issues (e.g., locked files).
- Support dynamic columns based on document properties.
