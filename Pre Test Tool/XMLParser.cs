using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pre_Test_Tool
{
    class XMLParser
    {
        public Regex reRoot = new Regex(@"(?<item><Item\d+ (?<properties>[^/>]*)/?>)\s*(?<children><Children>[\s\S]*</Children>)?");
        public Regex reProps = new Regex("(?<name>\\w*)=\"(?<value>[^\"]*)\"");
        public Regex reItemsNChildren = new Regex("(?<item><Item\\d+ (?<properties>[^/>]*)/?>)\\s*(<Children>(?<children>([\\s\\S](?!</Children>))+))?");

        public XMLItem rootItem;
 
        public XMLParser(String documentPath)
        {
            String text = System.IO.File.ReadAllText(documentPath);

            this.rootItem = new XMLItem();
            MatchCollection i = reRoot.Matches(text);

            foreach (Match match in i)
            {
                GroupCollection groups = match.Groups;

                String props = groups["properties"].Value;
                _setProps(rootItem, props);

                // Create and Add Children
                String childrenText = groups["children"].Value;
                _setChildren(rootItem, childrenText);
            }

            // Set Connections
            List<String> conns = getAllConnections();
            foreach(String s in conns)
            {
                List<String> ls = Regex.Split(s, "->").ToList();
                XMLItem a = rootItem._getComponentByName(ls[0]);
                XMLItem b = rootItem._getComponentByName(ls[1]);

                a.outGoings.Add(b);
                b.inComings.Add(a);
            }
        }

        public String show()
        {
            String str = "";
            List<String> ls = getAllComponents(" >");
            foreach(String s in ls) { str += s + "\n"; } str += "\n\n\n\n";
            double sum = 0;
            decimal count = 0;
            int con = 0 , allCon;


            sum = rootItem.children.Count - getAllConnections().Count;


            foreach (XMLItem item in rootItem.children)
            {
                 con = getAllOutgoingConnections(item.getName());
                 allCon = getAllConnections().Count;
                if (item._getProperty("ItemKind") != "DiagramConnector")
                {
                    str += "System coh  " + sum +"\n\n\n"+ item.getName() + "    " + Cohesion(item).ToString("F") +"    "+ Coupling(item).ToString("F") + "  "+CDep(item)+ "\n\n";
                }
            }
            return str;
        }

        public List<String> getAllComponents(String tabSpace = "")
        {
            List<String> comps = new List<string>();
            rootItem._getChildrenComponents(comps, tabSpace);
            return comps;
        }

        public List<String> getAllConnections()
        {
            List<String> cons = new List<string>();
            foreach(XMLItem item in rootItem.children)
            {
                if(item._getProperty("ItemKind") == "DiagramConnector")
                {
                    String desc;
                    List<String> begin = item._getProperty("BeginItem").Split(',').ToList();
                    List<String> end = item._getProperty("EndItem").Split(',').ToList();
                    String beginItem = rootItem._getChildNameByGlobalIndex(begin, 0);
                    String endItem = rootItem._getChildNameByGlobalIndex(end, 0);
                    desc = beginItem + "->" + endItem;
                    cons.Add(desc);
                }
            }
            return cons;
        }

        // Inner Connections
        public int getConnectionsInsideContainer(String containerName)
        {
            XMLItem container = getComponent(containerName);
            int sum = 0;
            foreach(XMLItem child in container.children)
            {
                foreach(XMLItem sender in child.inComings)
                {
                    if (sender.parent == container) sum++;
                }
            }
            return sum;
        }

        // All outgoing connections from a container
        public int getAllOutgoingConnections(String containerName)
        {
            XMLItem container = getComponent(containerName);
            int sum = 0;
            // first, the container connections..
            foreach(XMLItem receiver in container.outGoings)
            {
                if (receiver.parent != container && receiver != container) sum++;
            }

            // second, children connections
            foreach(XMLItem child in container.children)
            {
                foreach(XMLItem receiver in child.outGoings)
                {
                    if (receiver.parent != container && receiver != container) sum++;
                }
            }
            return sum;
        }

        // sub components
        public int getSubComponents(String containerName)
        {
            XMLItem container = getComponent(containerName);
            return container.children.Count;
        }

        public int getContainersConnectedTo(String containerName)
        {
            XMLItem container = getComponent(containerName);
            int sum = 0;
            foreach(XMLItem rec in container.outGoings)
            {
                if (rec.parent == rootItem) sum++;
            }
            return sum;
        }

        public int getConsTo(String containerName)
        {
            XMLItem container = getComponent(containerName);
            int sum = 0;
            foreach (XMLItem sen in container.inComings)
            {
                if (sen.parent == rootItem) sum++;
            }
            return sum;
        }

        public int getItemChildrenConnections(XMLItem it)
        {
            int sum = 0;
            foreach (XMLItem item in it.children)
            {
                sum += item.inComings.Count + item.outGoings.Count;
            }
            return sum;
        }

        public XMLItem getComponent(String componentName)
        {
            return rootItem._getComponentByName(componentName);
        }

        public void _setProps(XMLItem item, String props)
        {
            MatchCollection i = reProps.Matches(props);
            foreach (Match match in i)
            {
                GroupCollection groups = match.Groups;
                Property p = new Property
                {
                    name = groups["name"].Value,
                    value = groups["value"].Value
                };
                item.properties.Add(p);
            }
        }

        public void _setChildren(XMLItem root, String childrenText)
        {
            MatchCollection i = reItemsNChildren.Matches(childrenText);

            foreach (Match itemAndChild in i)
            {
                GroupCollection groups = itemAndChild.Groups;

                String props = groups["properties"].Value;
                XMLItem child = new XMLItem();
                _setProps(child, props);
                child.parent = root;
                root.children.Add(child);

                String childsChildren = groups["children"].Value;
                if (! String.IsNullOrEmpty(childsChildren))
                {
                    _setChildren(child, childsChildren);
                }
            }
        }

        public double Cohesion(XMLItem item)
        {
            double MC , COH;
           if (item._getProperty("ItemKind") == "DiagramShape") return 1;
           else
            {
                MC = getSubComponents(item.getName()) * (getSubComponents(item.getName()) - 1) * 0.5;
                COH = getConnectionsInsideContainer(item.getName()) / MC;
                if (COH == 0 || MC == 0) return 1;
                else return COH;
                
            }
        }

        public decimal Coupling(XMLItem item)
        {
            decimal COP;
            decimal x = (decimal)getAllConnections().Count;
            if(x == 0) { COP = 0; } else { COP = (decimal)getAllOutgoingConnections(item.getName()) / x; }
            return COP;
        }

        //component dependence
        public decimal CDep(XMLItem item)
        {
            decimal CDep;
            CDep = (decimal)getAllOutgoingConnections(item.getName()) / (decimal)(rootItem.children.Count - getAllConnections().Count);
            return CDep;
        }
    }
}
