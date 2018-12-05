using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCodeGenerator.Models
{
    public class GrammarModel : Base.Bindable
    {
        public GrammarModel()
        {
            _regularExpression = "";
            _name = "";
            _comment = "";
        }
        public GrammarModel(string regex, string name, string comment)
        {
            _regularExpression = regex;
            _name = name;
            _comment = comment;
        }


        private string _regularExpression;
        public string RegularExpression
        {
            get { return _regularExpression; }
            set { SetProperty(ref _regularExpression, value); }
        }
        string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }
    }
}
