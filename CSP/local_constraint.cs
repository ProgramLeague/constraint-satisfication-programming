using System.Linq;
using DIDTT=System.Collections.Generic.Dictionary<string,float>;
using LIDT=System.Collections.Generic.List<string>;
using LT=System.Collections.Generic.List<float>;

namespace CSP
{
    public class local_constraint
    {
        public delegate bool PREDICATE(LT lt);

        public LIDT related_var;
        public PREDICATE predicate;

        public local_constraint(LIDT related_var,PREDICATE predicate)
        {
            this.related_var = related_var;
            this.predicate = predicate;
        }

        public bool brackets(DIDTT partial_assignment)
        {
            LT arg=new LT();
            foreach (string i in related_var)
            {
                var it = partial_assignment[i];
                if (it == partial_assignment.Values.Last())
                {return true;}

                arg.Add(it);
            }
            return this.predicate(arg);
        }
    }
}