using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherDataExam.Model.Entities
{
    public class Values : WeatherData
    {
        public int DoorOpenMinutes { get; set; }
        public float MinDiff { get; set; }
        public float MaxDiff { get; set; }
    }
}
