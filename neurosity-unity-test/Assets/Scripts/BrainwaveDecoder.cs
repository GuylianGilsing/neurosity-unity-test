using System;
using System.Linq;
using Notion.Unity;

public class BrainwaveDecoder
{
    public static RawBrainWaves decodeFromEpoch(Epoch epoch)
    {
        RawBrainWaves rawBrainWaves = new();

        // Map the format below to a DTO
        // https://docs.neurosity.co/docs/api/brainwaves
        for (int i = 0; i < epoch.Info.ChannelNames.Count(); i += 1)
        {
            String channelName = epoch.Info.ChannelNames[i];
            Decimal[] values = new decimal[epoch.Data.Length];

            for (int x = 0; x < epoch.Data.Length; x += 1)
            {
                values[x] = epoch.Data[x][i];
            }

            switch (channelName)
            {
                case "CP3":
                    rawBrainWaves.CP3 = values;
                    break;

                case "C3":
                    rawBrainWaves.C3 = values;
                    break;

                case "F5":
                    rawBrainWaves.F5 = values;
                    break;

                case "PO3":
                    rawBrainWaves.PO3 = values;
                    break;

                case "PO4":
                    rawBrainWaves.PO4 = values;
                    break;

                case "F6":
                    rawBrainWaves.F6 = values;
                    break;

                case "C4":
                    rawBrainWaves.C4 = values;
                    break;

                case "CP4":
                    rawBrainWaves.CP4 = values;
                    break;
            }
        }

        return rawBrainWaves;
    }

    public static BrainWaveBands decodeFromPowerByBand(PowerByBand powerByBand)
    {
        BrainWaveBands bands = new(
            alpha: powerByBand.Data.Alpha,
            beta: powerByBand.Data.Beta,
            delta: powerByBand.Data.Delta,
            gamma: powerByBand.Data.Gamma,
            theta: powerByBand.Data.Theta
        );

        return bands;
    }
}
