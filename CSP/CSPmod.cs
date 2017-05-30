using System.Collections.Generic;
using System.Linq;
using DIDTT=System.Collections.Generic.Dictionary<string,float>;
using LIDT=System.Collections.Generic.List<string>;
using LT=System.Collections.Generic.List<float>;
using KLDSL=System.Collections.Generic.KeyValuePair<System.Collections.Generic.List<float>, System.Collections.Generic.Dictionary<string,System.Collections.Generic.List<string>>>;

using RESULT=CSTD.ptr<System.Collections.Generic.Dictionary<string,float>>; //初始化为DIDTT
using PARELM=CSTD.ptr<float>; //初始化为float（result类型为LT）

namespace CSP
{
    public static class CSPmod
    {
        //backtracking_search的帮助函数
        private static bool recursion1(LT arg,List<LT> parameters,local_constraint con,bool need_var,int shrink_position,float it)
        {
            if (need_var)
                return true;

            if (arg.Count == parameters.Count)
            {
                if (con.predicate(arg))
                    return true;
            }
            else
            {
                if ( arg.Count == shrink_position )
                {
                    arg.Add(it);
                    need_var=recursion1(arg,parameters,con,false,shrink_position,it);
                    cstl.pop_back(arg);
                }
                else
                {
                    cstl.assert(arg.Count!=shrink_position);
                    if ( parameters[arg.Count].Count==0 )
                    { need_var = true; }

                    foreach (float i in parameters[arg.Count])
                    {
                        arg.Add(i);
                        need_var=recursion1(arg,parameters,con,need_var,shrink_position,it);
                        cstl.pop_back(arg);

                        if ( need_var )
                        { return true; } //backtracking_search返回点
                    }
                }
            }
            return need_var;
        }

        public static RESULT backtracking_search(
            Dictionary<string,
                        KeyValuePair<LT, Dictionary<string,LIDT>>
                    >variable,
            DIDTT partial_assignment,
            HashSet<string> modify_variable,
            uint generalized_arc_consistency_upperbound,
            List<local_constraint> constraint_set,
            RESULT result)
        {
            cstl.assert(partial_assignment.Count <= variable.Count);

            if ( modify_variable.Count == 1 )
            {
                var mit = modify_variable.First();
                cstl.assert( mit != modify_variable.Last());
                var pit = partial_assignment[mit];
                if( pit != partial_assignment.Last().Value)
                {
                    LT newlt=new LT();
                    newlt.Add(pit);

                    variable[mit]=new KeyValuePair<LT, Dictionary<string, LIDT>>(newlt,variable[mit].Value); //改变key不改变value
                }
            }

            if (cstl.any_of(constraint_set,
                           con=> {return !con.brackets(partial_assignment);}
                ))
            {return result;}

            if ( partial_assignment.Count == variable.Count)
            {
                result.change(partial_assignment);
                result.resultptr++;
                return result;
            }

            while (modify_variable.Count!=0)
            {
                string current = modify_variable.First();
                modify_variable.Remove(current);
                foreach (var con in constraint_set)
                {
                    if (con.related_var.Count <= generalized_arc_consistency_upperbound &&
                        con.related_var.Count >= 2 &&
                        cstl.count(con.related_var, current) != 0)
                    {
                        List<LT> parameters=new List<LT>();
                        foreach (var t in con.related_var)
                        {
                            var it = variable[t];
                            //cstl.assert(it!=variable.Last().Value);
                            parameters.Add(it.Key);
                        }
                        //parameters成为了con.related_var的拷贝
                        for (int shrink_position = 0; shrink_position < parameters.Count; shrink_position++)
                        {
                            bool arc_prune = false;
                            for (var it = new PARELM(parameters[shrink_position], 0); //获取的是迭代器
                                it.get() != parameters[shrink_position].Last();)
                            {
                                bool need_var = false;
                                LT arg=new LT();

                                need_var=recursion1(arg,parameters,con,need_var,shrink_position,it.get());

                                if (need_var == false)
                                {
                                    modify_variable.Add(con.related_var[ shrink_position ]);
                                    parameters[shrink_position].Remove(it.get());
                                    it.resultptr++;
                                    arc_prune = true;
                                    var it2 = variable[con.related_var[shrink_position]];
                                    //cstl.assert(it2!=variable.Last());
                                    foreach (var t in con.related_var)
                                    {
                                        if ( t != con.related_var[ shrink_position ] )
                                        {
                                            var i = variable[t];
                                            //cstl.assert( i != variable.Last());
                                            cstl.assert(i.Key.Count!=0);
                                            it2.Value[t] = i.Value.Keys.ToList();
                                        }
                                    }
                                }

                                //自增代码
                                if (arc_prune == false)
                                    it.resultptr++;
                                else
                                    arc_prune = false;
                            }
                        }
                        KeyValuePair<string,KLDSL> next_element = cstl.min_element(variable,
                            (KeyValuePair<string,KLDSL> l, KeyValuePair<string,KLDSL> r) =>
                            {
                                return (!cstl.count(partial_assignment, l.Key) ? l.Value.Key.Count : cstl.limitmax) <
                                       (!cstl.count(partial_assignment, r.Key) ? r.Value.Key.Count : cstl.limitmax);
                            });
                        cstl.assert(!cstl.count(partial_assignment,next_element.Key));
                    }
                }
            }

        }
    }
}