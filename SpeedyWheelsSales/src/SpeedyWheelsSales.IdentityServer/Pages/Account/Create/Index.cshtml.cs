// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Security.Claims;
using Domain.Entities;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SpeedyWheelsSales.IdentityServer.Pages.Account.Create;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly SignInManager<AppUser> _signInManager;

    [BindProperty] public InputModel Input { get; set; } = default!;

    public Index(
        IIdentityServerInteractionService interaction,
        SignInManager<AppUser> signInManager)
    {
        _interaction = interaction;
        _signInManager = signInManager;
    }

    public IActionResult OnGet(string? returnUrl)
    {
        Input = new InputModel { ReturnUrl = returnUrl };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // the user clicked the "cancel" button
        if (Input.Button != "create")
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                return Redirect(Input.ReturnUrl ?? "~/");
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }

        if (await _signInManager.UserManager.FindByNameAsync(Input.Username) != null)
        {
            ModelState.AddModelError("Input.Username", "This username has already taken.");
        }

        if (await _signInManager.UserManager.Users
                .FirstOrDefaultAsync(x => x.PhoneNumber == Input.PhoneNumber) != null)
        {
            ModelState.AddModelError("Input.Username", "This phone number has already used.");
        }

        if (ModelState.IsValid)
        {
            var user = new AppUser()
            {
                UserName = Input.Username,
                PhoneNumber = Input.PhoneNumber,
                Name = Input.Name
            };

            var result = await _signInManager.UserManager.CreateAsync(user, Input.Password);

            var claims = new List<Claim>
            {
                new Claim("username", user.UserName)
            };

            var isuser = new IdentityServerUser(user.Id)
            {
                DisplayName = user.UserName,
                AdditionalClaims = claims
            };

            await HttpContext.SignInAsync(isuser);

            if (context != null)
            {
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                // we can trust Input.ReturnUrl since GetAuthorizationContextAsync returned non-null
                return Redirect(Input.ReturnUrl ?? "~/");
            }

            if (Input.ReturnUrl != null)
            {
                return Redirect(Input.ReturnUrl);
            }
            else
            {
                return Redirect("~/");
            }

            // // request for a local page
            // if (Url.IsLocalUrl(Input.ReturnUrl))
            // {
            //     return Redirect(Input.ReturnUrl);
            // }
            // else if (string.IsNullOrEmpty(Input.ReturnUrl))
            // {
            //     return Redirect("~/");
            // }
            // else
            // {
            //     // user might have clicked on a malicious link - should be logged
            //     throw new ArgumentException("invalid return URL");
            // }
        }

        return Page();
    }
}