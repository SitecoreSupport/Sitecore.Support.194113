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
          if (!String.IsNullOrEmpty(siteInf.RootPath) && !String.IsNullOrEmpty(siteInf.StartItem) && IsNeededSite(item.Paths.FullPath, siteInf.RootPath + siteInf.StartItem))
          {
            if (String.IsNullOrEmpty(siteInf.Scheme) || String.IsNullOrEmpty(siteInf.TargetHostName)) continue;
            Sitecore.Context.Site = Sitecore.Sites.SiteContext.GetSite(siteInf.Name);
            args.Url = String.Format("{0}://{1}", siteInf.Scheme, siteInf.TargetHostName) + urlString.GetUrl();
          }
        }
        if (String.IsNullOrEmpty(args.Url)) args.Url = Sitecore.Web.WebUtil.GetServerUrl() + urlString.GetUrl();
      }

      else

      {

        #endregion

        args.Url = Sitecore.Web.WebUtil.GetServerUrl() + urlString.GetUrl();
      }
    }
    private bool IsNeededSite(string itemPath, string sitePath)
    {
      string[] itemPathArr = itemPath.Replace("//","/").Split('/');
      string[] sitePathArr = sitePath.Replace("//","/").Split('/');

      for (int i = 0; i < sitePathArr.Length; i++)
      {
        if (itemPathArr[i].ToLowerInvariant() != sitePathArr[i].ToLowerInvariant()) return false;
      }
      return true;
    }
  }
}