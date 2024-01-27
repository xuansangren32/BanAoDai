using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BanAoDai.Libraries
{
    public static class MyString
    {
        public static string str_Slug(String s)
        {
            String[][] symbols =
            {
                //new String[] { "[aAeEoOuUiIdDyY]","aAeEoOuUiIdDyY" },
                 new String[] { "[áàạảãâấầậẩẫăắằặẳẵ]","a" },
                  //new String[] { "[ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ]","A" },
                   new String[] { "[éèẹẻẽêếềệểễ]","e" },
                    new String[] { "[ÉÈẸẺẼÊẾỀỆỂỄ]","E" },
                     new String[] { "[óòọỏõôốồộổỗơớờợởỡ]","o" },
                      //new String[] { "[ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ]","O" },
                       new String[] {"[úùụủũưứừựửữ]","u" },
                        //new String[] { "[ÚÙỤỦŨƯỨỪỰỬỮ]","U" },
                         new String[] {  "[íìịỉĩ]","i" },
                          //new String[] { "[ÍÌỊỈĨ]","I" },
                           //new String[] { "[Đ]","D" },
                            new String[] { "[đ]","d" },
                             new String[] { "[ýỳỵỷỹ]","y" },
                              //new String[] { "[ÝỲỴỶỸ]","Y" },
                              new String[] { "[\\s'\";,:]","-" }
                             
            };
            s=s.ToLower();
            foreach(var ss in symbols)
            {
                s = Regex.Replace(s, ss[0], ss[1]);
            }
            return s;
        }
    }
}