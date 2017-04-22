using System.Collections.Generic;
using DIDTT=System.Collections.Generic.Dictionary<string,float>;
using LIDT=System.Collections.Generic.List<string>;
using LT=System.Collections.Generic.List<float>;

namespace CSTD
{
    public class ptr<T>
    {
        private List<T> result;
        public int resultptr;

        public ptr(List<T> result, int resultptr)
        {
            this.result = result;
            this.resultptr = resultptr;
        }

        public T get()
        {
            return this.result[resultptr];
        }

        public void change(T content)
        {
            this.result[resultptr] = content;
        }

        /*public T at(int sub)
        {
            return this.result[sub];
        }

        public int count()
        {
            return result.Count;
        }*/
    }
}