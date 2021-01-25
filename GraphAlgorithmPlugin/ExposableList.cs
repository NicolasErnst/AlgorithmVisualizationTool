using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public class ExposableList : ObservableCollection<object>
    {
        private string listName = "";

        /// <summary>
        /// 
        /// </summary>
        public string ListName
        {
            get
            {
                return listName;
            }
            set
            {
                if (listName == value)
                {
                    return;
                }

                listName = value;

                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
        public string JoinedListItems
        {
            get
            {
                return "{ " + string.Join(", ", Items.Select(x => x.ToString())) + " }"; 
            }
        }


        public ExposableList(string listName) : this(listName, new List<object>())
        {
            // Nothing to do here
        }

        public ExposableList(string listName, IEnumerable<object> items)
        {
            ListName = listName;
            AddRange(items);
        }


        public void AddRange(IEnumerable<object> items)
        {
            foreach(object item in items) {
                Items.Add(item);
            }
        }
    }
}
