using System;
using System.Collections.Generic;
using System.Text.Json;
using FluentResults;
using Iwentys.Common;

namespace Iwentys.Domain.Study;

//FYI: very bad hack. It is not logic of StudyFeature
public class GoogleTableData
{
    public string Id { get; set; }
    public string SheetName { get; set; }
    public string FirstRow { get; set; }
    public string LastRow { get; set; }
    public List<string> NameColumnsList { get; set; }
    public string ScoreColumn { get; set; }

    public GoogleTableData()
    {
    }

    public GoogleTableData(string id,
        string sheetName,
        string firstRow,
        string lastRow,
        string[] nameColumns,
        string scoreColumn)
    {
        Id = id;
        SheetName = sheetName;
        FirstRow = firstRow;
        LastRow = lastRow;
        ScoreColumn = scoreColumn;
        NameColumnsList = nameColumns.SelectToList(x => x);
    }

    public static bool TryCreate(string serializedGoogleTableConfig, out GoogleTableData data)
    {
        if (serializedGoogleTableConfig is null)
        {
            data = null;
            return false;
        }

        try
        {
            var googleTableData = JsonSerializer.Deserialize<GoogleTableData>(serializedGoogleTableConfig);
            data = googleTableData;
            return true;
        }
        catch (Exception)
        {
            data = null;
            return false;
        }
    }

    public string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }
}