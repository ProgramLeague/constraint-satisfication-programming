using System;
using System.Collections.Generic;
using DIDTT=System.Collections.Generic.Dictionary<string,float>;
using LIDT=System.Collections.Generic.List<string>;
using LT=System.Collections.Generic.List<float>;
using KLDSL=System.Collections.Generic.KeyValuePair<System.Collections.Generic.List<float>, System.Collections.Generic.Dictionary<string,System.Collections.Generic.List<string>>>;

namespace CSP
{
    public static class cstl
    {
        public delegate bool LCLAM(local_constraint con);
        public delegate bool KD2LAM(KeyValuePair<string,KLDSL> l, KeyValuePair<string,KLDSL> r);

        public static readonly int limitmax = 2147483647;

        public static bool any_of(List<local_constraint> list, LCLAM determine)
        {
            foreach (var i in list)
            {
                if (determine(i))
                    return true;
            }
            return false;
        }

        public static int count(List<string> list, string val)
        {
            int Count = 0;
            foreach (var i in list)
            {
                if (i == val)
                    Count++;
            }
            return Count;
        }

        public static void pop_back(List<float> list)
        {
            list.RemoveAt(list.Count-1);
        }

        public static void assert(bool b)
        {
            System.Diagnostics.Debug.Assert(b);
        }

        public static KeyValuePair<string,KLDSL> min_element(Dictionary<string, KLDSL> variable, KD2LAM compare) //compare是小于号
        {
            KeyValuePair<string,KLDSL> ret;
            bool start = false;
            foreach (var i in variable)
            {
                if (!start)
                {
                    start = true;
                    ret = i;
                    continue;
                }
                if (compare(i, ret)) //i是否小于ret
                    ret = i;
            }
            return ret;
        }

        public static bool count(DIDTT partial_assignment, string key)
        {
            try
            {var a = partial_assignment[key];}
            catch (Exception e)
            {return false;}
            return true;
        }
    }
}