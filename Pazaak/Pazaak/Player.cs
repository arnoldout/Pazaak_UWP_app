using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pazaak
{
    class Player
    {
        private Card[] crdPool;
        private Card[] hand;
        private Card[] onBrd;
        private String name;
        private int trnCnt;
        private int rndsWn;
        private int gmsWn;
        private int currScr;
        private Boolean stndng;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int RndsWn
        {
            get
            {
                return rndsWn;
            }

            set
            {
                rndsWn = value;
            }
        }

        public int GmsWn
        {
            get
            {
                return gmsWn;
            }

            set
            {
                gmsWn = value;
            }
        }

        public int CurrScr
        {
            get
            {
                return currScr;
            }

            set
            {
                currScr = value;
            }
        }

        public bool Stndng
        {
            get
            {
                return stndng;
            }

            set
            {
                stndng = value;
            }
        }

        internal Card[] OnBrd
        {
            get
            {
                return onBrd;
            }

            set
            {
                onBrd = value;
            }
        }

        public int TrnCnt
        {
            get
            {
                return trnCnt;
            }

            set
            {
                trnCnt = value;
            }
        }

        public Player(Card[] crdPool, String name) {
            this.crdPool = crdPool;
            this.hand = genHnd(crdPool);
            this.onBrd = new Card[9];
            this.rndsWn = 0;
            this.gmsWn = 0;
            this.trnCnt = 0;
            this.name = name;
            this.currScr = 0;   
        }
        public Card[] genHnd(Card [] crdPool)
        {
            Card[] hand = new Card[4];
            for(int genLoop = 0; genLoop<4; genLoop++)
            {
                Random r = new Random();
                int card = r.Next(0, crdPool.Length);
                hand[genLoop] = crdPool[card]; 
            }
            return hand;
        }
        public void addCrd(Card c)
        {
            onBrd[this.trnCnt] = c;

        }
    }
}

