using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyWordsWPF.View
{
    public class ExamplesMock
    {
        public ExamplesMock()
        {
            Examples.Add("test text 1");
            Examples.Add("test text 4");
        }

        public List<string> Examples { get; set; }
    }
}
