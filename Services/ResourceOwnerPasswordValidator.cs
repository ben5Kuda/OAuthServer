using IdentityServer4.Models;
using IdentityServer4.Validation;
using IdSrvr4Demo.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdSrvr4Demo.Services
{
  public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
  {
    private readonly IUserRepository _userRepository;  
    private readonly ILogger _logger;

    public ResourceOwnerPasswordValidator(IUserRepository userRepository, ILoggerFactory loggerFactory)
    {
      _userRepository = userRepository;
      _logger = loggerFactory.CreateLogger("ResourceOwnerPasswordValidator");
    }

    async Task IResourceOwnerPasswordValidator.ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
      await ValidatePassword(context);
    }

    public async Task<ResourceOwnerPasswordValidationContext> ValidatePassword(ResourceOwnerPasswordValidationContext context)
    {

      var isValidCredentials = _userRepository.ValidateCredentials(context.UserName, context.Password);
      if (!isValidCredentials)
      {
        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
        return context;
      }

      var user = _userRepository.FindUserByUsernameOrEmail(context.UserName);

      if (user != null)
      {
        // account is locked
        if (user.IsLockedOut)
        {
          context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Account Locked out");
          return context;
        }
      
        //user exists but incorrect password
        if (!isValidCredentials)
        {         

          if (user.PasswordRetryCount < 3)
          {
            user.PasswordRetryCount = user.PasswordRetryCount + 1;
            _userRepository.Update(user);

             _userRepository.Save();
          }

          if (user.PasswordRetryCount >= 3)
          {
            user.IsLockedOut = true;

            _userRepository.Update(user);
            _userRepository.Save();
          }

          context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
          return context;
        }
      
        if (isValidCredentials)
        {
          user.PasswordRetryCount = 0;
          user.IsLockedOut = false;

          _userRepository.Update(user);
          _userRepository.Save();

          //set the result
          context.Result = new GrantValidationResult(
                    subject: user.UsersId.ToString(),
                    identityProvider: "sso",
                    authenticationMethod: "password",
                    claims: null);

          await Task.Factory.StartNew(() => _logger.LogInformation("Password validation completed"));
        
        }
      }

      return context;
    }
    }
  }
