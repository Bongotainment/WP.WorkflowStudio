# WP.WorkflowStudio - Deutsch

## Ein Workflow visualisierungstool
[DOWNLOAD - BETA](/downloads/WorkflowStudio.zip)
![ArticleEvents](/images/articleevents_ws.png)



### Einleitung
Workflows und Events in der JTL Warenwirtschaft können sich gegenseitig aufrufen und zeitverzögert ausgeführt werden. Um die internen Abläufe innerhalb der eigenen Warenwirtschaft zu verstehen, kann es sinnvoll sein, die Struktur der Workflows visualisiert vor Augen zu haben.
Mit WorkflowStudio können diese internen Prozesse anschaulich dargestellt werden, um ungenutzte Workflows zu identifizieren und Abhängigkeiten aufzudecken.

### Verbundene Workflows
Workflows können andere Events aufrufen und in vielen Fällen ist dies ein wünschenswerter Umstand. Allerdings kann es bei langen Aufrufketten zu Verwirrung führen, wenn man diese Verbindungen nicht im Blick hat.
In WorkflowStudio werden Workflow-Eventverbindungen durch eine Verbindungslinie dargestellt. Das aufrufende Ende wird durch einen gelben Punkt und das aufgerufene Ende durch einen magentafarbenen Punkt visualisiert. So können Sie auf einen Blick sehen, welche Workflows miteinander verbunden sind.

![ArticleEvents](/images/simpleConnection.png)

### Zeitverzögerte Workflows
Workflows, die zeitverzögert ausgeführt werden, laufen nicht auf dem Clientsystem, sondern auf dem JTL Worker System. Dies ist besonders wichtig, wenn Dateien geschrieben werden oder externe Applikationen ausgeführt werden müssen, die sich ebenfalls auf dem JTL Worker System befinden.
Die Daten für zeitverzögerte Workflows werden in einer separaten Tabelle in der JTL-Datenbank gespeichert. Wenn mehrere Workflows sich gegenseitig aufrufen und jeweils zeitverzögert sind, kann dies zu längeren Wartezeiten führen.
In der Visualisierungsansicht von WorkflowStudio werden zeitverzögerte Workflows durch eine Uhr markiert. So können Sie auf einen Blick erkennen, welche Workflows zeitverzögert sind und welche nicht.

![ArticleEvents](/images/delayed.png)

### Deaktivierte Workflows:
Deaktivierte Workflows werden von der JTL WaWi und dem JTL Worker nicht ausgeführt. Wenn ein Workflow deaktiviert ist, wird sein Name in hellgrauer Schrift dargestellt. Dadurch können Sie auf einen Blick erkennen, welche Workflows derzeit deaktiviert sind und nicht ausgeführt werden.

![ArticleEvents](/images/deactivated.png)

### Langzeit inaktive Workflows
Wenn ein Workflow längere Zeit nicht im Workflow-Log auftaucht, wird er als inaktiv markiert und durch eine Schneeflocke gekennzeichnet. Dies bedeutet nicht automatisch, dass der Workflow nicht mehr verwendet wird oder ungenutzt ist. Es ist jedoch ein Hinweis darauf, dass Sie überprüfen sollten, ob dieser Workflow noch relevant ist.
Falls Sie Ihr Workflow-Log regelmäßig leeren, können Sie dieses Kennzeichen ignorieren. Ansonsten sollten Sie sich die Zeit nehmen, inaktive Workflows zu überprüfen und gegebenenfalls zu entfernen oder zu aktualisieren.

![ArticleEvents](/images/inactive.png)

### Workflows mit erweiterten Eigenschaften
Für einen Workflow können Bedingungen definiert werden, die nicht im Standard enthalten sind. Diese erweiterten Eigenschaften können jedoch bei einem Update auf eine neuere Version der JTL Wawi zu einem Problem werden, insbesondere wenn sie eingebettete SQL-Abfragen enthalten.
Workflows mit erweiterten Eigenschaften werden in WorkflowStudio mit einer anderen Schriftfarbe gekennzeichnet, um sofort darauf hinzuweisen, dass bei einem JTL-Update vorher kontrolliert werden sollte, ob die entsprechenden Felder oder Abfragen auch nach dem Update noch vorhanden sind und ordnungsgemäß funktionieren. Es wird empfohlen, diese Workflows vor jedem Update sorgfältig zu überprüfen, um Probleme zu vermeiden.
![ArticleEvents](/images/advancedproperty_ws.png)
![ArticleEvents](/images/advancedproperty.png) 


