using System;
using System.Collections.Generic;
using System.Text;
using opcBase;

namespace OPCWcf
{
    public class casterWeight
    {
        public string code { get; set; }
        public string lastweight { get; set; }
        public double curweight { get; set; }
        public string L1weight { get; set; }
        public int xiachuanFlag { get; set; }
        public int L1ValId { get; set; }
        public string ts { get; set; }
        public void downLoadWeight()
        {
            if (lastweight == curweight.ToString())
            {
                if (xiachuanFlag == 1)
                {
                    return;
                }
                else
                {
                    if (PlcSvr.GetInstance().setVal(L1ValId, curweight) == true)
                    {
                        xiachuanFlag = 1;
                    }
                }
            }
            else
            {
                xiachuanFlag = 0;
                lastweight = curweight.ToString();
                if (PlcSvr.GetInstance().setVal(L1ValId, curweight) == true)
                {
                    xiachuanFlag = 1;
                }
            
            }
        }
    }
}
