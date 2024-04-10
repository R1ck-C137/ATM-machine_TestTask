using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM_machine_TestTask
{
    public class ATM
    {
        private Dictionary<int, int> banknotes = new Dictionary<int, int>();
        string banknotesFile = "../../../banknotes.txt";

        public ATM() 
        {
            banknotes = LoadBanknotes(banknotesFile);
        }

        public ATM(string banknotesFile)
        {
            banknotes = LoadBanknotes(banknotesFile);
        }

        private static Dictionary<int, int> LoadBanknotes(string filename)
        {
            return File.ReadAllLines(filename)
                       .Select(line => line.Split('-'))
                       .ToDictionary(parts => int.Parse(parts[0]), parts => int.Parse(parts[1]));
        }

        public Dictionary<int, int>? WithdrawAmount(int remainingAmount)
        {
            var withdrawal = new Dictionary<int, int>();
            var availableBanknotes = banknotes.OrderByDescending(kv => kv.Key);

            foreach (var (nominal, count) in availableBanknotes)
            {
                var withdrawnCount = Math.Min(remainingAmount / nominal, count);
                if (withdrawnCount > 0)
                {
                    withdrawal[nominal] = withdrawnCount;
                    remainingAmount -= withdrawnCount * nominal;
                }
            }

            if (remainingAmount == 0)
            {
                foreach (var (nominal, count) in withdrawal)
                    banknotes[nominal] -= count;
                return withdrawal;
            }
            else
                return null;
        }
    }
}
