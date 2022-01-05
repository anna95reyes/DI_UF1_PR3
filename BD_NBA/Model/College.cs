using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NBA_BD.Model
{
    public class College : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private String name;

        public College(string name)
        {
            Name = name;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is College college &&
                   Name == college.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
