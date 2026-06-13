using LiteDB;
using Spectre.Console;
using System.IO;
using System.Text.RegularExpressions;

var pathArg = args.FirstOrDefault(a => !a.StartsWith("-"));
var maxRows = 20;

// Parse -M option
var mArg = args.FirstOrDefault(a => a.StartsWith("-M"));
if (mArg != null && int.TryParse(mArg.Substring(2), out var parsedMax))
{
    maxRows = parsedMax;
}

if (string.IsNullOrEmpty(pathArg))
{
    AnsiConsole.MarkupLine("[red]Error:[/] Please provide a folder path.");
    return;
}

var folderPath = pathArg;
var dbFiles = Directory.EnumerateFiles(folderPath, "*.db", SearchOption.TopDirectoryOnly).ToList();

var idToNameMap = new Dictionary<string, string>();
var fileCounters = new Dictionary<string, int>();

AnsiConsole.Status().Start("Indexing and resolving entities...", ctx =>
{
    foreach (var dbFile in dbFiles)
    {
        var fileNameOnly = Path.GetFileNameWithoutExtension(dbFile);
        try
        {
            using var db = new LiteDatabase($"Filename={dbFile};ReadOnly=true");
            foreach (var colName in db.GetCollectionNames())
            {
                var col = db.GetCollection(colName);
                var docs = col.FindAll().ToList();
                
                foreach (var doc in docs)
                {
                    var id = doc["_id"];
                    string idStr = id.IsGuid ? id.AsGuid.ToString() : id.ToString();
                    
                    if (idToNameMap.ContainsKey(idStr)) continue;

                    string? resolvedName = null;

                    if (doc.ContainsKey("Name")) resolvedName = doc["Name"].AsString;
                    else if (doc.ContainsKey("FirstName") || doc.ContainsKey("LastName"))
                    {
                        var first = doc.ContainsKey("FirstName") ? doc["FirstName"].AsString : "";
                        var last = doc.ContainsKey("LastName") ? doc["LastName"].AsString : "";
                        resolvedName = $"{first} {last}".Trim();
                    }

                    if (string.IsNullOrWhiteSpace(resolvedName))
                    {
                        if (!fileCounters.ContainsKey(fileNameOnly)) fileCounters[fileNameOnly] = 1;
                        resolvedName = $"{fileNameOnly}{fileCounters[fileNameOnly]++}";
                    }

                    idToNameMap[idStr] = resolvedName;
                }
            }
        } catch {}
    }
});

AnsiConsole.MarkupLine($"[green]Indexed {idToNameMap.Count} unique entities. Showing max {maxRows} rows per collection.[/]\n");

foreach (var dbFile in dbFiles)
{
    AnsiConsole.Write(new Rule($"[blue]File: {Path.GetFileName(dbFile)}[/]"));
    using var db = new LiteDatabase($"Filename={dbFile};ReadOnly=true");
    
    foreach (var colName in db.GetCollectionNames())
    {
        AnsiConsole.MarkupLine($"\n[yellow]Collection: {colName}[/]");
        var col = db.GetCollection(colName);
        var docs = col.FindAll().Take(maxRows).ToList();

        if (!docs.Any()) { AnsiConsole.MarkupLine("[grey]Empty[/]"); continue; }

        var keys = docs.SelectMany(d => d.Keys).Distinct()
            .OrderBy(k => k == "_id" ? 0 : 1).ThenBy(k => k).ToList();

        var table = new Table().Border(TableBorder.Rounded).Expand();
        foreach (var key in keys) table.AddColumn(new TableColumn($"[bold]{key}[/]").LeftAligned());

        foreach (var doc in docs)
        {
            var row = new List<string>();
            foreach (var key in keys)
            {
                if (!doc.ContainsKey(key)) row.Add("[grey]-[/]");
                else row.Add(FormatValue(doc[key], idToNameMap));
            }
            table.AddRow(row.ToArray());
        }
        AnsiConsole.Write(table);
    }
}

static string FormatValue(BsonValue val, Dictionary<string, string> map)
{
    string guidStr = "";
    if (val.IsGuid) guidStr = val.AsGuid.ToString();
    else if (val.IsDocument && val.AsDocument.ContainsKey("$guid")) guidStr = val.AsDocument["$guid"].AsString;

    if (!string.IsNullOrEmpty(guidStr))
    {
        if (map.TryGetValue(guidStr, out var name))
            return $"[cyan]{name}[/]\n[grey]{guidStr}[/]";
        return $"[grey]{guidStr}[/]";
    }

    if (val.IsDateTime) return val.AsDateTime.ToString("yyyy-MM-dd HH:mm");
    if (val.IsBoolean) return val.AsBoolean ? "[green]true[/]" : "[red]false[/]";

    var str = val.ToString();
    if (str.StartsWith("{\"$numberLong\":")) return val.AsInt64.ToString();

    return str.Trim('"');
}
