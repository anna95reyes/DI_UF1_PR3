using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NBA_BD.Model
{
    public class Division : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private String caption;

        public Division(string caption)
        {
            Caption = caption;
        }

        public string Caption {
            get
            {
                return caption;
            }
            set
            {
                if (!validaCaption(value)) throw new Exception("La caption de Division es obligatoria");
                caption = value;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Division division &&
                   Caption == division.Caption;
        }

        public override int GetHashCode()
        {
            return 287138253 + EqualityComparer<string>.Default.GetHashCode(Caption);
        }

        public override string ToString()
        {
            return Caption;
        }

        public static bool validaCaption(String caption)
        {
            return caption.Length > 0;
        }
    }
}
