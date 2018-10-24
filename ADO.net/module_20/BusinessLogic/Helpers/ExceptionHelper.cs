using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using module_20.Helpers;

namespace module_20.BusinessLogic.Helpers
{
    static class ExceptionHelper
    {
        public static void CheckCollection(ICollection collection)
        {
            if (collection == null)
            {
                throw new NullReferenceException();
            }
        }  
    }
}
