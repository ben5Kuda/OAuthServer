using IdentityServer4.Models;
using IdentityServer4.Validation;
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
    private readonly Context.SsoDbContext _userRepository;  
    private readonly ILogger _logger;

    public ResourceOwnerPasswordValidator(Context.SsoDbContext userRepository, ILoggerFactory loggerFactory)
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

      var user = (from u in _userRepository.Users
                        where u.Username == context.UserName
                        select u).FirstOrDefault();

      if (user == null)
      {
        // invalid username or password
        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
        return context;
      }

      if (user != null)
      {
        // account is locked
        if (user.IsLockedOut)
        {
          context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Account Locked out");
          return context;
        }

        var isValidPassword = user.Password == context.Password;

        //user exists but incorrect password
        if (!isValidPassword)
        {         

          if (user.PasswordRetryCount < 3)
          {
            user.PasswordRetryCount = user.PasswordRetryCount + 1;
            _userRepository.Entry(user).State = EntityState.Modified;

            await _userRepository.SaveChangesAsync();
          }

          if (user.PasswordRetryCount >= 3)
          {
            user.IsLockedOut = true;

            _userRepository.Entry(user).State = EntityState.Modified;
            await _userRepository.SaveChangesAsync();
          }

          context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid username or password");
          return context;
        }
      
        if (isValidPassword)
        {
          user.PasswordRetryCount = 0;
          user.IsLockedOut = false;

          _userRepository.Entry(user).State = EntityState.Modified;
          await _userRepository.SaveChangesAsync();

          //set the result
          context.Result = new GrantValidationResult(
                    subject: user.UsersId.ToString(),
                    identityProvider: "ssodb",
                    authenticationMethod: "password",
                    claims: null);

          _logger.LogInformation("Password validation completed");
        }
      }

      return context;
    }
    }
  }
