using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommitTemplateExtension.ViewModels
{
    public class AddAuthDetailsVM : INotifyPropertyChanged
    {
        private string name;
        private string tokenOrPassword;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Personal access token or password to authenticate with
        /// E.g. generate a token on github and use it   > settings > dev settings > personal access tokens
        /// </summary>
        public string TokenOrPassword
        {
            get { return tokenOrPassword; }
            set
            {
                if (value != tokenOrPassword)
                {
                    tokenOrPassword = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
