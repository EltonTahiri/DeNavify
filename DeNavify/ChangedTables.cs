using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeNavify
{
    public class ChangedFields
    {
        public string Name { get; set; }    
    }

    public class ChangedTable
    {
        private List<ChangedFields> _fields;
        public ChangedTable() 
        {
            _fields = new List<ChangedFields>();
        }
        public string Name { get; set; }        

        public List<ChangedFields> ChangedFields
        {
            get
            {
                return _fields;
            }
            set
            {
                _fields = value;
            }
        }
    }
}
