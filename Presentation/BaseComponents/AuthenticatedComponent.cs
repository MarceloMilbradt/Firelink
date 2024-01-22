using Microsoft.AspNetCore.Components;
using Mediator;
using Firelink.Application.Auth.Queries.GetUserLoginStatus;

namespace Firelink.Presentation.BaseComponents;

public class AuthenticatedComponent : ComponentBase
{
    public bool UserIsLoggedIn { get; private set; }
    [Inject]
    protected ISender Sender { get; set; } = default!;
    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {

        UserIsLoggedIn = await Sender.Send(GetUserLoginStatusQuery.Default);
        if (!UserIsLoggedIn)
        {
            NavigationManager.NavigateTo("login", forceLoad: true);
        }
    }
}