### Benutzeroberfläche
Die Navigation zu den verschiedenen Workflowkategorien befindet sich auf der rechten Seite und orientiert sich an der Unterteilung, die sich auch in der JTL WaWi findet. Die Kategorien sind farblich unterschiedlich gestaltet, um eine Unterscheidung zwischen den Events in der Visualisierungsansicht im linken Bereich der Anwendung zu ermöglichen.

![ArticleEvents](/images/WorkflowstudioMenu.png)

### Warnung
Bitte beachten Sie, dass Workflowstudio sich in einer sehr frühen Phase der Entwicklung befindet und den Beta-Status noch nicht verlassen hat. Wir weisen ausdrücklich darauf hin, dass es sich bei der genutzten Beta-Version um ein unfertiges Softwareprodukt handelt. Wir übernehmen daher keine Haftung für mögliche Fehlfunktionen der Software und hieraus resultierende Schäden.

## Verwendung

Laden Sie die Datei [WorkflowStudio.zip](/downloads/WorkflowStudio.zip) herunter und extrahieren Sie die Inhalte. Anschließend können Sie WP.WorkflowStudio.Desktop.exe öffnen und sich mit Ihren Zugangsdaten zum SQL Server anmelden. Nach erfolgreichem Login können Sie über das rechte Menü die verfügbaren Workflows einsehen

![ArticleEvents](/images/login_ws.png)

Einige Workflows greifen auf Ereignisse aus anderen Kategorien zu. Beispielsweise können Sie ein Ereignis aus den Retouren in den Aufträgen aufrufen. Um dies zu veranschaulichen, klicken Sie auf "Alle". Sie können einen solchen Aufruf anhand der unterschiedlichen Umrandungen der Ereignisse erkennen.

![ArticleEvents](/images/connectionFromReturnToOrder.png)


# WP.WorkflowStudio - English:

## A Workflow Visualization Tool

### Introduction
Workflows and events in the JTL Wawi can call each other and be executed with a time delay. To understand the internal processes within your own System, it can be helpful to have a visualization of the structure of the workflows. With WorkflowStudio, these internal processes can be presented visually to identify unused workflows and uncover dependencies.

### Connected Workflows
Workflows can call other events, and in many cases, this is desirable. However, long chains of calls can lead to confusion if these connections are not clear. In WorkflowStudio, workflow-event connections are represented by a connection line. The calling end is visualized by a yellow dot, and the called end is visualized by a magenta-colored dot. This allows you to see at a glance which workflows are connected to each other.

![ArticleEvents](/images/simpleConnection.png)

### Time-delayed Workflows
Workflows that are executed with a time delay do not run on the client system but on the JTL worker system. This is particularly important when files need to be written or external applications need to be executed, which are also located on the JTL worker system. The data for time-delayed workflows is stored in a separate table in the JTL database. If several workflows call each other and are each time-delayed, this can lead to longer waiting times. In the visualization view of WorkflowStudio, time-delayed workflows are marked with a clock icon. This allows you to see at a glance which workflows are time-delayed and which are not.

![ArticleEvents](/images/delayed.png)

### Deactivated Workflows
Deactivated workflows are not executed by the JTL WaWi and the JTL worker. If a workflow is deactivated, its name is displayed in light gray font. This allows you to see at a glance which workflows are currently deactivated and not being executed.

![ArticleEvents](/images/deactivated.png)

### Inactive Workflows
If a workflow does not appear in the workflow log for a long time, it is marked as inactive and labeled with a snowflake icon. This does not automatically mean that the workflow is no longer used or unused. However, it is an indication that you should check whether this workflow is still relevant. If you regularly clear your workflow log, you can ignore this label. Otherwise, you should take the time to review inactive workflows and remove or update them if necessary.

![ArticleEvents](/images/inactive.png)

### Workflows with Advanced Properties
Conditions can be defined for a workflow that are not included in the standard. However, these advanced properties can become a problem when updating to a newer version of the JTL WaWi, especially if they contain embedded SQL queries. Workflows with advanced properties are marked with a different font color in WorkflowStudio to immediately indicate that the corresponding fields or queries should be checked before a JTL update to ensure they are still present and functioning properly. It is recommended to carefully review these workflows before every update to avoid problems.

![ArticleEvents](/images/advancedproperty_ws.png)

### User Interface
Navigation to the different workflow categories is located on the right side and follows the same division as in the JTL WaWi. The categories are colored differently to differentiate between events in the visualization view on the left side of the application.

![ArticleEvents](/images/articleevents_ws.png)

### Warning
Please note that Workflowstudio is in a very early stage of development and has not yet left the beta status. 
We expressly point out that the beta version used is an unfinished software product. 
Therefore, we do not assume any liability for possible malfunctions of the software and resulting damages.
