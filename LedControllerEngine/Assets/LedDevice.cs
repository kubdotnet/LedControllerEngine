using GalaSoft.MvvmLight;

namespace LedControllerEngine.Assets
{
    public class LedDevice : ObservableObject
    {
        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            internal set
            {
                _index = value;
                RaisePropertyChanged(() => Index);
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            internal set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            internal set
            {
                _isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }
    }
}
