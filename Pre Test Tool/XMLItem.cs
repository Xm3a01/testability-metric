using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pre_Test_Tool
{
    class XMLItem
    {
        public List<Property> properties;
        public List<XMLItem> children;
        public List<XMLItem> inComings;
        public List<XMLItem> outGoings;
        public XMLItem parent;

        public XMLItem()
        {
            properties = new List<Property>();
            children = new List<XMLItem>();
            inComings = new List<XMLItem>();
            outGoings = new List<XMLItem>();
        }

        public String _getProperty(String prop)
        {
            for (int i = 0; i < properties.Count; i++)
            {
                if (prop == properties[i].name) return properties[i].value;
            }
            return "Empty!";
        }

        public String getName()
        {
            if (_getProperty("ItemKind") == "DiagramContainer") return _getProperty("Header");
            else if (_getProperty("ItemKind") == "DiagramShape") return _getProperty("Content");
            else if (_getProperty("ItemKind") == "DiagramRoot") return "Root";
            else return "No_Name!";
        }


        // NO USE
        public String _getChildNameByGlobalIndex(List<String> ls, int ind = 0)
        {
            if (ind == ls.Count - 1)
            {
                return this.children[int.Parse(ls[ind])].getName();
            }
            else
            {
                return this.children[int.Parse(ls[ind])]._getChildNameByGlobalIndex(ls, ind + 1);
            }
        }

        // NO CLEAR USE
        public String _getAllProps()
        {
            String props = "";
            for (int i = 0; i < this.properties.Count; i++)
            {
                props += properties[i].name + ":\"";
                props += properties[i].value + "\"  ";
            }
            return props;
        }

        // WRAPPED to LIB
        public List<String> _getChildrenComponents(List<String> c, String tab)
        {
            foreach (XMLItem child in children)
            {
                if (child.getName() != "No_Name!")
                {
                    c.Add(tab + child.getName());
                    child._getChildrenComponents(c, tab + tab);
                }
            }
            return c;
        }

        public XMLItem _getComponentByName(String cName)
        {
            foreach (XMLItem child in children)
            {
                if (child.getName() == cName)
                {
                    return child;
                }
                else
                {
                    XMLItem it = child._getComponentByName(cName);
                    if (it._getProperty("ItemKind") != "None")
                    {
                        return it;  // Hehe 
                    }
                }
            }
            XMLItem none = new XMLItem();
            Property p = new Property() { name = "ItemKind", value = "None" };
            none.properties.Add(p);
            return none;
        }
    }
}
