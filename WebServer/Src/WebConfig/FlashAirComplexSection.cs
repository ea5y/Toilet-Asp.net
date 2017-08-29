using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebServer
{
    public class FlashAirComplexSection : ConfigurationSection
    {
        [ConfigurationProperty("child", IsDefaultCollection = false)]
        public ChildSection Child
        {
            get { return (ChildSection)base["child"]; }
            set { base["child"] = value; }
        }

        [ConfigurationProperty("children", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ChildSection))]
        public Children Children
        {
            get { return (Children)base["children"]; }
            set { base["children"] = value; }
        }
    }

    public class Children : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new ChildSection();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ChildSection)element).Index;
        }

        public ChildSection this[int i]
        {
            get { return (ChildSection)base.BaseGet(i); }
        }

        public ChildSection this[string key]
        {
            get { return (ChildSection)base.BaseGet(key); }
        }
    }

    public class ChildSection : ConfigurationElement
    {
        [ConfigurationProperty("index", IsRequired = true, IsKey = true)]
        public string Index
        {
            get { return (string)base["index"]; }
            set { base["index"] = value; }
        }

        [ConfigurationProperty("unitId", DefaultValue = "0", IsKey = true)]
        public string UnitId
        {
            get { return (string)base["unitId"]; }
            set { base["unitId"] = value; }
        }

        [ConfigurationProperty("modelName", DefaultValue = "DIO-16/32A-U", IsKey = true)]
        public string ModelName
        {
            get { return (string)base["modelName"]; }
            set { base["modelName"] = value; }
        }

        [ConfigurationProperty("floorName", IsRequired = true, IsKey = true)]
        public string FloorName
        {
            get { return (string)base["floorName"]; }
            set { base["floorName"] = value; }
        }

        [ConfigurationProperty("roomCount", IsRequired = true, IsKey = true)]
        public string RoomCount
        {
            get { return (string)base["roomCount"]; }
            set { base["roomCount"] = value; }
        }

        [ConfigurationProperty("flashAirUrl", IsRequired = true, IsKey = true)]
        public string FlashAirUrl
        {
            get { return (string)base["flashAirUrl"]; }
            set { base["flashAirUrl"] = value; }
        }
    }
}