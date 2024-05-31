using Newtonsoft.Json;
using System;

namespace Notion.Unity
{
    public class AccelerometerHandler : IMetricHandler
    {
        public Metrics Metric => Metrics.Accelerometer;
        public string Label => string.Empty;

        public Action<Accelerometer> OnUpdated { get; set; }

        public void Handle(string metricData)
        {
            Accelerometer accelerometer = JsonConvert.DeserializeObject<Accelerometer>(metricData);

            OnUpdated.Invoke(accelerometer);
        }
    }
}
