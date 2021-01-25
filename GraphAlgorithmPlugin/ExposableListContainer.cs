using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithmPlugin
{
    public class ExposableListContainer : ObservableCollection<ExposableList>
    {
        public new void Add(ExposableList list)
        {
            list.CollectionChanged += ListItem_CollectionChanged;
            base.Add(list);
        }

        private void ListItem_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
