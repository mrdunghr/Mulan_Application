using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mulan_Auto
{
    internal class Key
    {
        private string codeKey;
        private float price;
        private DateTime startDate;
        private DateTime endDate;
        private string computerName;
        private string ipAddress;
        private string macAddress;

        public string CodeKey
        {
            get { return codeKey; }
            set { codeKey = value; }
        }

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }

        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public string MACAddress
        {
            get { return macAddress; }
            set { macAddress = value; }
        }

    }
}
