using DearDiary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DearDiary.ViewModels
{
    public class DiaryViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<DiaryEntry> DiaryEntries { get; set; }

        private DiaryEntry currentEntry;
        public DiaryEntry CurrentEntry
        {
            get
            {
                return currentEntry;
            }
            set
            {
                if (currentEntry != value)
                {
                    currentEntry = value;
                    RaisePropertyChanged();
                }
            }
        }

        public DiaryViewModel()
        {
            SeedData();
        }

        public void AddNewEntry(DiaryEntry newEntry)
        {
            if (DiaryEntries == null)
                DiaryEntries = new ObservableCollection<DiaryEntry>();

            DiaryEntries.Add(newEntry);
        }

        public void DeleteEntry(DiaryEntry entryToDelete)
        {
            if (DiaryEntries != null)
                DiaryEntries.Remove(entryToDelete);
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberNameAttribute] String propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private void SeedData()
        {
            DiaryEntries = new ObservableCollection<DiaryEntry>();
            DiaryEntries.Add(new DiaryEntry { EntryDate = new DateTime(2014, 01, 15), Details = "Snow day today! So relieved I don't have to be out on the roads in this weather." });
            DiaryEntries.Add(new DiaryEntry { EntryDate = new DateTime(2014, 02, 19), Details = "Went out for Mike's birthday. It was a great time!" });
            DiaryEntries.Add(new DiaryEntry { EntryDate = new DateTime(2014, 04, 12), Details = "Tried to make a new recipe for dinner tonight. Nobody liked it." });
            DiaryEntries.Add(new DiaryEntry { EntryDate = new DateTime(2014, 05, 11), Details = "Spent the day cleaning the house only to have my teen destroy it in a matter of minutes. Trampled through the house in dirty shoes." });
            DiaryEntries.Add(new DiaryEntry { EntryDate = new DateTime(2014, 07, 01), Details = "Canada Day. It's nice to have a holiday but I'm bored." });
            DiaryEntries.Add(new DiaryEntry { EntryDate = new DateTime(2014, 07, 20), Details = "Big Music Fest is in town. Didn't get tickets but still listened to Aerosmith from the Tim Horton's parking lot." });
            DiaryEntries.Add(new DiaryEntry { EntryDate = new DateTime(2014, 08, 10), Details = "Heading out to That Conference in Wisconsin! Can't wait to learn all the things! And teach others about stuffs!" });
            
        }
    }
}
