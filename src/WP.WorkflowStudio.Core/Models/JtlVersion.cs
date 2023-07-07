namespace WP.WorkflowStudio.Core.Models;

public class JtlVersion
{
    public JtlVersion(string versionnumber)
    {
        VersionNumber = versionnumber;
        SetVersionInformations(versionnumber);
    }

    public string VersionNumber { get; set; }
    public int Major { get; internal set; }
    public int Minor { get; internal set; }
    public int Build { get; internal set; }
    public int Revision { get; internal set; }

    private void SetVersionInformations(string versionnumber)
    {
        var numbers = versionnumber.Split(".");
        var iNumbers = new int[4];
        if (numbers.Length < 4) throw new ArgumentException("The provided string is no valid JTLVerionnumber");

        for (var i = 0; i <= numbers.Length; i++)
        {
            numbers[i] = numbers[i].Replace(".", "");
            iNumbers[i] = Convert.ToInt32(numbers[i]);
        }

        Major = iNumbers[0];
        Minor = iNumbers[1];
        Build = iNumbers[2];
        Revision = iNumbers[3];
    }
}