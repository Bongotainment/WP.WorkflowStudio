using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WP.WorkflowStudio.Desktop.Services;

public class WawiDbDocumentationService
{
    //Currently not used. 
    //Will be used if the CustomWorkflows are implemented and the JTL Documentation is needed.
    //Needs to be saved into the database so that the JTL Servers arent constantly bombarded with requests!!!
    public WawiDbDocumentationService()
    {
        InitializeService();
    }

    public DocumentationRoot? Root { get; private set; }

    private void InitializeService()
    {
        var web = new HtmlWeb();

        // get current version and refactor
        var doc = web.Load("https://wawi-db.jtl-software.de/tables/1.6.47.0");
        var rows = doc.DocumentNode.SelectSingleNode("//tables-page").OuterHtml;

        var regex = new Regex(@"\:cv=""\{(.+?)\}""");
        var match = regex.Match(rows);
        if (match.Success)
        {
            var jsonString = WebUtility.HtmlDecode("{" + match.Groups[1].Value + "}");

            var jsonObject = JObject.Parse(jsonString);
            var tables = JsonConvert.DeserializeObject<DocumentationRoot>(jsonString);
            if (tables == null) return;

            AddColumnInformations(tables, web);
            Root = tables;
        }
    }

    private static void AddColumnInformations(object tables, HtmlWeb web)
    {
        // foreach (DocumentationEntry entry in tables.Tables)
        // {
        //     var entryJson = web.Load($"https://wawi-db.jtl-software.de/int/table-columns/{entry.Id}");
        //     var values = JsonConvert.DeserializeObject<List<DocumentationColumn>>(entryJson.DocumentNode.InnerHtml);
        //     entry.Columns = values;
        // }
    }
}