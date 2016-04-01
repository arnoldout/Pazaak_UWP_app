using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
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

        //constructor for a player, it needs a card pool and an identifier
        public Player(Card[] crdPool, String name) {
            this.crdPool = crdPool;
            //hand is populated randomly from the card pool
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
        //method that keeps track of the game loop
        public void rndWn(Player p, InGame ig)
        {
            //the game is reset once the amount of rounds won by the current player is above 2
            if (this.rndsWn<2)
            {
                ResourceCandidate rc;
                rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/lost",
                ResourceContext.GetForCurrentView());
                string lost = rc.ValueAsString;

                this.rndsWn++;
                ig.status = lost;
            }
            else
            {
                ResourceCandidate rc;
                rc = ResourceManager.Current.MainResourceMap.GetValue("Resources/newGm",
                ResourceContext.GetForCurrentView());
                string newGm = rc.ValueAsString;

                this.rndsWn++;
                ig.status = newGm;
                this.gmsWn++;
                //HighScore h = new HighScore(this.gmsWn, p.gmsWn);
                //h.readScores();
                //h.writeScores();
                
            }
        }
        public Card[] genHnd(Card [] crdPool)
        {
            Card[] pool = crdPool;
            Card[] hand = new Card[4];
            //4 random cards are pulled from the card pool
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
            //the card is added to the table
            onBrd[this.trnCnt] = c;    
        }
        public void reset()
        {
            //reset non crucial variables
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
            //hand and rounds won are reset
            //as well as non crucial variables
            reset();
            this.rndsWn = 0;
            this.hand = genHnd(this.crdPool);
        }
    }
}

