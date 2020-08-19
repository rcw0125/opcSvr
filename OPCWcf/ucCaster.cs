using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using opcBase;

namespace OPCWcf
{
    public partial class ucCaster : UserControl
    {
   
        public ucCaster()
        {
            InitializeComponent();
        }
        public cutStrandInfo strand1 = new cutStrandInfo();
        public cutStrandInfo strand2 = new cutStrandInfo();
        public cutStrandInfo strand3 = new cutStrandInfo();
        public cutStrandInfo strand4 = new cutStrandInfo();

        public void setupConfig()
        {
            strand1.circlePointX = 450;
            strand1.circlePointY = 50;
            strand1.radius = 200;
            strand1.gunaochangdu = 450;
            strand1.shijigunaochangdu = 30;
            strand1.huoqiejiweizhi = 24.8;
            strand1.shanxingweizhi = 14.13;
            strand1.zhupiStart = 30;
            strand1.zhupiEnd = 5;
            int pointCha = 70;
            strand2.circlePointX = 450;
            strand2.circlePointY = 50+pointCha;
            strand2.radius = 200;
            strand2.gunaochangdu = 450;
            strand2.shijigunaochangdu = 30;
            strand2.huoqiejiweizhi = 24.8;
            strand2.shanxingweizhi = 14.13;
            strand2.zhupiStart = 30;
            strand2.zhupiEnd = 5;

            strand3.circlePointX = 450;
            strand3.circlePointY = 50 +2* pointCha;
            strand3.radius = 200;
            strand3.gunaochangdu = 450;
            strand3.shijigunaochangdu = 30;
            strand3.huoqiejiweizhi = 24.8;
            strand3.shanxingweizhi = 14.13;
            strand3.zhupiStart = 30;
            strand3.zhupiEnd = 5;

            strand4.circlePointX = 450;
            strand4.circlePointY = 50 + 3 * pointCha;
            strand4.radius = 200;
            strand4.gunaochangdu = 450;
            strand4.shijigunaochangdu = 30;
            strand4.huoqiejiweizhi = 24.8;
            strand4.shanxingweizhi = 14.13;
            strand4.zhupiStart = 30;
            strand4.zhupiEnd = 5;     
        }

        

        public void Redraw()
        {

            drawCircle(strand1);
            drawCircle(strand2);
            drawCircle(strand3);
            drawCircle(strand4);
        }

        public void drawCircle(cutStrandInfo strand)
        {
            Graphics graphics = this.CreateGraphics();
            Pen pen = new Pen(Color.Gray, 10);
            Pen redpen = new Pen(Color.Red, 4);
            Pen cutpen = new Pen(Color.Red, 20);
            Pen fengpen = new Pen(Color.Green, 10);
            int gundaofeng = 3;

            //弧形+辊道
            graphics.DrawArc(pen, strand.circlePointX - strand.radius, strand.circlePointY - strand.radius, 2 * strand.radius, 2 * strand.radius, 90, 90);
            graphics.DrawLine(pen, strand.circlePointX + gundaofeng, strand.circlePointY + strand.radius, strand.circlePointX + strand.gunaochangdu + gundaofeng, strand.circlePointY + strand.radius);

            //火切机位置
            int hqj = Convert.ToInt32((strand.huoqiejiweizhi - strand.shanxingweizhi) * strand.gunaochangdu / strand.shijigunaochangdu);
            graphics.DrawLine(cutpen, strand.circlePointX + hqj + gundaofeng, strand.circlePointY + strand.radius, strand.circlePointX + hqj + 3 + gundaofeng, strand.circlePointY + strand.radius);


            if (strand.zhupiStart > strand.shanxingweizhi)
            {
                int hudu = Convert.ToInt32((strand.shanxingweizhi - strand.zhupiEnd) * 90.0 / strand.shanxingweizhi);
                graphics.DrawArc(redpen, strand.circlePointX - strand.radius, strand.circlePointY - strand.radius, 2 * strand.radius, 2 * strand.radius, 90, hudu);
                int changdu = Convert.ToInt32((strand.zhupiStart - strand.shanxingweizhi) * strand.gunaochangdu / strand.shijigunaochangdu);
                graphics.DrawLine(redpen, strand.circlePointX + gundaofeng, strand.circlePointY + strand.radius, strand.circlePointX + changdu + gundaofeng, strand.circlePointY + strand.radius);
            }
           
            foreach (var item in strand.listfeng)
            {
                //end点的跟踪值-缝跟踪值-  +end点的位置
                double fengweizhi = strand.endtrack - item.startTrack + strand.zhupiEnd;
                if (fengweizhi < strand.shanxingweizhi)
                {
                    int starthudu = Convert.ToInt32(180 - fengweizhi * 90.0 / strand.shanxingweizhi);
                    graphics.DrawArc(fengpen, strand.circlePointX - strand.radius, strand.circlePointY - strand.radius, 2 * strand.radius, 2 * strand.radius, starthudu, 3);
                }
                else
                {
                    int changdu = Convert.ToInt32((fengweizhi - strand.shanxingweizhi) * strand.gunaochangdu / strand.shijigunaochangdu);
                    graphics.DrawLine(fengpen, strand.circlePointX + changdu + gundaofeng, strand.circlePointY + strand.radius, strand.circlePointX + changdu + 3 + gundaofeng, strand.circlePointY + strand.radius);
                }
            }

        }

    }
    public class cutStrandInfo
    { 
       public int circlePointX { get; set; }
       public int circlePointY { get; set; }
       public int radius { get; set; }

        public double endtrack { get; set; }
        public double starttrack { get; set; }
        public double zhupiStart { get; set; }
       public double zhupiEnd { get; set; }
       public  int gunaochangdu { get; set; }
        public double huoqiejiweizhi { get; set; }

        public double shanxingweizhi { get; set; }

        public double shijigunaochangdu { get; set; }

        public List<ladleFeng> listfeng = new List<ladleFeng>();


    }

  
}
