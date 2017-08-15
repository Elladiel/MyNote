using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MyNote.DAO;

namespace MyNote.Models
{
    public class Entry : INotifyPropertyChanged
    {
        private String date;
        private String textEntry;
        public int Id { get; set; }


        public String Date
        {
            get{return date;}
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public String TextEntry
        {
            get { return textEntry; }
            set
            {
                textEntry = value;
                OnPropertyChanged("TextEntry");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
