using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace XMLFilter_Classes
{
    public class Filter
    {
        public IEnumerable<XmlNode> FilterNodes(XmlDocument xmlDocument, string childNodeName, string exclusionMask)
        {
            IEnumerable<XmlNode> moduleNodes = FilterNodes("Module", xmlDocument.DocumentElement);
            return FilterByName(moduleNodes, exclusionMask);
        }

        private IEnumerable<XmlNode> FilterByName(IEnumerable<XmlNode> moduleNodes, string stringEnding)
        {
            foreach (XmlNode moduleNode in moduleNodes)
            {
                if (FullfillsNameCondition(stringEnding, moduleNode))
                {
                    yield return moduleNode;
                }
            }
        }

        private bool FullfillsNameCondition(string stringEnding, XmlNode moduleNode)
        {
            bool fulfillsCondition = false;
            foreach (XmlNode childNode in moduleNode)
            {
                if (NodeNameHasValue(childNode, "ModuleName") && !Regex.Match(childNode.InnerText, @stringEnding + "$").Success)
                {
                    fulfillsCondition = true;
                }
            }
            return fulfillsCondition;
        }

        public IEnumerable<XmlNode> FilterNodes(string nodeName, XmlNode rootNode)
        {
            ThrowExceptionIfNodeHasNoChildren(rootNode);
            foreach (XmlNode childNode in rootNode)
            {
                if (NodeNameHasValue(childNode, nodeName))
                {
                    yield return childNode;
                }
            }
        }

        private void ThrowExceptionIfNodeHasNoChildren(XmlNode rootNode)
        {
            if (!rootNode.HasChildNodes)
            {
                throw new Exception("Root node" + rootNode.Name + "has no children. Coverage not computed.");
            }
        }
        
        //TODO is it part of filtering or reading? or is it a filtering condition?
        public bool NodeNameHasValue(XmlNode childNode, string name)
        {
            return childNode.Name == name;
        }
    }
}
