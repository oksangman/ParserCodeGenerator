using ParserCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ParserCodeGenerator.ViewModels
{
    public class CreateClassViewModel : Base.Bindable
    {
        private Window _window;
        private ObservableCollection<GrammarModel> _grammarList;
        public CreateClassViewModel()
        {
            CreateCommand = new Base.Command(Create);
        }

        public void Initialize(Window window, ObservableCollection<GrammarModel> grammarList)
        {
            _window = window;
            _grammarList = grammarList;

        }


        #region Properties
        private string _namespace;
        public string Namespace
        {
            get { return _namespace; }
            set { SetProperty(ref _namespace, value); }
        }

        private bool _checkCSharp = true;
        public bool CheckCSharp
        {
            get { return _checkCSharp; }
            set { SetProperty(ref _checkCSharp, value); }
        }

        private bool _checkCpp = true;
        public bool CheckCpp
        {
            get { return _checkCpp; }
            set { SetProperty(ref _checkCpp, value); }
        }

        private bool _selectCreateFile = true;
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

        #region Command
        public ICommand CreateCommand;
        #endregion

        #region Command Function
        void Create()
        {
            if (SelectQuickView)
            {

                return;
            }
        }
        #endregion Command Function
    }



}
