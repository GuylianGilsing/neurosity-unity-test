using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine;

namespace Notion.Unity
{
    public class BrainwavesRawHandler : IMetricHandler
    {
        public Metrics Metric => Metrics.Brainwaves;
        public string Label => "raw";

        public Action<Epoch> OnUpdated { get; set; }

        public void Handle(string metricData)
        {
            Epoch epoch = JsonConvert.DeserializeObject<Epoch>(metricData);

            this.OnUpdated.Invoke(epoch);
        }
    }
}
