using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public enum LocaleEnum
    {
        [Description("zh-tw")]
        tw,
        [Description("jp")]
        jp,
        [Description("en")]
        en
    }
}
