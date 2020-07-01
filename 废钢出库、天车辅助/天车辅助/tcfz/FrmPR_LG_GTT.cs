
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
//using BLL;
using DevExpress.XtraScheduler;
using Rcw.Method;

namespace tcfz
{
    public partial class FrmPR_LG_GTT : Form
    {
        public FrmPR_LG_GTT()
        {
            InitializeComponent();
           
        }
        public string apiurl = "http://192.168.48.234/api/tc/";

        public void checkIp()
        {
            string url = apiurl + "getIpList";
            List<funcinfo> listIp = WebOper.HttpGet(url, typeof(List<funcinfo>)) as List<funcinfo>;
            var curip = listIp.Where(o => o.funcName == GetLocalIP()).FirstOrDefault();
            if (curip == null)
            {
                MessageBox.Show("本机未授权");
                this.Close();
            }
        }

        public List<scjz> getScjz()
        {
            string url = apiurl + "getScjz";
            List<scjz> listscjz = WebOper.HttpGet(url, typeof(List<scjz>)) as List<scjz>;
            return listscjz;        
        }

        public string GetLocalIP()
        {
            try
            {
                string ip = "";
                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (IpEntry.AddressList[i].ToString().StartsWith("192.168.36") || IpEntry.AddressList[i].ToString().StartsWith("192.168.48"))
                        {
                            return IpEntry.AddressList[i].ToString();
                        }
                        ip = IpEntry.AddressList[i].ToString();


                    }
                }
                return ip;
            }
            catch
            {
                return "";
            }
        }
        //private Bll_TPC_PLAN_SMS bll_tpc_plan_sms = new Bll_TPC_PLAN_SMS();//炼钢排产计划

        private void FrmPR_LG_GTT_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 3;

            //初始化甘特图
            InitGrantView(this.schedulerControl1);
            List<Task> tasks = new List<Task>();
            List<TaskDetails> taskDetails = new List<TaskDetails>();
       
            //任务属性映射
            var sourceMap = this.schedulerStorage1.Resources.Mappings;
            sourceMap.Caption = "Name";
            sourceMap.Id = "ID";
            //数据绑定
            this.schedulerStorage1.Resources.DataSource = tasks;
            /*日程属性映射*/
            var appointMentMap = this.schedulerStorage1.Appointments.Mappings;
            appointMentMap.ResourceId = "ParentID";/*资源ID*/
            appointMentMap.AppointmentId = "ID";/*任务ID*/
            appointMentMap.Start = "Start";
            appointMentMap.End = "End";           
            //                            Description
            appointMentMap.Description = "Description";
            appointMentMap.AllDay = "AllDay";
            appointMentMap.Label = "Label";
            appointMentMap.Subject = "Subject";
            appointMentMap.PercentComplete = "PercentComplete";
            appointMentMap.Location = "Center";           
            /*数据绑定*/
            this.schedulerStorage1.Appointments.DataSource = taskDetails;
            checkIp();
        }

        public void loaddata()
        {
            #region 重组连铸计划
            List<Task> tasks = new List<Task>();
            List<TaskDetails> taskDetails = new List<TaskDetails>();
            var listscjz = getScjz();

            if (comboBox1.Text == "1#炉"|| comboBox1.Text == "2#炉"|| comboBox1.Text == "3#炉"|| comboBox1.Text == "1#LF" || comboBox1.Text == "2#LF" || comboBox1.Text == "3#机" || comboBox1.Text == "4#机")
            {
                AddPlan("S21", "1#炉", tasks, taskDetails, listscjz);
                AddPlan("S22", "2#炉", tasks, taskDetails, listscjz);
                AddPlan("S23", "3#炉", tasks, taskDetails, listscjz);
                AddPlan("S41", "1#LF", tasks, taskDetails, listscjz);
                AddPlan("S42", "2#LF", tasks, taskDetails, listscjz);
                AddPlan("S63", "3#机", tasks, taskDetails, listscjz);
                AddPlan("S64", "4#机", tasks, taskDetails, listscjz);
            }
            else
            {
                AddPlan("S68", "", tasks, taskDetails, listscjz);
                AddPlan("S24", "4#炉", tasks, taskDetails, listscjz);
                AddPlan("S43", "3#LF", tasks, taskDetails, listscjz);
                AddPlan("S51", "RH", tasks, taskDetails, listscjz);
                AddPlan("S65", "5#机", tasks, taskDetails, listscjz);
               
                AddPlan("S69", "", tasks, taskDetails, listscjz);

            }

           
           
            //AddPlan("S63", "3#机", tasks, taskDetails, listscjz);
            //AddPlan("S64", "4#机", tasks, taskDetails, listscjz);
            //AddPlan("S65", "5#机", tasks, taskDetails, listscjz);


            #endregion
            //任务属性映射
            var sourceMap = this.schedulerStorage1.Resources.Mappings;
            sourceMap.Caption = "Name";
            sourceMap.Id = "ID";
            //数据绑定
            this.schedulerStorage1.Resources.DataSource = tasks;
            /*日程属性映射*/
            var appointMentMap = this.schedulerStorage1.Appointments.Mappings;
            appointMentMap.ResourceId = "ParentID";/*资源ID*/
            appointMentMap.AppointmentId = "ID";/*任务ID*/
            appointMentMap.Start = "Start";
            appointMentMap.End = "End";
            //                            Description
            appointMentMap.Description = "Description";
            appointMentMap.AllDay = "AllDay";
            appointMentMap.Label = "Label";
            appointMentMap.Subject = "Subject";
            appointMentMap.PercentComplete = "PercentComplete";
            appointMentMap.Location = "Center";
            /*数据绑定*/
            this.schedulerStorage1.Appointments.DataSource = taskDetails;
            

        }

        public void AddPlan(string code, string des, List<Task> taskList, List<TaskDetails> detailsList,List<scjz> scjzList)
        {
            List<scjz> unitScjz = null;
            if (code.StartsWith("S2"))
            {
                unitScjz = scjzList.Where(o => o.bofid == code).OrderBy(o => o.heatid).ToList();
            }
            else if(code.StartsWith("S4"))
            {
                unitScjz = scjzList.Where(o => o.lfid == code).OrderBy(o => o.heatid).ToList();
            }
            else if (code.StartsWith("S5"))
            {
                unitScjz = scjzList.Where(o => o.rhid == code).OrderBy(o => o.heatid).ToList();
            }
            else if (code.StartsWith("S6"))
            {
                unitScjz = scjzList.Where(o => o.casterid == code).OrderBy(o => o.heatid).ToList();
            }

            Task task = new Task();
            task.ID = code;
            if (des == "")
            {
                task.Name = "";
            }
            else
            {
                task.Name = des + "计划";
            }
            
            taskList.Add(task);
            DateTime lastjhjs=DateTime.Now.AddHours(-3) ;
            int i = 0;
            Task task1 = new Task();
            task1.ID = code + "_Fact";
            if (des == "")
            {
                task1.Name = "";
            }
            else
            {
                task1.Name = des + "实绩";
            }
            
            taskList.Add(task1);
            if (unitScjz == null)
            {
                return;
            }
            foreach (var item in unitScjz)
            {
                i++;
                TaskDetails taskDetail = new TaskDetails();
                taskDetail.ID = item.heatid;
                if (code.StartsWith("S2"))
                {
                    taskDetail.Start = item.aim_time_bofstart;
                    taskDetail.End = item.aim_time_boftapped;
                }
                else if (code.StartsWith("S4"))
                {
                    taskDetail.Start = item.aim_time_lfstart;
                    taskDetail.End = item.aim_time_lfend;
                }
                else if (code.StartsWith("S5"))
                {
                    taskDetail.Start = item.aim_time_rhstart;
                    taskDetail.End = item.aim_time_rhend;
                }
                else if (code.StartsWith("S6"))
                {
                    taskDetail.Start = item.aim_time_casterarrival;
                    taskDetail.End = item.aim_time_castingend;
                }
                if (i > 1)
                {
                    if (taskDetail.Start < lastjhjs)
                    {
                        taskDetail.Start = lastjhjs;
                    }
                }
                lastjhjs = taskDetail.End;


                taskDetail.Subject = item.heatid + Environment.NewLine + item.steelgrade;
                taskDetail.Description = "1";
                if (des == comboBox1.Text)
                {
                    if (taskDetail.End > DateTime.Now)
                    {
                        taskDetail.Label = "2";
                    }
                    else
                    {
                        taskDetail.Label = "4";
                    }
                   
                }
                else
                {
                    if (taskDetail.End < DateTime.Now)
                    {
                        taskDetail.Label = "4";
                    }
                    else if (taskDetail.End>DateTime.Now && taskDetail.Start<DateTime.Now)
                    {
                        taskDetail.Label = "2";
                    }
                    else
                    {
                        taskDetail.Label = "3";
                    }
                  
                }
                
                taskDetail.ParentID = task.ID;
                detailsList.Add(taskDetail);

                TaskDetails taskDetail1 = new TaskDetails();
                taskDetail1.ID = item.heatid;
                if (code.StartsWith("S2"))
                {
                    taskDetail1.Start = item.act_time_bofstart;
                    taskDetail1.End = item.act_time_boftapped;
                }
                else if (code.StartsWith("S4"))
                {
                    taskDetail1.Start = item.act_time_lfstart;
                    taskDetail1.End = item.act_time_lfend;
                }
                else if (code.StartsWith("S5"))
                {
                    taskDetail1.Start = item.act_time_rhstart;
                    taskDetail1.End = item.act_time_rhend;
                }
                else if (code.StartsWith("S6"))
                {
                    taskDetail1.Start = item.act_time_castingstart;
                    taskDetail1.End = item.act_time_castingend;
                }
                
                taskDetail1.Subject = item.heatid;
                taskDetail1.Description = "2";
                taskDetail1.Label = "1";
                taskDetail1.ParentID = task1.ID;
                detailsList.Add(taskDetail1);
            }
        }


        private void InitGrantView(DevExpress.XtraScheduler.SchedulerControl sc)
        {

            //设置资源
            sc.GroupType = SchedulerGroupType.Resource;

            //设置甘特图
            sc.ActiveViewType = SchedulerViewType.Gantt;

            //设置Resource 字体不旋转
            sc.OptionsView.ResourceHeaders.RotateCaption = false;
            sc.OptionsView.ResourceHeaders.Height = 60;
            sc.OptionsView.ShowOnlyResourceAppointments = true;

            
            sc.Start = DateTime.Now;

            sc.DataStorage.Appointments.Labels.Clear();
            var label1 = sc.DataStorage.Appointments.Labels.CreateNewLabel("1", "Label");
            label1.SetColor(Color.LightBlue);
            sc.DataStorage.Appointments.Labels.Add(label1);
            
            var label2 = sc.DataStorage.Appointments.Labels.CreateNewLabel("2", "Label");
            label2.SetColor(Color.Red);
            sc.DataStorage.Appointments.Labels.Add(label2);

            var label3 = sc.DataStorage.Appointments.Labels.CreateNewLabel("3", "Label");
            label3.SetColor(Color.Gold);
            sc.DataStorage.Appointments.Labels.Add(label3);

            var label4 = sc.DataStorage.Appointments.Labels.CreateNewLabel("4", "Label");
            label4.SetColor(Color.Green);
            sc.DataStorage.Appointments.Labels.Add(label4);

            //设置资源+-按钮不可见
            sc.ResourceNavigator.Visibility = ResourceNavigatorVisibility.Never;

            //禁止块冲突
            sc.OptionsCustomization.AllowAppointmentConflicts = AppointmentConflictsMode.Allowed;
            //不允许复制
            sc.OptionsCustomization.AllowAppointmentCopy = UsedAppointmentType.None;
            //不允许创建
            sc.OptionsCustomization.AllowAppointmentCreate = UsedAppointmentType.None;
            //不允许删除
            sc.OptionsCustomization.AllowAppointmentDelete = UsedAppointmentType.None;
            //不允许拖到
            sc.OptionsCustomization.AllowAppointmentDrag = UsedAppointmentType.None;
            //不允许不同资源间创建
            sc.OptionsCustomization.AllowAppointmentDragBetweenResources = UsedAppointmentType.None;
            //允许编辑
            //sc.OptionsCustomization.AllowAppointmentEdit = UsedAppointmentType.All;
            //不允许多个选择
            sc.OptionsCustomization.AllowAppointmentMultiSelect = false;
            //不允许改变大小
            sc.OptionsCustomization.AllowAppointmentResize = UsedAppointmentType.None;
            //不允许依赖窗体弹出
            sc.OptionsCustomization.AllowDisplayAppointmentDependencyForm = AllowDisplayAppointmentDependencyForm.Never;
            //允许Appointment窗体弹出
            sc.OptionsCustomization.AllowDisplayAppointmentForm = AllowDisplayAppointmentForm.Never;
            //不允许编辑文字
            sc.OptionsCustomization.AllowInplaceEditor = UsedAppointmentType.None;

            //GrantView
            sc.Views.GanttView.CellsAutoHeightOptions.Enabled = false;//设置单元格不自适应高度 

            sc.Views.GanttView.CellsAutoHeightOptions.MinHeight = 40;//设置最小高度

           
            //高度
            sc.Views.GanttView.AppointmentDisplayOptions.AppointmentHeight = 40;
            //设置Appointment 的高度不自适应
            sc.Views.GanttView.AppointmentDisplayOptions.AppointmentAutoHeight = false;
           // sc.Views.GanttView.AppointmentDisplayOptions.AppointmentAutoHeight = true;
            sc.Views.GanttView.AppointmentDisplayOptions.AppointmentHeight = 40;
            sc.Views.GanttView.AppointmentDisplayOptions.AppointmentInterspacing = 1;
            sc.Views.GanttView.AppointmentDisplayOptions.ContinueArrowDisplayType = AppointmentContinueArrowDisplayType.Never;
            //不显示开始时间
            sc.Views.GanttView.AppointmentDisplayOptions.StartTimeVisibility = AppointmentTimeVisibility.Never;
            //不显示结束时间
            sc.Views.GanttView.AppointmentDisplayOptions.EndTimeVisibility = AppointmentTimeVisibility.Never;
            //不显示进度条
            sc.Views.GanttView.AppointmentDisplayOptions.PercentCompleteDisplayType = PercentCompleteDisplayType.None;
            //不显示恢复
            sc.Views.GanttView.AppointmentDisplayOptions.ShowRecurrence = false;
            //不显示提醒
            sc.Views.GanttView.AppointmentDisplayOptions.ShowReminder = false;
            sc.Views.GanttView.AppointmentDisplayOptions.SnapToCellsMode = AppointmentSnapToCellsMode.Never;
            //不显示状态
            sc.Views.GanttView.AppointmentDisplayOptions.StatusDisplayType = AppointmentStatusDisplayType.Never;
            //显示样式
            sc.Views.GanttView.AppointmentDisplayOptions.TimeDisplayType = AppointmentTimeDisplayType.Text;
            //sc.Views.GanttView.AppointmentDisplayOptions.



        }

        private void schedulerControl1_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            e.Menu.Items.Clear();
        }

        private void btn_Query_Click(object sender, EventArgs e)
        {         
            loaddata();
        }
    }



    public class Task
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class TaskDetails
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string Label { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }



}
