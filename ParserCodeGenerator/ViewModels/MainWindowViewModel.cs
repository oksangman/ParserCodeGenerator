using Microsoft.Win32;
using Newtonsoft.Json;
using ParserCodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ParserCodeGenerator.ViewModels
{
    public class MainWindowViewModel : Base.Bindable
    {

        private System.Windows.Window _mainWindow;
        public MainWindowViewModel()
        {
            OpenFileCommand = new Base.Command(OpenFile);
            SaveFileCommand = new Base.Command(SaveFile);
            ExitCommand = new Base.Command(Exit);
        }

        public void Init(System.Windows.Window mainWindow)
        {
            _mainWindow = mainWindow;

            if (true)
            {
                string json = File.ReadAllText("test.json");
                MainWindowViewModel temp = Newtonsoft.Json.JsonConvert.DeserializeObject<MainWindowViewModel>(json);
                GrammarList = temp.GrammarList;
                TestString = temp.TestString;
            }
        }

#region Properties
        private List<GrammarModel> _grammerList = new List<GrammarModel>();
        public List<GrammarModel> GrammarList
        {
            get { return _grammerList; }
            set { SetProperty(ref _grammerList, value); }
        }

        private string _testString;
        public string TestString
        {
            get { return _testString; }
            set { SetProperty(ref _testString, value); }
        }

        private string _resultString;
        [JsonIgnore]
        public string ResultString
        {
            get { return _resultString; }
            set { SetProperty(ref _resultString, value); }
        }

        private List<ResultModel> _resultList = new List<ResultModel>();
        [JsonIgnore]
        public List<ResultModel> ResultList
        {
            get { return _resultList; }
            set { SetProperty(ref _resultList, value); }
        }
#endregion Properties

#region Command
        [JsonIgnore]
        public ICommand OpenFileCommand { get; set; }
        [JsonIgnore]
        public ICommand SaveFileCommand { get; set; }

        [JsonIgnore]
        public ICommand ExitCommand { get; set; }
#endregion Command

#region Command Function
        void OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text files (*.json)|*.json|All files (*.*)|*.*";
            ofd.Multiselect = false;
            ofd.InitialDirectory = Environment.CurrentDirectory;
            bool? ret = ofd.ShowDialog(_mainWindow);
            if (!ret.HasValue)
                return;

            if (!ret.Value)
                return;

            if (!File.Exists(ofd.FileName))
                return;

            string json = File.ReadAllText(ofd.FileName);
            MainWindowViewModel temp = Newtonsoft.Json.JsonConvert.DeserializeObject<MainWindowViewModel>(json);
            GrammarList = temp.GrammarList;
            TestString = temp.TestString;
        }


        void SaveFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text files (*.json)|*.json|All files (*.*)|*.*";
            sfd.InitialDirectory = Environment.CurrentDirectory;
            bool? ret = sfd.ShowDialog(_mainWindow);
            if (!ret.HasValue)
                return;

            if (!ret.Value)
                return;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(sfd.FileName, json);
        }

        void Exit()
        {
            _mainWindow.Close();
        }
#endregion Command Function
    }
}
