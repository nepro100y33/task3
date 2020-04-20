using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution__1_List_
{
    class DynamicArray<T> where T : new()
    {
        T[] array;
        //!!! здесь она увеличивается только в 2 раза, так? А как увеличить на другое количество, если добавляем массив?
        //!!! И да, лучше использовать Capacity, а не Length, так же логичнее
        private void IncreaseCapacity() //увеличение емкости в два раза
        {
            IncreaseCapacity(Capacity * 2); 
        }

        private void IncreaseCapacity(int size) //увеличение емкости на необходимую величину
        {
            T[] cap = array;
            array = new T[size + Capacity];

            for (int i = 0; i < cap.Length; i++)
            {
                array[i] = cap[i];
            }
        }
        //Свойство Length–получение длины заполненной части массива
        //!!! добавьте приватное поле length, доступ к которому будет осуществляться через это свойство
        //!!! на set увеличивать/уменьшать его, плюс проверка на выход за границы Capacity
        private int lenght;
        public int Length
        {            
            get
            {
                return lenght;
            }  
            set
            {
                lenght = value;
                if (lenght > Capacity)
                {
                    IncreaseCapacity();
                }
            }
        }

        //Свойство Capacity–получение реальной ёмкости массива
        public int Capacity
        {
            get
            {
                return array.Length;
            }
        }
        //Конструктор без параметров
        public DynamicArray()
        {
            array = new T[8];
        }
        //Конструктор с 1 целочисленным параметром
        public DynamicArray(int size)
        {
            array = new T[size];
        }
        //Конструктор, который в качестве параметра принимает массив
        public DynamicArray(T[] arr)
        {
            array = new T[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                array[i] = arr[i];
            }
            lenght = arr.Length;
        }
        //Метод Add, добавляющий в конец массива один элемент
        public void Add(T elem)
        {
            if (Length != Capacity)//если емкость позволяет, добавляем элемент
            {
                array[Length] = elem;
            }
            else
            {
                IncreaseCapacity();//если нет, увеличиваем емкость
                array[Length] = elem;
            }
            lenght++;
        }
        //МетодAddRange, добавляющий в конец массива содержимое переданного массива
        public void AddRange(T[] arr)
        {
            if (Capacity - Length <= arr.Length)
            {
                IncreaseCapacity(arr.Length);
            }
            for (int i = 0; i < arr.Length; i++)
            {
                array[lenght] = arr[i];//!!! вы здесь заносите данные только в один элемент array[Length]
                lenght++;
            }
        }
        //!!! лучше удалить элемент, а остальные сдвинуть
        public void RemoveAt(int index)//удалить элемент по индексу
        {
            if (index >= Length)
            {      
                throw new Exception($"Выход за пределы массива");
            }
            int pos = index + 1;
            if (pos < Length)
            {
                Array.Copy(array, index + 1, array, index, lenght - 1 - index);
            }
            lenght--;
        }
        //Метод Remove, удаляющий из коллекции указанный элемент
        public bool Remove(T elem)//удалить указанный элемент
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(elem))
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    
        //Метод Insert, позволяющий добавить элемент в произвольную позицию массива
        public void Insert(T elem, int position)
        {
            if (position < Capacity && position >= 0)//проверка на возможнось вставки элемента в выбранную позицию
            {
                if (Capacity <= Length)
                {
                    IncreaseCapacity(); //увеличиваем емкость
                }
                for (int i = 0; i < Length - position; i++)//!!! дублирование кода, вынесите его за пределы условного оператора
                {
                    array[Length - i] = array[Length - i - 1];
                }
                array[position] = elem;//устанавливаем элемент на выбранную позицию
                lenght++;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        //Индексатор, позволяющий работать с элементом с указанным номером
        //!!! а если индекс отрицательный?
        public T this[int index]
        {
            get
            {
                if (index < Capacity && index >= 0)
                {
                    return array[index];
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                if (index < Capacity && index >= 0)
                {
                    array[index] = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }


        public void Sort(Func<T, T, int> FComp)
        {
            T temp;

            for (int j = 0; j < array.Length - 1; j++)
            {
                for (int i = 0; i < array.Length - 1; i++)
                {
                    if (FComp(array[i], array[i + 1]) == 1)
                    {
                        temp = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = temp;
                    }
                }
            }
        }

        public void Filter (Func<T, bool> FEquals)
        {
            for (int i = 0; i < Length; i++)
            {
                if (!FEquals(array[i]))
                {
                    RemoveAt(i--);
                }
            }
        }
    }
}
