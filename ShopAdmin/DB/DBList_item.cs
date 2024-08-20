using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ShopAdmin.DB
{
    public class DBList_item
    {
        public uint ID { get; set; }
        public string DBPath { get; set; }
        public string DBName { get; set; }
        public string DBDate { get; set; }
        public string DBLastChange { get; set; }
        public string DBSize { get; set; }
        public Button Open { get; set; }


    }
}
