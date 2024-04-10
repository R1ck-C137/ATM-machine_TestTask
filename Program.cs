namespace ATM_machine_TestTask
{
    internal class Program
    {
        static Dictionary<int, int> LoadBanknotes(string filename)
        {
            return File.ReadAllLines(filename)
                       .Select(line => line.Split('-'))
                       .ToDictionary(parts => int.Parse(parts[0]), parts => int.Parse(parts[1]));
        }

        static Dictionary<int, int>? WithdrawAmount(int remainingAmount, Dictionary<int, int> banknotes)
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

        static void Main(string[] args)
        {
            var banknotesFile = "../../../banknotes.txt";
            var banknotes = LoadBanknotes(banknotesFile);

            while (true)
            {
                try
                {
                    Console.Write("Введите сумму для снятия: ");
                    var amount = int.Parse(Console.ReadLine());
                    var withdrawal = WithdrawAmount(amount, banknotes);
                    if (withdrawal != null)
                    {
                        Console.WriteLine("Выдано следующее количество купюр:");
                        foreach (var (nominal, count) in withdrawal)
                        {
                            Console.WriteLine($"{count}x{nominal}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Невозможно выдать указанную сумму.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: введите целое число.");
                }
            }
        }
    }

}
