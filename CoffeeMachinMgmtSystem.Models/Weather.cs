using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachinMgmtSystem.Models
{
    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }
    }

    public class Weather
    {
        [JsonProperty("main")]
        public Main Main { get; set; }
    }
     
}
