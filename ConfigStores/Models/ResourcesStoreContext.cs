namespace IdentityServer.Web.ConfigStores.Models
{
  using System.Collections.Generic;

  public class ApiResourcesStoreContext
  {
    public ICollection<Scope> Scopes { get; set; }
  }

  public class Scope
  {
    public string Name { get; set; }
    public bool Required { get; set; }
    public bool Emphasize { get; set; }
    public ICollection<Values> Claims { get; set; }
  }

  public class Identity
  {
    public ICollection<Resource> Resources { get; set; }
  }

  public class Resource
  {
    public string Name { get; set; }
    public ICollection<Values> ClaimTypes { get; set; }
  }
}
