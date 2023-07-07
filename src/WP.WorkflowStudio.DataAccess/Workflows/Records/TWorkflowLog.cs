public class TWorkflowLog
{
    public int KWorkflowLog { get; set; }

    public int KObjektPk { get; set; }

    public int KWorkflowAktion { get; set; }

    public DateTime DDatum { get; set; } = DateTime.MinValue;

    public string CArbeitsplatz { get; set; } = string.Empty;

    public int KBenutzer { get; set; }

    public int NTyp { get; set; }

    public string CLog { get; set; } = string.Empty;

    public int KWorkflow { get; set; }

    public string? CObjectId { get; set; }
}