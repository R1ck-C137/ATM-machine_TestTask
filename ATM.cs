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
        private string banknotesFile = "../../../banknotes.txt";

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
            var result = TryWithdrawAmount(remainingAmount, banknotes);

            if (result != null && result.Value.Item1 != null && result.Value.Item2 != null)
            {
                banknotes = result.Value.Item1;
                return result.Value.Item2;
            }
            else
                return null;
        }

        private (Dictionary<int, int>, Dictionary<int, int>)? TryWithdrawAmount(int remainingAmount, Dictionary<int, int> banknotes, Dictionary<int, int>? withdrawal = null)
        {
            if(withdrawal == null)
                withdrawal = new Dictionary<int, int>();

            if (remainingAmount == 0)
                return (banknotes, withdrawal);

            foreach (var (nominal, count) in banknotes.OrderByDescending(kv => kv.Key))
            {
                if (count > 0 && nominal <= remainingAmount)
                {
                    var newListOfBanknotes = new Dictionary<int, int>(banknotes);
                    newListOfBanknotes[nominal]--;
                    var newWithdrawal = new Dictionary<int, int>(withdrawal);

                    if (newWithdrawal.ContainsKey(nominal))
                        newWithdrawal[nominal]++;
                    else
                        newWithdrawal.Add(nominal, 1);

                    var result = TryWithdrawAmount(remainingAmount - nominal, newListOfBanknotes, newWithdrawal);

                    if (result != null)
                        return result;
                }
            }

            return null;
        }
    }
}
