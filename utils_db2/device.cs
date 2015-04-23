using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils_db2
{
    public class Device
    {
        public int device_id = 0;
        public string name = "undefined";
        public Device(int i, string n)
        {
            device_id = i;
            name = n;
        }
        public override string ToString()
        {
            return name;
        }
    }
    public class Devices
    {
        /// <summary>
        /// link to Device->device_id
        /// </summary>
        public int device_id = 0;
        /// <summary>
        /// link to utilities devices_id
        /// </summary>
        public int devices_id = 0;
        public Devices(int device_device_id, int util_devices_id)
        {
            device_id= device_device_id;
            devices_id = util_devices_id;
        }
    }
}
