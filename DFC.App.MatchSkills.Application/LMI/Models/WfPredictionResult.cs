using System.ComponentModel;

namespace DFC.App.MatchSkills.Application.LMI.Models
{
    public enum JobGrowth
    {
        Undefined,
        Increasing,
        Decreasing
    }

    public enum Region
    {
        [Description("London")]
        London = 1,
        [Description("South East (England)")]
        SouthEast = 2,
        [Description("East of England")]
        EastOfEngland = 3,
        [Description("South West (England)")]
        SouthWest = 4,
        [Description("West Midlands (England)")]
        WestMidlands = 5,
        [Description("East Midlands (England)")]
        EastMidlands = 6,
        [Description("Yorkshire and the Humber")]
        YorkshireAndTheHumber = 7,
        [Description("North West (England)")]
        NorthWest = 8,
        [Description("North East (England)")]
        NorthEast = 9,
        [Description("Wales")]
        Wales = 10,
        [Description("Scotland")]
        Scotland = 11,
        [Description("Northern Ireland")]
        NorthernIreland = 12

    }
    public class WfPredictionResult
    {
        public long Soc { get; set; }

        public string Note { get; set; }

        public string Breakdown { get; set; }

        public PredictedEmployment[] PredictedEmployment { get; set; }
        


    }
    public class PredictedEmployment
    {
        public int Year { get; set; }

        public Breakdown[] Breakdown { get; set; }
    }

    public class Breakdown
    {
        public Region Code { get; set; }

        public string Note { get; set; }

        public string Name { get; set; }

        public int Employment { get; set; }
    }
}
