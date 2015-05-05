using Calculator.Common;
using Calculator.Model;
using Microsoft.ServiceFabric.Actors;
using StatelessCalculatorActor.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Calculator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly CultureInfo _culture;
        private bool _autoClear;
        private bool _dotUsed;
        private double _firstValue;
        private double _secondValue;
        private CalcOperator _calcOperator;

        public MainViewModel()
        {
            _culture = CultureInfo.CreateSpecificCulture("en-US");
            _calcOperator = CalcOperator.None;
            CurrentValue = "0";
            ArithmeticalProblem = "";
        }

        #region Public Properties

        private string _currentValue;
        public string CurrentValue
        {
            get { return _currentValue; }
            set { SetField(ref _currentValue, value, "CurrentValue"); }
        }

        private string _arithmeticalProblem;
        public string ArithmeticalProblem
        {
            get { return _arithmeticalProblem; }
            set { SetField(ref _arithmeticalProblem, value, "ArithmeticalProblem"); }
        }

        #endregion

        #region Commands

        private RelayCommand<string> _addNumberCommand;
        public RelayCommand<string> AddNumberCommand
        {
            get
            {
                return _addNumberCommand ?? (_addNumberCommand = new RelayCommand<string>(
                    (number) => {
                        if (_autoClear) Clear();
                        CurrentValue += number;
                        CurrentValue = CurrentValue.TrimStart('0');
                        if (CurrentValue == "") CurrentValue = "0";
                    }));
            }
        }

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand(
                    () => ParseFirstValue(CalcOperator.Add)));
            }
        }

        private RelayCommand _substractCommand;
        public RelayCommand SubstractCommand
        {
            get
            {
                return _substractCommand ?? (_substractCommand = new RelayCommand(
                    () => ParseFirstValue(CalcOperator.Substract)));
            }
        }

        private RelayCommand _multiplyCommand;
        public RelayCommand MultiplyCommand
        {
            get
            {
                return _multiplyCommand ?? (_multiplyCommand = new RelayCommand(
                    () => ParseFirstValue(CalcOperator.Multiply)));
            }
        }

        private RelayCommand _divideCommand;
        public RelayCommand DivideCommand
        {
            get
            {
                return _divideCommand ?? (_divideCommand = new RelayCommand(
                    () => ParseFirstValue(CalcOperator.Divide)));
            }
        }

        private RelayCommand _clearCommand;
        public RelayCommand ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new RelayCommand(() => {
                    _firstValue = 0;
                    _secondValue = 0;
                    _calcOperator = CalcOperator.None;
                    Clear();
                }));
            }
        }

        private RelayCommand _dotCommand;
        public RelayCommand DotCommand
        {
            get
            {
                return _dotCommand ?? (_dotCommand = new RelayCommand(
                    () => {
                        if (_autoClear) Clear();
                        CurrentValue += ".";
                        _dotUsed = true;
                    },
                    () => !_dotUsed));
            }
        }

        private RelayCommand _evaluateCommand;
        public RelayCommand EvaluateCommand
        {
            get
            {
                return _evaluateCommand ?? (_evaluateCommand = new RelayCommand(
                    async () => {
                        try
                        {
                            ParseSecondValue();

                            if (_calcOperator != CalcOperator.None)
                            {
                                //var result = CalcEngine.Evaluate(ArithmeticalProblem);
                                var result = await GetResult();
                                CurrentValue = result.ToString(_culture);
                            }

                            _calcOperator = CalcOperator.None;
                        }
                        catch (Exception ex)
                        {
                            Clear();
                            ArithmeticalProblem = ex.Message;
                        }
                    },
                    () => !string.IsNullOrEmpty(ArithmeticalProblem)));
            }
        }

        #endregion

        private void Clear()
        {
            CurrentValue = "0";
            _dotUsed = false;
            _autoClear = false;
            UpdateArithmeticalProblem(false);
        }

        private async void ParseFirstValue(CalcOperator calcOperator)
        {
            if (_calcOperator != CalcOperator.None)
            {
                ParseSecondValue();
                _firstValue = await GetResult();
            }
            else
            {
                if (!double.TryParse(CurrentValue, NumberStyles.Float, _culture, out _firstValue))
                {
                    ArithmeticalProblem = "Error";
                    _calcOperator = CalcOperator.None;
                    return;
                }
            }
            
            _calcOperator = calcOperator;
            _dotUsed = false;
            _autoClear = true;
            UpdateArithmeticalProblem(false);
        }

        private void ParseSecondValue()
        {
            if (!double.TryParse(CurrentValue, NumberStyles.Float, _culture, out _secondValue))
            {
                ArithmeticalProblem = "Error";
                _calcOperator = CalcOperator.None;
                return;
            }
            _dotUsed = false;
            _autoClear = true;
            UpdateArithmeticalProblem(true);
        }

        private void UpdateArithmeticalProblem(bool displaySecondValue)
        {
            string calcOperator;
            switch (_calcOperator)
            {
                case CalcOperator.Add:
                    calcOperator = "+";
                    break;
                case CalcOperator.Substract:
                    calcOperator = "-";
                    break;
                case CalcOperator.Multiply:
                    calcOperator = "*";
                    break;
                case CalcOperator.Divide:
                    calcOperator = "/";
                    break;
                default:
                    ArithmeticalProblem = "";
                    return;
            }
            ArithmeticalProblem = displaySecondValue
                ? string.Format(_culture, "{0} {1} {2}", _firstValue, calcOperator, _secondValue)
                : string.Format(_culture, "{0} {1}", _firstValue, calcOperator);
        }

        private IStatelessCalculatorActor _proxy;
        protected IStatelessCalculatorActor Proxy
        {
            get
            {
                return _proxy
                    ?? (_proxy = ActorProxy.Create<IStatelessCalculatorActor>(
                        ActorId.NewId(), "fabric:/StatelessCalculatorActorApplication"));
            }
        }

        private async Task<double> GetResult()
        {
            switch (_calcOperator)
            {
                case CalcOperator.Add:
                    return await Proxy.Add(_firstValue, _secondValue);
                case CalcOperator.Substract:
                    return await Proxy.Substract(_firstValue, _secondValue);
                case CalcOperator.Multiply:
                    return await Proxy.Multiply(_firstValue, _secondValue);
                case CalcOperator.Divide:
                    return await Proxy.Divide(_firstValue, _secondValue);
                default:
                    return 0;
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}