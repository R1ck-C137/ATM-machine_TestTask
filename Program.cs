namespace ATM_machine_TestTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var atm = new ATM();

            while (true)
            {
                try
                {
                    Console.Write("Введите сумму для снятия: ");
                    var amount = int.Parse(Console.ReadLine());
                    var withdrawal = atm.WithdrawAmount(amount);
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
