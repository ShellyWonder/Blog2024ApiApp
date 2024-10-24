using System.ComponentModel;

namespace Blog2024ApiApp.Enums
{
    public enum PostState
    {
        [Description("Production Ready")]
        ProductionReady,


        [Description("In Process")]
        InDevelopment,

        [Description("Awaiting Preview")]
        PreviewReady
    }
}

