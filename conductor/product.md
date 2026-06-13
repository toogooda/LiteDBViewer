# LiteDBViewer Product Definition

## Goal
A .NET CLI application that scans a directory for LiteDB database files and displays their collection contents in a clean, tabular format.

## Core Features
- **Directory Scanning**: Accept a path as an argument and find all LiteDB files.
- **Data Extraction**: Connect to each database and retrieve data from all collections.
- **Tabular Display**: Output data in a table format with headers for readability.

## Tech Stack
- .NET 8.0+ (Console Application)
- LiteDB (Document Database)
- Spectre.Console (For rich terminal output/tables)
