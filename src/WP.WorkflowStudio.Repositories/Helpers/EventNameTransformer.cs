namespace WP.WorkflowStudio.Repositories.Helpers;

internal class EventNameTransformer
{
    //If the workflow object needs to be changed and saved this method needs to change
    //the big switch is currently the easiest solution. Not clean but easy enough
    //to be understood.
    internal string GetEventName(int nObject, int nEvent)
    {
        var eventName = string.Empty;

        switch (nObject)
        {
            case 1:
                eventName = GetArticleEventName(nEvent);
                break;
            case 2:
                eventName = GetWarenlagerAusgangEventName(nEvent);
                break;
            case 3:
                eventName = GetLieferantenbestellungEventName(nEvent);
                break;
            case 4:
                eventName = GetEingangsrechnungEventName(nEvent);
                break;
            case 5:
                eventName = GetKundeEventName(nEvent);
                break;
            case 6:
                eventName = GetAuftragEventName(nEvent);
                break;
            case 7:
                eventName = GetLieferscheinEventName(nEvent);
                break;
            case 8:
                eventName = GetRechnungEventName(nEvent);
                break;
            case 9:
                eventName = GetGutschriftEventName(nEvent);
                break;
            case 10:
                eventName = GetPicklisteEventName(nEvent);
                break;
            case 11:
                eventName = GetWarenlagerEingangEventName(nEvent);
                break;
            case 12:
                eventName = GetAngebotEventName(nEvent);
                break;
            case 13:
                eventName = "Not implemented"; //Kategorie
                break;
            case 14:
                eventName = GetRetourePositionEventName(nEvent);
                break;
            case 15:
                eventName = GetRetoureEventName(nEvent);
                break;
            case 16:
                eventName = GetVersandOrPaketeEventName(nEvent);
                break;
            case 17:
                eventName = "Not implemented"; //Umlagerung
                break;
            case 18:
                eventName = GetPickEventName(nEvent);
                break;
            case 19:
                eventName = "Not implemented"; //Lagerplatz
                break;
            case 20:
                eventName = GetTicketEventName(nEvent);
                break;
            case 61:
                eventName = "Not implemented"; //FFN
                break;
        }

        return eventName;
    }

    private static string GetLieferantenbestellungEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 0:
                return "Erstellt";
            case 1:
                return "Abgeschlossen";
            case 2:
                return "Freigegeben";
            case 3:
                return "In Bearbeitung";
            case 5:
                return "Ist bestätigt";
            case 6:
                return "Lieferantenbestellungsposition reduziert";
        }

        return string.Empty;
    }

    private static string GetEingangsrechnungEventName(int nEvent)
    {
        return "Erstellt";
    }

    private static string GetKundeEventName(int nEvent)
    {
        if (nEvent == 1)
            return "Angelegt";
        if (nEvent == 2) return "Geändert";

        return string.Empty;
    }

    private static string GetAuftragEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 1:
                return "Erstellt";
            case 2:
                return "Gelöscht";
            case 3:
                return "Geändert";
            case 4:
                return "Ausgeliefert";
            case 5:
                return "Storniertt";
            case 6:
                return "Zusammengefasst";
            case 7:
                return "Abgesplittet";
            case 9:
                return "Ohne Versand abgeschlossen";
            case 10:
                return "Komplett bezahlt";
        }

        return string.Empty;
    }

    private static string GetLieferscheinEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 1:
                return "Erstellt";
            case 2:
                return "Gelöscht";
            case 3:
                return "Versendet";
        }

        return string.Empty;
    }

    private static string GetRechnungEventName(int nEvent)
    {
        if (nEvent == 1)
            return "Erstellt";
        if (nEvent == 3) return "Rechnung komplett bezahlt";

        return string.Empty;
    }

    private static string GetGutschriftEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 1:
                return "Erstellt";
            case 2:
                return "Gelöscht";
            case 3:
                return "Geändert";
        }

        return string.Empty;
    }

    private static string GetPicklisteEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 1:
                return "Erstellt";
            case 2:
                return "Menge verringert";
            case 3:
                return "Gepickt mit Mobile App";
            case 4:
                return "Abgeschlossen";
        }

        return string.Empty;
    }

    private static string GetWarenlagerEingangEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 1:
                return "Erstellt";
            case 2:
                return "Gelöscht";
            case 3:
                return "Geändert";
        }

        return string.Empty;
    }

    private static string GetAngebotEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 1:
                return "Erstellt";
            case 2:
                return "Gelöscht";
            case 3:
                return "Geändert";
        }

        return string.Empty;
    }

    private static string GetRetourePositionEventName(int nEvent)
    {
        return "";
    }

    private static string GetRetoureEventName(int nEvent)
    {
        return "";
    }

    private static string GetVersandOrPaketeEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 1:
                return "Paket Erstellt";
            case 2:
                return "Sendungsstatus aktualisiert";
            case 3:
                return "Versanddatenexport fehlgeschlagen";
        }

        return string.Empty;
    }

    private static string GetPickEventName(int nEvent)
    {
        if (nEvent == 1) return "Gepickt mit Mobile App";

        return string.Empty;
    }

    private static string GetTicketEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 1:
                return "Erstellt";
            case 2:
                return "Gelöscht";
            case 3:
                return "Geändert";
        }

        return string.Empty;
    }

    private static string GetWarenlagerAusgangEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 1:
                return "Mobile App Fehlmenge ausbuch";
            case 2:
                return "Minusbuchung";
        }

        return string.Empty;
    }

    private static string GetArticleEventName(int nEvent)
    {
        switch (nEvent)
        {
            case 1:
                return "Erstellt";
            case 2:
                return "Geändert";
            case 3:
                return "Gelöscht";
        }

        return string.Empty;
    }
}