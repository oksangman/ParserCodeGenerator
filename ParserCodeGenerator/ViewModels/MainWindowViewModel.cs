using Microsoft.Win32;
using Newtonsoft.Json;
using ParserCodeGenerator.Models;
using ParserCodeGenerator.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;
using WPFLocalizeExtension.Engine;

namespace ParserCodeGenerator.ViewModels
{
    public class MainWindowViewModel : Base.Bindable
    {
        private System.Windows.Window _mainWindow;
        private BackgroundWorker _worker = new BackgroundWorker();
        private GrammarBuilder _grammarBuilder = new GrammarBuilder();
        public MainWindowViewModel()
        {
            OpenFileCommand = new Base.Command(OpenFile);
            SaveFileCommand = new Base.Command(SaveFile);
            LoadExampleCommand = new Base.Command(LoadExample);
            ExitCommand = new Base.Command(Exit);

            BuildTestCommand = new Base.Command(BuildTest);
            CreateClassCommand = new Base.Command(CreateClass);
            LocalizingCommand = new Base.Command(Localizing);

            DataGridMoveUpDownCommand = new Base.Command<Key>(DataGridMoveUpDown);
        }

        public void Init(System.Windows.Window mainWindow)
        {
            _mainWindow = mainWindow;


            ProgressMaximum = 1;
            ProgressMinimum = 0;
            ProgressValue = 0;

            _grammarBuilder.ProgressDelegate += _grammarBuilder_ProgressDelegate;
            _worker.DoWork += worker_DoWork;

        }

        #region Properties
        private GrammarModel _selectedGrammer;
        public ParserCodeGenerator.Models.GrammarModel SelectedGrammer
        {
            get { return _selectedGrammer; }
            set { SetProperty(ref _selectedGrammer, value); }
        }
        private ObservableCollection<GrammarModel> _grammarList = new ObservableCollection<GrammarModel>();
        public ObservableCollection<GrammarModel> GrammarList
        {
            get { return _grammarList; }
            set { SetProperty(ref _grammarList, value); }
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

        private double _progressMaximum;
        [JsonIgnore]
        public double ProgressMaximum
        {
            get { return _progressMaximum; }
            set { SetProperty(ref _progressMaximum, value); }
        }

        private double _progressMinimum;
        [JsonIgnore]
        public double ProgressMinimum
        {
            get { return _progressMinimum; }
            set { SetProperty(ref _progressMinimum, value); }
        }

        private double _progressValue;
        [JsonIgnore]
        public double ProgressValue
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }
        #endregion Properties

        #region Command
        [JsonIgnore]
        public ICommand OpenFileCommand { get; set; }
        [JsonIgnore]
        public ICommand SaveFileCommand { get; set; }
        [JsonIgnore]
        public ICommand LoadExampleCommand { get; set; }

        [JsonIgnore]
        public ICommand ExitCommand { get; set; }

        [JsonIgnore]
        public ICommand BuildTestCommand { get; set; }
        [JsonIgnore]
        public ICommand CreateClassCommand { get; set; }
        [JsonIgnore]
        public ICommand LocalizingCommand { get; set; }

        [JsonIgnore]
        public ICommand DataGridMoveUpDownCommand { get; set; }
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

        void LoadExample()
        {
            Uri uri = new Uri("pack://application:,,,/Resources/example.json", UriKind.Absolute);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            StreamReader sr = new StreamReader(info.Stream);

            string json = sr.ReadToEnd();
            MainWindowViewModel temp = Newtonsoft.Json.JsonConvert.DeserializeObject<MainWindowViewModel>(json);
            GrammarList = temp.GrammarList;
            TestString = temp.TestString;
        }


        void Exit()
        {
            _mainWindow.Close();
        }

        void BuildTest()
        {
            _grammarBuilder.Clear();
            foreach (var item in GrammarList)
                _grammarBuilder.AddRegExToken(item.RegularExpression, item.Name);

            _mainWindow.IsEnabled = false;
            _worker.RunWorkerAsync(TestString);
        }

        void CreateClass()
        {
            --ProgressValue;

        }

        void Localizing()
        {
            CultureInfo newCulture;
            if (LocalizeDictionary.Instance.Culture.Name.Contains("en-US"))
            {
                newCulture = CultureInfo.GetCultureInfo("ko-KR");
            }
            else
            {
                newCulture = CultureInfo.GetCultureInfo("en-US");
            }

            LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            LocalizeDictionary.Instance.Culture = newCulture;
        }

        void DataGridMoveUpDown(Key direction)
        {
            //GrammarModel selected = SelectedGrammer;
            //List<GrammarModel> list = GrammarList;
            //int target = list.IndexOf(selected) - 1;
            //if (target < 0)
            //    return;
            //list.Remove(selected);
            //list.Insert(target, selected);
            //
            //RaisePropertyChanged("GrammarList");
            ////GrammarList = list;
            //SelectedGrammer = selected;
        }
        #endregion Command Function

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            string str = (string)e.Argument;
            List<GrammarBuilder.Token> list = _grammarBuilder.Generate(str);
            List<ResultModel> resultList = new List<ResultModel>();
            int i = 0;

            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                resultList.Add(new ResultModel(++i, item.Name, item.Value.Trim(), item.Index, item.Length));
                sb.Append($"{{{item.Name}}}");
            }
            _mainWindow.Dispatcher.Invoke(() =>
            {
                ResultList = resultList;
                ResultString = sb.ToString();
                _mainWindow.IsEnabled = true;
            });
        }

        private void _grammarBuilder_ProgressDelegate(double progress)
        {
            _mainWindow.Dispatcher.Invoke(() =>
            {
                ProgressValue = progress;
            });
        }
    }
}
