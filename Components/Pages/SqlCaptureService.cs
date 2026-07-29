namespace QuickGridDemo.Components.Pages;

public class SqlCaptureService
{
    public string? IdsQuerySql { get; set; }
    public string? DataQuerySql { get; set; }
    public string? CursorLookupSql { get; set; }
    public long IdsQueryMs { get; set; }
    public long DataQueryMs { get; set; }
    public int IdsQueryRows { get; set; }
    public string? Strategy { get; set; }

    public void Reset()
    {
        IdsQuerySql = null;
        DataQuerySql = null;
        CursorLookupSql = null;
        IdsQueryMs = 0;
        DataQueryMs = 0;
        IdsQueryRows = 0;
        Strategy = null;
    }
}
