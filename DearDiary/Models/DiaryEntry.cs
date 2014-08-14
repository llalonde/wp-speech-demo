using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DearDiary.Models
{
    public class DiaryEntry : INotifyPropertyChanged
    {
        

        private string details;
        public string Details
        {
            get
            {
                return details;
            }
            set
            {
                if (details != value)
                {
                    details = value;
                    RaisePropertyChanged();
                }

            }
        }

        private DateTime entryDate;
        public DateTime EntryDate
        {
            get
            {
                return entryDate;
            }
            set
            {
                if (entryDate != value)
                {
                    entryDate = value;
                    RaisePropertyChanged();
                }
            }
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
    }
}
