using GalaSoft.MvvmLight;

namespace LedControllerEngine.Assets
{
    public class Fan : ObservableObject
    {
        private int _fanIndex;
        public int FanIndex
        {
            get
            {
                return _fanIndex;
            }
            internal set
            {
                _fanIndex = value;
                RaisePropertyChanged(() => FanIndex);
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
