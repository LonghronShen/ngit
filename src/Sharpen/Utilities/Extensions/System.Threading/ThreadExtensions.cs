using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace System.Threading
{

    public static class ThreadExtensions
    {

        public static void Abort(this Thread thread)
        {
            MethodInfo abort = null;
            foreach (MethodInfo m in thread.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if (m.Name.Equals("AbortInternal") && m.GetParameters().Length == 0) abort = m;
            }
            if (abort == null)
            {
                throw new Exception("Failed to get Thread.Abort method");
            }
            abort.Invoke(thread, new object[0]);
        }

    }

}