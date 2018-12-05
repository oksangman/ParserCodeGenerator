using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCodeGenerator.ViewModels
{
    public class CreateClassViewModel : Base.Bindable
    {
        #region Properties
        private string _namespace;
        public string Namespace
        {
            get { return _namespace; }
            set { SetProperty(ref _namespace, value); }
        }

        private bool _checkCSharp;
        public bool CheckCSharp
        {
            get { return _checkCSharp; }
            set { SetProperty(ref _checkCSharp, value); }
        }

        private bool _checkCpp;
        public bool CheckCpp
        {
            get { return _checkCpp; }
            set { SetProperty(ref _checkCpp, value); }
        }

        private bool _selectCreateFile;
        public bool SelectCreateFile
        {
            get { return _selectCreateFile; }
            set { SetProperty(ref _selectCreateFile, value); }
        }

        private bool _selectQuickView;
        public bool SelectQuickView
        {
            get { return _selectQuickView; }
            set { SetProperty(ref _selectQuickView, value); }
        }
        #endregion Properties
    }



}
