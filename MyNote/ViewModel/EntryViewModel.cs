using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using MyNote.Models;
using System.Collections.Generic;
using MyNote.DAO;
using System.Linq;
namespace MyNote.ViewModel 
{
    public class EntryViewModel : INotifyPropertyChanged
    {
        // команда добавления нового объекта
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      Entry entry = new Entry();
                      Entries.Insert(0, entry);
                      
                      SelectedEntry = entry;
                  }));
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                  (saveCommand = new RelayCommand(obj =>
                  {
                      var id = EntryDap.Save(selectedEntry);
                      selectedEntry.Id = id;
                  }));
            }
        }

        private RelayCommand deleteAllCommand;
        public RelayCommand DeleteAllCommand
        {
            get
            {
                return deleteAllCommand ??
                  (deleteAllCommand = new RelayCommand(obj =>
                  {
                      Entries.Clear();
                      EntryDap.DeleteAll();
                  }));
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand(obj =>
                  {
                      var selEntry = Entries.First(entry => entry.Id == selectedEntry.Id);
                      Entries.Remove(selEntry);
                      EntryDap.Delete(selEntry.Id);
                  }));
            }
        }

        private Entry selectedEntry;

        public Entry SelectedEntry
        {
            get { return selectedEntry; }
            set
            {
                selectedEntry = value;
                OnPropertyChanged("SelectedEntry");
            }
        }

        public EntryViewModel (IEnumerable<Entry> entries)
        {
            Entries = new ObservableCollection<Entry>(entries);
        }

        public ObservableCollection<Entry> Entries { get; set; }
    
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
