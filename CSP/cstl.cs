using System.Collections.Generic;

namespace CSP
{
    public static class cstl
    {
        public delegate bool LCLAM(local_constraint con);

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
    }
}