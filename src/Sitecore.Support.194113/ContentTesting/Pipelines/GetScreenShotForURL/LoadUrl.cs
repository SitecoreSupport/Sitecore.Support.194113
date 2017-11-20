using Sitecore.Diagnostics;
using Sitecore.Text;
using Sitecore.Web;
using System;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Links;
using Sitecore.ContentTesting.Pipelines.GetScreenShotForURL;

namespace Sitecore.Support.ContentTesting.Pipelines.GetScreenShotForURL
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

      #region Added Code
      if (Sitecore.Configuration.Settings.GetBoolSetting("Rendering.SiteResolving", true))
      {
        Item item = Database.GetDatabase("master").GetItem(args.ItemId);
        var siteInfoList = Sitecore.Configuration.Factory.GetSiteInfoList();
        foreach (SiteInfo siteInf in siteInfoList)
        {
          if (!String.IsNullOrEmpty(siteInf.RootPath) && !String.IsNullOrEmpty(siteInf.StartItem) && item.Paths.FullPath.StartsWith(siteInf.RootPath + siteInf.StartItem))
          {
            Sitecore.Context.Site = Sitecore.Sites.SiteContext.GetSite(siteInf.Name);

            args.Url = String.Format("{0}://{1}", siteInf.Scheme, siteInf.TargetHostName) + urlString.GetUrl();
          }
        }

      }

      else

      {

        #endregion

        args.Url = Sitecore.Web.WebUtil.GetServerUrl() + urlString.GetUrl();
      }
    }
  }
}