using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine;

namespace Notion.Unity
{
    public class BrainwavesPowerByBandHandler : IMetricHandler
    {
        public Metrics Metric => Metrics.Brainwaves;
        public string Label => "powerByBand";

        public Action<PowerByBand> OnUpdated { get; set; }

        public void Handle(string metricData)
        {
            PowerByBand powerByBand = JsonConvert.DeserializeObject<PowerByBand>(metricData);

            this.OnUpdated.Invoke(powerByBand);
        }
    }
}
