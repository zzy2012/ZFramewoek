using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bit
{
    public static int Bit_2_31 = (int)Mathf.Pow(2, 31);
    public static bool CheckBitMap(int[] map, int key, bool unset = false)
    {
        int min = 1;
        int max = map.Length * 53;
        if (key < min || key > max)
        {
            Debug.LogError("checkBitMap: bit=" + key + " is out of map range " + min + " - " +max);
        }
        return unset ? !(Chkbita(map, key)) : (Chkbita(map, key));
    }

    public static bool Chkbita(int[] map, int key)
    {
        if (key > map.Length * 53) return false;
        int j = Mathf.CeilToInt(key / 53);
        int a = key % 53;
        a = a == 0 ? 53 : a;
        if (Band54(map[j], Lshift54(1, a - 1)) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static int Band54(int a, int b)
    {
        int a1 = a / Bit_2_31;
        int a2 = a % Bit_2_31;
        int b1 = b / Bit_2_31;
        int b2 = b % Bit_2_31;
        a1 &= b1;
        a2 &= b2;
        return a1 * Bit_2_31 + a2;
    }

    public static int Lshift54(int a, int b)
    {
        return a * (int)(Mathf.Pow(2, b));
    }

    public static int[] SetbitMap(int[] map, int[] bits)
    {
        int min = 1;
        int max = map.Length * 53;
        if (bits.Length > 0)
        {
            int len = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                int bit = bits[i];
                if (bit > max) Debug.LogError("setBitMap :  bit " + bit + "is bigger than max number" + max);
                if (bit < min) Debug.LogError("setBitMap :  bit " + bit + "less than min number 1");
                map = Setbita(map, bit);
                len++; ;
            }
        }
        return map;
    }

    public static int[] Setbita(int[] map, int bit)
    {
        int j = Mathf.CeilToInt(bit / 53);
        int b = bit % 53;
        int val = map[j];
        b = b == 0 ? 53 : b;
        map[j] = Bor54(val, Lshift54(1, b-1));
        return map;
    }

    public static int Bor54(int a, int b)
    {
        int a1 = (int)(a / Bit_2_31);
        int a2 = (int)(a % Bit_2_31);
        int b1 = b / Bit_2_31;
        int b2 = b % Bit_2_31;
        a1 |= b1;
        a2 |= b2;
        return a1 * Bit_2_31 + a2;
    }

    public static int[] Broa(int[] a, int[] b)
    {
        int[] ret =  new int[] { };
        for (int i = 0; i < a.Length; i++)
        {
            ret[i] = Bor54(a[i], b[i]);
        }
        return ret;
    }

    public static int[] Banda(int[] a, int[] b)
    {
        int[] ret = new int[] { };
        for (int i = 0; i < a.Length; i++)
        {
            ret[i] = Band54(a[i], b[i]);
        }
        return ret;
    }

    public static List<int> Cmpbita(int[] a, int[] b)
    {
        List<int> ret = new List<int>();
        for (int i = 0; i < a.Length; i++)
        {
            int d = Bxor54(a[i], b[i]);
            if (d != 0)
            {
                for (int j = 0; j < 54; j++)
                {
                    if (CheckBit54(d, j))
                    {
                        ret.Add(i * 53 + j);
                    }
                }
            }
        }
        return ret;
    }

    public static bool CheckBit54(int a, int b)
    {
        if (Band54(a, Lshift54(1, b - 1)) == 0)
        {
            return false;
        }
        return true;
    }

    public static int Bxor54(int a, int b)
    {
        int a1 = (int)(a / Bit_2_31);
        int a2 = (int)(a % Bit_2_31);
        int b1 = (int)(b / Bit_2_31);
        int b2 = (int)(b % Bit_2_31);
        a1 ^= b1;
        a2 ^= b2;
        return a1 * Bit_2_31 + a2;
    }
}
