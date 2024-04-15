using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShoppingCart.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Private

        private bool isBusy;
        private string busyTitle;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }

        public string BusyTitle
        {
            get => busyTitle;
            set
            {
                busyTitle = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null) return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}