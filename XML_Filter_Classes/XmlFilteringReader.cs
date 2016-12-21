using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;

namespace XMLFilter_Classes
{
    public class XmlFilteringReader
    {
        private Filter Filter { get; set; }


        public XmlFilteringReader(Filter filter)
        {
            Filter = filter;  
        }

        private XmlDocument LoadXmlDocument(string pathName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(pathName);
            return xmlDocument;     
        }

        public void ComputeCoverage(string pathName, string modulesToExcludeMask)
        {
            XmlDocument xmlDocument = LoadXmlDocument(pathName);
            IEnumerable<XmlNode> moduleNodes = Filter.FilterNodes("Module", xmlDocument.DocumentElement); //returneaza o lista cu toate nodurile numite  Module
            IEnumerable<ModuleCoverageInfo> moduleCoverageInfo = FilterByName(moduleNodes, modulesToExcludeMask);
            foreach (ModuleCoverageInfo mci in moduleCoverageInfo)
            {
                Console.WriteLine(mci.ModuleName);
                Reading(mci);
            }
            Console.ReadKey();
        }

        private void Reading(ModuleCoverageInfo mci)
        {
            Console.Write("\n");
            Search(mci.nodeOfModule, "ModuleName");
            foreach (XmlNode childNode in mci.nodeOfModule)
            {
                if (Filter.NodeNameHasValue(childNode, "NamespaceTable"))
                {
                    foreach (XmlNode childOfChildNode in childNode)
                    {
                        if (Filter.NodeNameHasValue(childOfChildNode, "Class"))
                        {
                            Search(childOfChildNode, "ClassName");
                        }
                    }
                }
            }
        }

        private void Search(XmlNode node, string string_to_find)
        {
            try
            {
                int noOfBlocksCovered = 0, noOfBlocksNotCovered = 0;
                string string_name = "";

                XmlDocument xmlDocument = node.OwnerDocument;
                // TODO instantiere de clasa noua
                // search sa fie facut de clasa noua
                string_name = GetNameData(xmlDocument, node, string_to_find);
                noOfBlocksCovered = GetNumberData(xmlDocument, node, "BlocksCovered");
                noOfBlocksNotCovered = GetNumberData(xmlDocument, node, "BlocksNotCovered");
                
                string_to_find = string_to_find.Remove(string_to_find.Length - 4);
                if (string_to_find == "Class")
                {
                    Console.SetCursorPosition(6, Console.CursorTop + 1);
                }
                Console.Write("{0}: {1}", string_to_find, string_name);
                Console.SetCursorPosition(50, Console.CursorTop);
                Console.Write("Blocks covered: " + String.Format("{0:0.##%}", (float)noOfBlocksCovered / (noOfBlocksCovered + noOfBlocksNotCovered)));
                if (noOfBlocksCovered != 0)
                    Console.Write(" ({0}/{1})", noOfBlocksCovered, noOfBlocksCovered + noOfBlocksNotCovered);
                //Console.Write("\n"); //to see more clear, remove comment tags. 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private int GetNumberData(XmlDocument xmlDocument, XmlNode node, string stringh)
        {
            return Int32.Parse(Filter.FilterNodes(stringh, node).First().InnerText);
        }

        private string GetNameData(XmlDocument xmlDocument, XmlNode node, string stringh)
        {
            return Filter.FilterNodes(stringh, node).First().InnerText;
        }

        private IEnumerable<ModuleCoverageInfo> FilterByName(IEnumerable<XmlNode> moduleNodes, string stringEnding)
        {
            foreach (XmlNode moduleNode in moduleNodes)
            {
                if (FullfillsNameCondition(stringEnding, moduleNode))
                {
                    ModuleCoverageInfo moduleCoverageInfo = CreateModuleCoverageInfoFromXmlNode(moduleNode);
                    yield return moduleCoverageInfo;
                }
            }
        }

        private bool FullfillsNameCondition(string stringEnding, XmlNode moduleNode)
        {
            bool fulfillsCondition = false;
            foreach (XmlNode childNode in moduleNode)
            {
                if (Filter.NodeNameHasValue(childNode, "ModuleName") && !Regex.Match(childNode.InnerText, @stringEnding + "$").Success)
                {
                    fulfillsCondition = true;
                }
            }
            return fulfillsCondition;
        }

        private ModuleCoverageInfo CreateModuleCoverageInfoFromXmlNode(XmlNode node)
        {
            ModuleCoverageInfo moduleCoverageInfo = new ModuleCoverageInfo();
            moduleCoverageInfo.ModuleName = GetNameData(node.OwnerDocument, node, "ModuleName");
            moduleCoverageInfo.noOfBlocksCovered = GetNumberData(node.OwnerDocument, node, "BlocksCovered");
            moduleCoverageInfo.noOfBlocksNotCovered = GetNumberData(node.OwnerDocument, node, "BlocksNotCovered");
            moduleCoverageInfo.nodeOfModule = node;
            return moduleCoverageInfo;
        }
    }
}
