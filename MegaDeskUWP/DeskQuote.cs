using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MegaDeskUWP
{
    class DeskQuote
    {

        const int basePrice = 200;
        const int pricePerInchSquared = 1;
        const int pricePerDrawer = 50;

        


        
        public string CustomerName { get; set; }

        public int RushOrder { get; set; }

        public String QuoteDate { get; set; }

        public decimal QuotePrice { get; set; }

        public Desk Desk { get; set; }

        public string[] RushOrderPriceArray { get; set; }

        public int GetRushOrder()
        {

            string[] rushOrderPricesArray = RushOrderPriceArray; 
            string[,] newRushOrderPrices = new string[9, 2]
            {
                {"3", rushOrderPricesArray[0] },
                {"3", rushOrderPricesArray[1] },
                {"3", rushOrderPricesArray[2] },
                {"5", rushOrderPricesArray[3] },
                {"5", rushOrderPricesArray[4] },
                {"5", rushOrderPricesArray[5] },
                {"7", rushOrderPricesArray[6] },
                {"7", rushOrderPricesArray[7] },
                {"7", rushOrderPricesArray[8] }
            };

            if (RushOrder == 3 && DeskSize() < 1000)
            {
                return Int32.Parse(newRushOrderPrices[0, 1]);

            }
            else if (RushOrder == 3 && DeskSize() >= 1000 && DeskSize() <= 2000)
            {
                return Int32.Parse(newRushOrderPrices[1, 1]);

            }
            else if (RushOrder == 3 && DeskSize() > 2000)
            {
                return Int32.Parse(newRushOrderPrices[2, 1]);
            }
            else if (RushOrder == 5 && DeskSize() < 1000)
            {
                return Int32.Parse(newRushOrderPrices[3, 1]);

            }
            else if (RushOrder == 5 && DeskSize() >= 1000 && DeskSize() <= 2000)
            {
                return Int32.Parse(newRushOrderPrices[4, 1]);

            }
            else if (RushOrder == 5 && DeskSize() > 2000)
            {
                return Int32.Parse(newRushOrderPrices[5, 1]);
            }
            else if (RushOrder == 7 && DeskSize() < 1000)
            {
                return Int32.Parse(newRushOrderPrices[6, 1]);

            }
            else if (RushOrder == 7 && DeskSize() >= 1000 && DeskSize() <= 2000)
            {
                return Int32.Parse(newRushOrderPrices[7, 1]);

            }
            else if (RushOrder == 7 && DeskSize() > 2000)
            {
                return Int32.Parse(newRushOrderPrices[8, 1]);
            }
            else
            {
                return 0;
            }






        }

        public DeskQuote(Desk desk)
        {
            this.Desk = desk;

        }

        public int DeskSize()
        {
            int size = Desk.Width * Desk.Depth;
            return size;
        }

        public decimal GetQuote()
        {
            int drawers = Desk.Drawers;
            decimal totalPrice;
            int size = DeskSize();
            string material = Desk.DeskMaterial;
            int materialPrice = 0;
            int rushPrice;
            int sizePrice;


            if (size > 1000)
            {
                sizePrice = (size - 1000) * pricePerInchSquared;
            }
            else
            {
                sizePrice = 0;
            }

            switch (material)
            {
                case "Laminate":
                    materialPrice = (int)Desk.Material.Laminate;
                    break;
                case "Oak":
                    materialPrice = (int)Desk.Material.Oak;
                    break;
                case "Rosewood":
                    materialPrice = (int)Desk.Material.Rosewood;
                    break;
                case "Veneer":
                    materialPrice = (int)Desk.Material.Veneer;
                    break;
                case "Pine":
                    materialPrice = (int)Desk.Material.Pine;
                    break;
            }

            rushPrice = GetRushOrder();



            totalPrice = basePrice + sizePrice + (pricePerDrawer * drawers) + materialPrice + rushPrice;

            return totalPrice;
        }
    }
}
