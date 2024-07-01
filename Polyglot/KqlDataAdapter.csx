/// Class for adapting data stored as a polyglot notebook variable to something plottable
using System.Text.Json;

public class KqlDataAdapter
{
    public KqlDataAdapter(JsonDocument source)
    {
        this.Source = source;
    }

    public JsonDocument Source { get; init; }

    public IList<T> GetColumnValues<T>(string columnName)
    {
        var labels = new List<T>();
        foreach(var table in this.Source.RootElement.EnumerateArray())
        {
            GetColumnValues(labels, table, columnName);
        }

        return labels;
    }

    public IList<T> GetColumnValues<T>(string columnName, int tableIndex)
    {
        var labels = new List<T>();

        //// TODO: Figure out how to guard indexing        
        GetColumnValues(labels, this.Source.RootElement[tableIndex], columnName);

        return labels;
    }

    private void GetColumnValues<T>(IList<T> outputList, JsonElement table, string columnName)
    {
        foreach(var row in table.GetProperty("data").EnumerateArray())
        {
            outputList.Add(row.GetProperty(columnName).Deserialize<T>());
        }
    }
};