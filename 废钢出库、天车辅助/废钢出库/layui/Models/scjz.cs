using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace layui.Models
{
    public class scjz
    {
//        select a.heatid, a.casterid, a.bofid, a.lfid, a.rhid, a.AIM_TIME_IRONPREPARED, a.AIM_TIME_BOFSTART, a.AIM_TIME_BOFTAPPED, a.AIM_TIME_TAPPEDSIDEEND,
//a.AIM_TIME_LFARRIVAL, a.AIM_TIME_LFSTART, a.AIM_TIME_LFEND, a.AIM_TIME_LFLEAVE, a.AIM_TIME_RHARRIVAL,
//a.AIM_TIME_RHSTART, a.AIM_TIME_RHEND, a.AIM_TIME_RHLEAVE, a.AIM_TIME_CASTERARRIVAL, a.AIM_TIME_CASTINGSTART, a.AIM_TIME_CASTINGEND ,
//b.ACT_TIME_IRONPREPARED, b.ACT_TIME_BOFSTART, b.ACT_TIME_BOFTAPPED,
//b.ACT_TIME_TAPPEDSIDEEND, b.ACT_TIME_LFARRIVAL, b.ACT_TIME_LFSTART, b.ACT_TIME_LFEND, b.ACT_TIME_LFLEAVE,
//b.ACT_TIME_RHARRIVAL, b.ACT_TIME_RHSTART, b.ACT_TIME_RHEND, b.ACT_TIME_RHLEAVE,
//b.ACT_TIME_CASTERARRIVAL, b.ACT_TIME_CASTINGSTART, b.ACT_TIME_CASTINGEND
        public string heatid { get; set; }
        public string steelgrade { get; set; }
        public string casterid { get; set; }
        public string bofid { get; set; }

        public string lfid { get; set; }

        public string rhid { get; set; }
        public DateTime aim_time_ironprepared { get; set; }
        public DateTime aim_time_bofstart { get; set; }
        public DateTime aim_time_boftapped { get; set; }
        public DateTime aim_time_tappedsideend { get; set; }
        public DateTime aim_time_lfarrival { get; set; }
        public DateTime aim_time_lfstart { get; set; }
        public DateTime aim_time_lfend { get; set; }
        public DateTime aim_time_lfleave { get; set; }
        public DateTime aim_time_rharrival { get; set; }
        public DateTime aim_time_rhstart { get; set; }
        public DateTime aim_time_rhend { get; set; }
        public DateTime aim_time_rhleave { get; set; }  
        public DateTime aim_time_casterarrival { get; set; }
        public DateTime aim_time_castingstart { get; set; }
        public DateTime aim_time_castingend { get; set; }

        public DateTime act_time_ironprepared { get; set; }
        public DateTime act_time_bofstart { get; set; }
        public DateTime act_time_boftapped { get; set; }
        public DateTime act_time_tappedsideend { get; set; }
        public DateTime act_time_lfarrival { get; set; }
        public DateTime act_time_lfstart { get; set; }
        public DateTime act_time_lfend { get; set; }
        public DateTime act_time_lfleave { get; set; }
        public DateTime act_time_rharrival { get; set; }
        public DateTime act_time_rhstart { get; set; }
        public DateTime act_time_rhend { get; set; }
        public DateTime act_time_rhleave { get; set; }
        public DateTime act_time_casterarrival { get; set; }
        public DateTime act_time_castingstart { get; set; }
        public DateTime act_time_castingend { get; set; }

    }
}