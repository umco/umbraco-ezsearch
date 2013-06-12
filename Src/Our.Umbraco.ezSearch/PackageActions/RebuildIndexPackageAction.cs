using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Examine;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using umbraco.BusinessLogic;

namespace Our.Umbraco.ezSearch.PackageActions
{
    public class RebuildIndexPackageAction : IPackageAction
    {
        public string Alias()
        {
            return "RebuildIndex";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            if (xmlData == null || xmlData.Attributes == null)
                return false;

            var indexAttr = xmlData.Attributes["index"];
            if (indexAttr == null)
                return false;

            var index = xmlData.Attributes["index"].InnerText;
            if (string.IsNullOrEmpty(index))
                return false;

            if (ExamineManager.Instance.IndexProviderCollection.All(x => x.Name != index))
                return false;

            var indexer = ExamineManager.Instance.IndexProviderCollection[index];
            indexer.RebuildIndex();
            return true;
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return true;
        }

        public XmlNode SampleXml()
        {
            var sample = "<Action runat=\"install\" undo=\"true\" alias=\"RebuildIndex\" index=\"IndexName\" />";
            return helper.parseStringToXmlNode(sample);
        }
    }
}
