using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Helpers
{
   public class DictionaryList<TKey,TListItem>
    {
        Dictionary<TKey, List<TListItem>> dictionary =  new Dictionary<TKey, List<TListItem>>();
        public DictionaryList()
        {
            
            dictionary = new Dictionary<TKey, List<TListItem>>();
        }

        public void Add(TKey key, TListItem item)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, new List<TListItem>());
            }

            var itemlist = dictionary[key];
            itemlist.Add(item);
        }

        public IEnumerable<TListItem> Get(TKey key)
        {
            return dictionary[key];
        }
    }
}
