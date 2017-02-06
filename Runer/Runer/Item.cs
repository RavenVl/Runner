using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Runer
{
    class Item
    
    {
        
            public string text { get; set; }
            public bool activ { get; set; }
            //public bool dostup { get; set; }

            public EventHandler Click;

            public Item(string t)
            {
                this.text = t;
                this.activ = true;

            }

            public void OnClick()
            {
                if (Click != null)
                {
                    Click(this, null);
                }
            }
   
    }

}
