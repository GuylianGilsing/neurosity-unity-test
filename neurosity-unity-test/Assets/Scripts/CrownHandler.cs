using System;
using System.Threading.Tasks;
using Notion.Unity;
using UnityEngine;

public class CrownHandler : MonoBehaviour
{
    public static event Action<Epoch> OnRawBrainwavesReceived;

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

        this.notion.Subscribe(new BrainwavesRawHandler
        {
            OnUpdated = (Epoch epoch) => {
                OnRawBrainwavesReceived.Invoke(epoch);
            },
        });
    }
}
