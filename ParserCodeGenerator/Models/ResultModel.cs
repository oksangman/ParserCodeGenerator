using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCodeGenerator.Models
{
    public class ResultModel : Base.Bindable
    {
        public ResultModel()
        {
            _order = "";
            _grammer = "";
            _value = "";
            _index = 0;
            _length = 0;
        }
        public ResultModel(string order, string grammer, string value, int index, int length)
        {
            _order = order;
            _grammer = grammer;
            _value = value;
            _index = index;
            _length = length;
        }

        private string _order;
        public string Order
        {
            get { return _order; }
            set { SetProperty(ref _order, value); }
        }

        private string _grammer;
        public string Grammar
        {
            get { return _grammer; }
            set { SetProperty(ref _grammer, value); }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        private int _length;
        public int Length
        {
            get { return _length; }
            set { SetProperty(ref _length, value); }
        }
    }
}
