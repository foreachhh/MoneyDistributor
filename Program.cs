using System.Globalization;

namespace MoneyDistributor
{
    internal class Program
    {
        static string confFilePath = AppContext.BaseDirectory + "config.txt";
        static Dictionary<string, float> distributionDirections = new Dictionary<string, float>();
        static void Main(string[] args)
        {
            while (true)
            {
                DictionaryToConsole();
                Console.WriteLine("Введите вашу зарплату. Для изменения пресета введите команду \"пресет\"");

                string input = Console.ReadLine();

                if(String.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Неккоректный ввод! Повторите попытку");
                    Console.ReadLine();
                }
                else if(input.Trim().ToLower() == "пресет")
                {
                    ChangePreset();
                }
                else
                {
                    Distributor(Convert.ToInt32(input));
                }
            }
        }
        static void DictionaryToConsole()
        {
            Console.Clear();
            Console.WriteLine("Текущий пресет для распределения:\n");
            foreach (var line in distributionDirections)
            {
                Console.WriteLine($"{line.Key} = {line.Value * 100}%");
            }
        }

        static void ChangePreset()
        {
            while (true)
            {
                DictionaryToConsole();
                Console.WriteLine("Вы хотите удалить или добавить значение?\n0. Удалить\n1. Добавить");
                string input = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(input))
                    return;
                
                else if (input == "1")
                {
                    AddKeyPair();
                }
                else if(input == "0")
                {
                    DeleteKeyPair();
                }
            }
        }
        static void AddKeyPair()
        {
            while (true)
            {
                DictionaryToConsole();
                Console.WriteLine("Для добавления введите значение в формате \"[название атрибута] = [значение для распределения]\".\nДля прекращения нажмите Enter");

                string elementBeignAdded = Console.ReadLine();

                if (String.IsNullOrWhiteSpace(elementBeignAdded))
                    return;

                DictionaryFiller(elementBeignAdded);
            }
        }
        static void DictionaryFiller(string line)    //Задача метода заполнить глобальный словарь отраслей, их названиями и коэффицентами
        {
            string[] splittedLine = line.Split('=');

            if (splittedLine.Length == 2)
            {
                string key = splittedLine[0].Trim();
                string valueStr = splittedLine[1].Trim();

                if (valueStr.Contains('%'))     //если пользователь написал строку со знаком процента то он убирается.
                    valueStr = valueStr.Remove(valueStr.Length - 1);

                if (float.TryParse(valueStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                {
                    value /= 100; //в словаре хранятся коэффеценты а не проценты. поэтому парсим

                    if(!IsCorrectValue(value))
                    {
                        Console.WriteLine("Сумма всех значений пресета не должна превышать 100%!");
                        Console.ReadLine();
                        return;
                    }
                    distributionDirections[key] = value;
                }
                else
                {
                    Console.WriteLine($"Не удалось преобразовать значение: {line}");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Вы ввели значение не по указанной маске!");
                Console.ReadLine();
            }
        }
        /// <summary>
        /// Проверяет, не приведёт ли добавление указанного коэффициента к превышению 100% (1.0).
        /// Возвращает <c>true</c>, если сумма существующих значений плюс <paramref name="val"/> не превышает 1.0.
        /// </summary>
        /// <param name="val">Коэффициент (в дробном виде, например 0.25 для 25%).</param>
        /// <returns><c>true</c>, если допустимо добавить значение; иначе <c>false</c>.</returns>
        static bool IsCorrectValue(float val)
        {
            foreach(var dir in distributionDirections)
                val += dir.Value;

            if (val <= 1)
                return true;
            else
                return false;
        }
        static void DeleteKeyPair()
        {
            while (true)
            {
                DictionaryToConsole();
                Console.WriteLine("Введите номер элемента который хотите удалить.\nДля прекращения нажмите Enter");

                string deleteIndexStr = Console.ReadLine();

                int deleteIndex;

                if (String.IsNullOrWhiteSpace(deleteIndexStr))
                    return;

                if (int.TryParse(deleteIndexStr, out deleteIndex) && deleteIndex < distributionDirections.Count && deleteIndex >= 0)
                {
                    string keyToDelete = distributionDirections.ElementAt(deleteIndex).Key;

                    distributionDirections.Remove(keyToDelete);
                }
                else
                {
                    Console.WriteLine("Некорректный ввод! Повторите ещё раз.");
                    Console.ReadLine();
                }
            }
        }
        static void Distributor(int value)
        {
            foreach (var dir in distributionDirections)
            {
                Console.WriteLine($"{dir.Key} = {value * dir.Value} руб");
            }
            Console.ReadLine();
        }
    }
}
