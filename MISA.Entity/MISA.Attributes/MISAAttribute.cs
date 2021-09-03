using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Entity.MISA.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MISARequired : Attribute
    {
        public string _fieldName = string.Empty;
        public string _message = string.Empty;

        public MISARequired(string fieldName)
        {
            _fieldName = fieldName;
            _message = $"Thông tin {_fieldName} không được để trống.";
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MISAExported : Attribute
    {
        public readonly string Name;
        public MISAExported(string name)
        {
            this.Name = name;
        }
    }
}
