using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Pazaak
{
    public class Player
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
        private Boolean gotDk;
        private Boolean isTrn;
        private Boolean isBust;
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
        internal Card[] Hand
        {
            get
            {
                return hand;
            }

            set
            {
                hand = value;
            }
        }
        public bool GotDk
        {
            get
            {
                return gotDk;
            }

            set
            {
                gotDk = value;
            }
        }
        public bool IsTrn
        {
            get
            {
                return isTrn;
            }

            set
            {
                isTrn = value;
            }
        }
        public bool IsBust
        {
            get
            {
                return isBust;
            }

            set
            {
                isBust = value;
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
            this.gotDk = false;
            this.isTrn = true;
            this.isBust = false;
        }
        public void rndWn(Player p, InGame ig)
        {
            //wassup wit dat?
            if(this.rndsWn<2)
            {
                this.rndsWn++;
                ig.status = "You Lost";
            }
            else
            {
                ig.status = "New Game? ";
                this.gmsWn++;
                HighScore h = new HighScore(this.gmsWn, p.gmsWn);
 //               h.readScores();
   //             h.writeScores();
                hardReset();
                p.hardReset();
                ig.resetScrCircles();
            }
        }
        public Card[] genHnd(Card [] crdPool)
        {
            Card[] pool = crdPool;
            Card[] hand = new Card[4];
            for (int genLoop = 0; genLoop<4; genLoop++)
            {
                int card = App.randomizer.Next(0, pool.Length);
                Card c = pool[card];
                hand[genLoop] = c;
                pool = pool.Except(new Card[] {c }).ToArray();
            }
            return hand;
        }
        public void addCrd(Card c)
        {
            onBrd[this.trnCnt] = c;    
        }
        public void reset()
        {
            this.onBrd = new Card[9];
            this.trnCnt = 0;
            this.currScr = 0;
            this.gotDk = false;
            this.isTrn = true;
            this.isBust = false;
            this.stndng = false;
            
        }
        public void hardReset()
        {
            reset();
            this.rndsWn = 0;
            this.hand = genHnd(this.crdPool);
        }
    }
}

