using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

//Завдання 1.
//Файл містить 100000 цілих чисел. Додаток має проаналізувати файл і відобразити статистику по ньому:
//Кількість додатних чисел. Кількість від'ємних чисел. Кількість двозначних чисел. Кількість п'ятизначних чисел.
//Крім того, додаток має створити файли з цими числами (додатні, від'ємні і т. д.).


namespace File_Array
{
    [Serializable]
    public class MyArray : IEnumerable<int>
    {
        private int[] array;
        public int Lenght { get; set; }

        public MyArray(int count, int minValue, int maxValue)
        {
            array = new int[count];
            Random random = new Random();

            for (int i = 0; i < count; i++)
            {
                array[i] = random.Next(minValue, maxValue + 1);
            }
        }

        public void PrintArray()
        {
            foreach (var item in array)
            {
                Console.Write(item + "\t");
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return ((IEnumerable<int>)array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return array.GetEnumerator();
        }
    }


    public class FilesHandling // класс для записи - чтения файла с объектом
    {
        public void SerializeObject(Object obj, string filePath)
        {
            try
            {
                using (FileStream writer = new FileStream(filePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(writer, obj);
                }

                Console.WriteLine("Объект успешно записан в файл: " + filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка при сериализации объекта: " + e.Message);
            }
        }

        public object DeserializeObject(string filePath)
        {
            try
            {
                using (FileStream reader = new FileStream(filePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка при десериализации объекта: " + e.Message);
                return null;
            }
        }

    }

    public class ArrayAnalyze //Кількість додатних чисел. Кількість від'ємних чисел. Кількість двозначних чисел. Кількість п'ятизначних чисел.
    {
        public int CountPositive(MyArray array)
        {
            int count = 0;
            foreach (int num in array)
            {
                if (num > 0)
                    count++;
            }
            return count;
        }
        public int CountNegative(MyArray array)
        {
            int count = 0;
            foreach (int num in array)
            {
                if (num < 0)
                    count++;
            }
            return count;
        }

        public int CountTwoDigit(MyArray array)
        {
            int count = 0;
            foreach (int num in array)
            {
                if ((num >= -99 && num <= -10) || (num >= 10 && num <= 99))
                    count++;
            }
            return count;
        }

        public int CountFiveDigit(MyArray array)
        {
            int count = 0;
            foreach (int num in array)
            {
                if (num >= -99999 && num <= -10000 || num >= 10000 && num <= 99999)
                    count++;
            }
            return count;
        }
    }



    internal class Program
    {
        static void Main(string[] args)
        {
            int arrayElementsCount = 100000;
            var myArray = new MyArray(arrayElementsCount, -20000, 20000);
            //myArray.PrintArray();

            string filePath = "ArrayTest.txt";
            var filesHandling = new FilesHandling();
            filesHandling.SerializeObject(myArray, filePath); // Запись массива-объекта в файл с именем "array.txt"

            object deserializedObject = filesHandling.DeserializeObject(filePath);
            if (deserializedObject == null)            // Проверка, был ли объект успешно десериализован
            {
                Console.WriteLine("Десериализация Объекта не осуществлена...");
                return;
            }
            MyArray myArray2 = (MyArray)deserializedObject;
            //myArray2.PrintArray();
            ArrayAnalyze arrayAnalyze = new ArrayAnalyze();
            Console.WriteLine("\n_______________________________________________________________________________________");
            Console.WriteLine("Колчество положительных элементов: " + arrayAnalyze.CountPositive(myArray2));
            Console.WriteLine("Колчество отрицательных элементов: " + arrayAnalyze.CountNegative(myArray2));
            Console.WriteLine("Колчество двузначных чисел: " + arrayAnalyze.CountTwoDigit(myArray2));
            Console.WriteLine("Колчество пятизначных чисел: " + arrayAnalyze.CountFiveDigit(myArray2));
        }
    }
}
