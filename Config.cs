using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Life_at_hand
{
    public class Config
    {
        //public class Province
        //{
        //    public string id;
        //    public string name;
        //    public class City
        //    {
        //        public string id;
        //        public string name;
        //        public class Area
        //        {
        //            public string id;
        //            public string name;
        //        };
        //        public Area[] areas = { };
        //    };
        //    public City[] cities = { };
        //};

        //public Province[] Provinces = { };


        public struct pca
        {
            public string pid;//省
            public string cid;//市
            public string aid;//区
            public string combine()
            {
                return (pid + cid + aid);
            }
        };



        public pca m_pca;

        public string lastCode;

        public string pName;
        public string cName;
        public string aName;

        public double opacity;
    }
}
