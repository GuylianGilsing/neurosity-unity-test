using System;
using System.Threading.Tasks;
using Notion.Unity;
using UnityEngine;

public class CrownHandler : MonoBehaviour
{
    public static event Action<Epoch> OnRawBrainwavesReceived;
    public static event Action<PowerByBand> OnBrainwaveBandsReceived;
    public static event Action<Accelerometer> OnBrainwaveAccelerometerReceived;

    [SerializeField] private Device device;

    private Notion.Unity.Notion notion;

    private void OnEnable()
    {
        this.SetupCrown();
    }

    private void OnDisable()
    {
        this.Logout();
    }

    private async void SetupCrown()
    {
        await this.Login();
        this.SubscribeToRawBrainwaves();
    }

    private async Task<bool> Login()
    {
        var controller = new FirebaseController();
        await controller.Initialize();

        this.notion = new Notion.Unity.Notion(controller);
        await this.notion.Login(this.device);

        return true;
    }

    private async void Logout()
    {
        await this.notion.Logout();
        this.notion = null;
    }

    private void SubscribeToRawBrainwaves()
    {
        if (!this.notion.IsLoggedIn)
        {
            return;
        }

        this.notion.Subscribe(new BrainwavesPowerByBandHandler
        {
            OnUpdated = (PowerByBand data) => {
                OnBrainwaveBandsReceived.Invoke(data);
            },
        });

        this.notion.Subscribe(new AccelerometerHandler
        {
            OnUpdated = (Accelerometer data) => {
                OnBrainwaveAccelerometerReceived.Invoke(data);
            },
        });
    }
}
