using Sitecore.Diagnostics;
using Sitecore.Text;
using Sitecore.Web;
using System;

namespace Sitecore.ContentTesting.Pipelines.GetScreenShotForURL
{
  [Sitecore.UsedImplicitly]
  public class LoadUrl : GenerateScreenShotProcessor
  {
    public override void Process(GetScreenShotForURLArgs args)
    {
      Sitecore.Diagnostics.Assert.IsNotNull(args, "args");
      Sitecore.Text.UrlString urlString = new Sitecore.Text.UrlString(args.UrlParameters)
      {
        Path = "/"
      };
      args.Url = Sitecore.Web.WebUtil.GetServerUrl() + urlString.GetUrl();
    }
  }
}