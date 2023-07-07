using System.Runtime.Serialization;

// add this line for each derived type see KnownTypes in jtlAktion
namespace WP.WorkflowStudio.Repositories;
#pragma warning disable CS8618
[DataContract(Name = "jtlAktion", Namespace = "WP.WorkflowStudio.Repositories")]
[KnownType("KnownTypes")]
internal abstract class jtlAktion
{
    [DataMember] public bool CancelOnError { get; set; }

    [DataMember] public string WawiVersion { get; set; }

    public static Type[] KnownTypes()
    {
        var types = new List<Type>
        {
            typeof(jtlAktionWorkflowStarten),
            typeof(jtlAktionCustomWorkflow),
            typeof(jtlAktionAusgabe)
        };
        return types.ToArray();
    }
}

[DataContract]
internal class jtlLiquidableAktion : jtlAktion
{
    [DataMember] public bool UseDotLiquidParameters { get; set; }

    [DataMember] public string? Parameters { get; set; }
}

[DataContract]
internal class jtlAktionWorkflowStarten : jtlAktion
{
    [DataMember] public string? NodePath { get; set; }

    [DataMember] public int? WorkflowEvent { get; set; }
}

[DataContract]
internal class jtlAktionCustomWorkflow : jtlAktion
{
    [DataMember] public string? ActionName { get; set; }
    //Todo
    // [DataMember]
    // public ActionParameter? ActionParameter { get; set; }
}

[DataContract]
internal class jtlAktionAusgabe : jtlLiquidableAktion
{
    [DataMember] public int Anzahl { get; set; }

    [DataMember] public string AusgabeModus { get; set; }

    [DataMember] public string Bcc { get; set; }

    [DataMember] public string Empfaenger { get; set; }

    [DataMember] public int FormularId { get; set; }

    [DataMember] public string NodePath { get; set; }

    [DataMember] public bool UseDotLiquidAnzahl { get; set; }

    [DataMember] public bool UseDotLiquidBcc { get; set; }

    [DataMember] public bool UseDotLiquidEmpfaenger { get; set; }

    [DataMember] public bool WaitForProcess { get; set; }

    [DataMember] public int WaitMS { get; set; }

    [DataMember] public bool WithPropertyCombobox { get; set; }
}
#pragma warning restore CS8618