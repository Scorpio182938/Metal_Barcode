using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metal_Barcode.DataAccess;
using Metal_Barcode.Models;

namespace Metal_Barcode.Service
{
    public class DeviceService
    {
        SqlServerAccess sqlServerAccess = new SqlServerAccess();
        public List<DeviceModel> GetDevices()
        {
            List<DeviceModel> deviceModels = new List<DeviceModel>();
            // 获取点位信息
            // 获取设备信息
            var d_info = sqlServerAccess.GetDevices();
            foreach (var item in d_info.AsEnumerable())
            {
                DeviceModel deviceModel = new DeviceModel();
                deviceModel.Name = item.Field<string>("d_name");
                deviceModel.SN = item.Field<string>("d_sn");
                deviceModel.CommType = (int)item.Field<Int32>("comm_type");

                // 获取协议信息
                var p_info = sqlServerAccess.GetProtocolSettings(item.Field<string>("d_id"), deviceModel.CommType);
                if (p_info != null&& p_info.AsEnumerable().Count()>0)
                {
                    var p_row = p_info.AsEnumerable().First();
                    if (deviceModel.CommType == 1)
                    {
                        deviceModel.Modbus = new ProtocolModbus()
                        {
                            IP = p_row.Field<string>("d_ip"),
                            Port = (int)p_row.Field<Int32>("d_port"),
                            // 其他属性
                            BaudRate = (int)p_row.Field<Int32>("baudrate")

                        };
                    }
                    else if (deviceModel.CommType == 2)
                    {
                        deviceModel.S7 = new ProtocolS7Model()
                        {
                            IP = p_row.Field<string>("d_ip"),
                            Port = (int)p_row.Field<Int32>("d_port"),
                            Rock = (int)p_row.Field<Int32>("d_rock"),
                            Slot = (int)p_row.Field<Int32>("d_slot")
                        };
                    }
                }

                // 获取点位信息
                var v_info = sqlServerAccess.GetMonitorValues(item.Field<string>("d_id"));
                if (v_info != null && v_info.AsEnumerable().Count() > 0)
                {
                    List<MonitorValueModel> vList = (from q in v_info.AsEnumerable()
                                                     select new MonitorValueModel
                                                     {
                                                         ValueName = q.Field<string>("tag_name"),
                                                         Address = q.Field<string>("address"),
                                                         DataType = q.Field<string>("data_type"),
                                                         Unit = q.Field<string>("unit")
                                                     }).ToList();
                    deviceModel.MonitorValueList = new System.Collections.ObjectModel.ObservableCollection<MonitorValueModel>(vList);
                }

                deviceModels.Add(deviceModel);
            }

            return deviceModels;
        }
    }
}
