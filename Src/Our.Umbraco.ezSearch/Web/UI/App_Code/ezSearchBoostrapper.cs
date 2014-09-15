using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Examine;
using Umbraco.Core;

namespace Our.Umbraco.ezSearch
{
    public class ezSearchBoostrapper : IApplicationEventHandler
    {
        #region Application Event Handlers

        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, 
            ApplicationContext applicationContext)
        { }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, 
            ApplicationContext applicationContext)
        { }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, 
            ApplicationContext applicationContext)
        {
            ExamineManager.Instance.IndexProviderCollection["ExternalIndexer"]
                .GatheringNodeData += OnGatheringNodeData;
        }

        #endregion

        private void OnGatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            // Create searchable path
            if (e.Fields.ContainsKey("path"))
            {
                e.Fields["searchPath"] = e.Fields["path"].Replace(',', ' ');
            }

            // Extract the filename from media items
            if(e.Fields.ContainsKey("umbracoFile"))
            {
                e.Fields["umbracoFileName"] = Path.GetFileName(e.Fields["umbracoFile"]);
            }

            // Stuff all the fields into a single field for easier searching
            var combinedFields = new StringBuilder();
            foreach (var keyValuePair in e.Fields)
            {
                combinedFields.AppendLine(keyValuePair.Value);
            }
            e.Fields.Add("contents", combinedFields.ToString());
        }
    }
}
