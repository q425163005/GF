using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fuse.Hotfix
{
    public class RandomHelper
    {
        /// <summary>ȫ�������</summary>
        private static readonly Random random = new Random(getRandomSeed());

        private static int getRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
       

        /// <summary>
        /// ����һ����Χ�ڵ������
        /// </summary>
        /// <param name="minValue">��Сֵ(��)</param>
        /// <param name="maxValue">���ֵ(��)</param>
        /// <returns></returns>
        public static int Random(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue + 1);
        }
        /// <summary>
        /// ��һ����Χ�ڣ����N�����ظ������� ����min ������Max
        /// </summary>
        /// <returns></returns>
        public static int[] Randoms(int Count, int minNum, int MaxNum)
        {
            if ((MaxNum - minNum) < Count)
            {
                Log.Info($"�������[{Count}]��������ֵ[{MaxNum - minNum}]");
                return null;
            }
            if (MaxNum < minNum)
            {
                Log.Info($"�������ֵ����[{MaxNum}]С��[{minNum}]");
                return null;
            }

            List<int> randomList = new List<int>();
            for (int i = minNum; i < MaxNum; i++)
            {
                randomList.Add(i);
            }
            List<int> result = new List<int>();
            for (int i = 0; i < Count; i++)
            {
                int MaxIndex = randomList.Count;
                int index = Random(0, MaxIndex - 1);
                result.Add(randomList[index]);
                randomList.RemoveAt(index);
            }
            return result.ToArray();
        }

        /// <summary>
        /// ����һ����Χ�ڵ������
        /// </summary>
        /// <param name="range">�����Χ[��Сֵ�����ֵ]</param>
        /// <returns></returns>
        public static int Random(int[] range)
        {
            if (range.Length < 2)
                return range[0];
            return Random(range[0], range[1]);
        }

        /// <summary>
        /// �жϰٷֱ�����Ƿ�����
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool RandomPercent(int val)
        {
            return random.Next(0, 100) < val;
        }
        /// <summary>
        /// �ж���ֱ�����Ƿ�����
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool RandomPercentW(int val)
        {
            if (val >= 10000) return true;
            return random.Next(0, 10000) < val;
        }


        /// <summary>
        /// ���б���������һ��ֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Randm<T>(List<T> list)
        {
            int randomVal = random.Next(list.Count);
            return list[randomVal];
        }

        /// <summary>
        /// ��������������һ��ֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Randm<T>(T[] list)
        {
            int randomVal = random.Next(list.Length);
            return list[randomVal];
        }


        /// <summary>
        /// �����������
        /// </summary>
        public static void RandomSort<T>(List<T> list)
        {
            T temp;
            for (int i = 0; i < list.Count; i++)
            {
                int index = RandomHelper.Random(0, i);
                temp = list[i];
                list[i] = list[index];
                list[index] = temp;
            }
        }

        /// <summary>
        /// �����������
        /// </summary>
        public static void RandomSort<T>(T[] list)
        {
            T temp;
            for (int i = 0; i < list.Length; i++)
            {
                int index = RandomHelper.Random(0, i);
                temp = list[i];
                list[i] = list[index];
                list[index] = temp;
            }
        }

        /// <summary>
        /// �����������ȡ�������ظ���
        /// ע�����޸�Դ�����е�����˳���������ı�Դ���ݣ����и��������ٴ�����
        /// </summary>
        /// <param name="list">Դ����</param>
        /// <param name="num">����ȡ����</param>
        /// <returns></returns>
        public static List<T> RandomGetNum<T>(List<T> _list, int maxNum, bool isClone = false)
        {
            List<T> list;
            if (isClone)
            {
                list = new List<T>();
                for (int i = 0; i < _list.Count; i++)
                    list.Add(_list[i]);
            }
            else
                list = _list;

            List<T> rtnList = new List<T>();
            int index;
            int arrLen = list.Count;
            for (int i = 0; i < maxNum; i++)
            {
                if (arrLen == 0)
                    break;
                index = RandomHelper.Random(0, arrLen - 1);
                rtnList.Add(list[index]);
                list[index] = list[arrLen - 1];
                arrLen -= 1;
            }

            return rtnList;
        }
        
        public static int RandomKey {
            get => UnityEngine.Random.Range(0, 0xffff);
        }

    }
}
