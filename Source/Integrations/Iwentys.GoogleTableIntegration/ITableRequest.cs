using Google.Apis.Sheets.v4.Data;

namespace Iwentys.GoogleTableIntegration;

public interface ITableRequest<T>
{
    string Id { get; }
    string Range { get; }

    T Parse(ValueRange values);
}