﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    class Card
    {
        private Int16 val;
        private String imgSrc;

        public string ImgSrc
        {
            get
            {
                return imgSrc;
            }

            set
            {
                imgSrc = value;
            }
        }

        public short Val
        {
            get
            {
                return val;
            }

            set
            {
                val = value;
            }
        }

        public Card(Int16 val)
        {
            this.val = val;
        }
        public Card()
        {
            Random rnd = new Random();
            Int16 value = (Int16)rnd.Next(1, 11);
            this.val = value;
        }
    }
}
