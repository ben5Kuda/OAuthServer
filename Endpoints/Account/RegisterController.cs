using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Quickstart.UI;
using IdentityServer4.Services;
using IdSrvr4Demo.Context;
using IdSrvr4Demo.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace IdSrvr4Demo.Endpoints.Account
{
  [SecurityHeaders]
  [AllowAnonymous]
  public class RegisterController : Controller
  {

    private readonly IIdentityServerInteractionService _interaction;
    private readonly SsoDbContext _userRepository;
    private readonly ILoggerFactory _loggerfactory = new LoggerFactory();
    private readonly IdentityServerTools _identityServerTools;



    public RegisterController(
        IIdentityServerInteractionService interaction,
        IEventService events,
        SsoDbContext userRepository,
        IdentityServerTools identityServerTools)
    {
      // if the TestUserStore is not in DI, then we'll just use the global users collection
      // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)

      _userRepository = userRepository;
      _interaction = interaction;
      _identityServerTools = identityServerTools;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Start(string button)
    {
      // var claims = new Claim[]
      // {
      //    new Claim(JwtClaimTypes.Subject, "4566"),
      //    new Claim("unique_name", "kuda"),    
      // };


      //var value = _identityServerTools.IssueJwtAsync( 3600, claims.AsEnumerable());
      //var token = value.Result;

      //   List<string> teams = new List<string>() { "Arsenal", "Burnley", "Brighton", "Bournemouth", "Cardiff", "Chelsea", "Crystal Palace", "Everton", "Fulham", "Huddersfield", "Leicester", "Liverpool" };

      RegisterViewModel model = new RegisterViewModel();
      var teams = _userRepository.Team.Select(x => x.Name).ToList();

       model.Teams = new SelectList(teams);
     // ViewData["Teams"] = model.Teams;
      return View("Create", model);
    }
    /// <summary>
    /// Handle postback from username/password login
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(RegisterViewModel model, string button)
    {

     if (button == "start")
      {
        return View();
      }

      var returnUrl = HttpContext.Request.Cookies["returnUrl"];

      if (button == "create")
      {
        if (!ModelState.IsValid)
        {
          var vm = new ErrorViewModel();
          vm.Error = new ErrorMessage
          {
            Error = "Registration failed",
            ErrorDescription = "Fields missing"
          };
          return View();
        }


        var userExists = _userRepository.Users.Where(u => u.Username == model.Username);
        var team = _userRepository.Team.Where(u => u.Name == model.FavouriteTeam).FirstOrDefault();
        if (team == null)
        {
          team.TeamId = 0;
        }
        if (userExists.Any())
        {
          var vm = new ErrorViewModel();
          vm.Error = new ErrorMessage
          {
            Error = "Registration failed",
            ErrorDescription = "User already exists"
          };

          return View("Error", vm);
        }

        var newUser = new Users
        {
          Name = model.Name,
          Surname = model.Surname,
          Username = model.Username,
          Password = model.Password,
          Profile = "User",
          Country = model.Country,
          TeamId = team.TeamId,
          Email = model.Email,
          Gender = model.Gender,
          Phone = model.PhoneNumber,
          UsersId = new Random().Next(10002, 1000000).ToString()
        };

        await _userRepository.Users.AddAsync(newUser);

        await _userRepository.SaveChangesAsync();

        SendEmail(model.Email); //decouple this

      }

      var context = await _interaction.GetAuthorizationContextAsync("/grants");
      LoginViewModel lm = new LoginViewModel();
      if (context?.IdP != null)
      {
        var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

        // this is meant to short circuit the UI and only trigger the one external IdP
        lm = new LoginViewModel
        {
          EnableLocalLogin = local,
          ReturnUrl = "/grants",
          Username = context?.LoginHint,
        };
      }

      return Redirect(Redirects.Uri); //redirect to client app => login page.
    }

    private Task<string> SendEmail(string email)
    {
      MailMessage mail = new MailMessage();
      mail.To.Add(email);

      mail.From = new MailAddress("kmkuda@gmail.com");
      mail.Subject = "Registration successful";

      mail.Body = "Thank you for registering with Ticket Market Place.";

      mail.IsBodyHtml = true;
      SmtpClient smtp = new SmtpClient();
      smtp.Host = "smtp.gmail.com"; //Or Your SMTP Server Address
      smtp.Credentials = new System.Net.NetworkCredential
           ("kmkuda@gmail.com", "00912941183"); // ***use valid credentials***
      smtp.Port = 587;

      //Or your Smtp Email ID and Password
      smtp.EnableSsl = true;
      smtp.Send(mail);

      return Task.FromResult("Completed");

    }
  }
}