public class Tbenutzer
{
    public int KBenutzer { get; set; }

    public string? CLogin { get; set; }

    public string? CPasswort { get; set; }

    public string? CName { get; set; }

    public int? KFirma { get; set; }

    public string? CTel { get; set; }

    public string? CEMail { get; set; }

    public string? CAbteilung { get; set; }

    public string? CFax { get; set; }

    public string? CMobil { get; set; }

    public string? CHinweis { get; set; }

    public Guid ISalt { get; set; }

    public bool NAktiv { get; set; }

    public string? CApiToken { get; set; }

    public DateTime? DPasswordLastChange { get; set; }

    public bool NPasswordNotExpire { get; set; }

    public bool NPasswordMustChange { get; set; }
}