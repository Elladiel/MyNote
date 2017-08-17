using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace MyNote.Models
{
    public class Entry : INotifyPropertyChanged
    {
        private String date;
        private String textEntry;
        public String ImagePath { get; set; }
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
