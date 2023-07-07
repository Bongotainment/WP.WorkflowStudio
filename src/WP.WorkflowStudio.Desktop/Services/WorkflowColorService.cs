using System.Collections.Generic;
using Avalonia.Media;

namespace WP.WorkflowStudio.Desktop.Services;

public class WorkflowColorService
{
    public WorkflowColorService()
    {
        NamedCategoryColors = new Dictionary<string, IBrush>();
        NamedCategoryColors["All"] = SolidColorBrush.Parse("#3763A7");
        NamedCategoryColors["Article"] = SolidColorBrush.Parse("#FAC427");
        NamedCategoryColors["Procurement"] = SolidColorBrush.Parse("#CF3232");
        NamedCategoryColors["Customers"] = SolidColorBrush.Parse("#F97C26");
        NamedCategoryColors["Orders"] = SolidColorBrush.Parse("#32B926");
        NamedCategoryColors["Delivery"] = SolidColorBrush.Parse("#F6C292");
        NamedCategoryColors["Returns"] = SolidColorBrush.Parse("#b32922");
        NamedCategoryColors["Invoice"] = SolidColorBrush.Parse("#0055b3");
        NamedCategoryColors["Picklist"] = SolidColorBrush.Parse("#E226B3");
        NamedCategoryColors["Offer"] = SolidColorBrush.Parse("#8AC349");
        NamedCategoryColors["Servicedesk"] = SolidColorBrush.Parse("#F68A25");

        CategoryIdColors = new Dictionary<int, IBrush>();
        CategoryIdColors[1] = NamedCategoryColors["Article"];
        CategoryIdColors[2] = NamedCategoryColors["Article"];
        CategoryIdColors[11] = NamedCategoryColors["Article"];

        CategoryIdColors[3] = NamedCategoryColors["Procurement"];
        CategoryIdColors[4] = NamedCategoryColors["Procurement"];
        CategoryIdColors[5] = NamedCategoryColors["Customers"];
        CategoryIdColors[6] = NamedCategoryColors["Orders"];
        CategoryIdColors[7] = NamedCategoryColors["Delivery"];
        CategoryIdColors[16] = NamedCategoryColors["Delivery"];
        CategoryIdColors[8] = NamedCategoryColors["Invoice"];
        CategoryIdColors[9] = NamedCategoryColors["Invoice"];
        CategoryIdColors[10] = NamedCategoryColors["Picklist"];
        CategoryIdColors[18] = NamedCategoryColors["Picklist"];
        CategoryIdColors[12] = NamedCategoryColors["Offer"];
        CategoryIdColors[20] = NamedCategoryColors["Servicedesk"];
        CategoryIdColors[14] = NamedCategoryColors["Returns"];
        CategoryIdColors[15] = NamedCategoryColors["Returns"];
    }

    private Dictionary<string, IBrush> NamedCategoryColors { get; }
    private Dictionary<int, IBrush> CategoryIdColors { get; }

    public Dictionary<string, IBrush> GetNamedCategoryColors()
    {
        return NamedCategoryColors;
    }

    public Dictionary<int, IBrush> GetCategoryIdColors()
    {
        return CategoryIdColors;
    }
}