using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLFilter_Classes
{
    public class Filter
    {
        public IEnumerable<XmlNode> FilterNodes(string nodeName, XmlNode rootNode)
        {
            ThrowExceptionIfNodeHasNoChildren(rootNode);
            return FilterChildNodesByName(nodeName, rootNode);
        }

        private IEnumerable<XmlNode> FilterChildNodesByName(string nodeName, XmlNode rootNode)
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
